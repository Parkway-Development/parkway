using System;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;
using Pulumi.AzureNative.Authorization.Inputs;
using Pulumi.AzureNative.Sql;
using Pulumi.AzureNative.Sql.Inputs;
using Pulumi.AzureNative.ContainerService;
using Pulumi.AzureNative.ContainerService.Inputs;
using System.Collections.Generic;
using KvSkuArgs = Pulumi.AzureNative.KeyVault.Inputs.SkuArgs;
using SqlSkuArgs = Pulumi.AzureNative.Sql.Inputs.SkuArgs;

return await Pulumi.Deployment.RunAsync(() =>
{
    var config = new Config();
    var location = config.Require("location");
    var resourcePrefix = config.Require("resourcePrefix");

    var tenantId = Environment.GetEnvironmentVariable("ARM_TENANT_ID") ?? throw new ArgumentNullException("ARM_TENANT_ID");
    // var objectId = Environment.GetEnvironmentVariable("ARM_CLIENT_OBJECT_ID") ?? throw new ArgumentNullException("ARM_CLIENT_OBJECT_ID");
    var objectId = config.Require("clientObjectId");

    // Create the Azure Resource Group
    var resourceGroup = new ResourceGroup($"{resourcePrefix}-rg", new ResourceGroupArgs
    {
        ResourceGroupName = $"{resourcePrefix}-rg",
        Location = location
    });

    // Create the Azure Key Vault
    var keyVault = new Vault($"{resourcePrefix}-kv", new VaultArgs
    {
        VaultName = $"{resourcePrefix}-kv",
        Location = location,
        ResourceGroupName = resourceGroup.Name,
        Properties = new VaultPropertiesArgs
        {
            TenantId = tenantId,
            Sku = new KvSkuArgs
            {
                Family = "A",
                Name = SkuName.Standard
            },
            AccessPolicies =
            {
                new AccessPolicyEntryArgs
                {
                    TenantId = tenantId,
                    ObjectId = objectId,
                    Permissions = new PermissionsArgs
                    {
                        Secrets = new InputList<Union<string, SecretPermissions>>
                        {
                            "Get",
                            "List",
                            "Set",
                            "Delete",
                            "Purge"
                        }
                    }
                }
            },
            EnabledForDeployment = true,
            EnabledForTemplateDeployment = true,
            EnabledForDiskEncryption = true,
            PublicNetworkAccess = "Enabled"
        }
    });

    // Get secrets from config
    var sqlAdminUsername = config.Require("sqlAdminUsername");
    var sqlAdminPassword = config.RequireSecret("sqlAdminPassword");

    // SQL Server
    var sqlServer = new Server($"{resourcePrefix}-sqlserver", new ServerArgs
    {
        ServerName = $"{resourcePrefix}-sqlserver",
        ResourceGroupName = resourceGroup.Name,
        Location = location,
        AdministratorLogin = sqlAdminUsername,
        AdministratorLoginPassword = sqlAdminPassword,
        Version = "12.0",
    });

    // SQL Database
    var sqlDatabase = new Database($"{resourcePrefix}-sqldb", new DatabaseArgs
    {
        DatabaseName = $"{resourcePrefix}-sqldb",
        ResourceGroupName = resourceGroup.Name,
        ServerName = sqlServer.Name,
        Sku = new SqlSkuArgs
        {
            Name = "Basic",
            Tier = "Basic"
        }
    });

    // Build the SQL connection string dynamically
    var sqlConnectionString = Output.Tuple<string, string, string>(
        sqlServer.Name,
        sqlAdminUsername,
        sqlAdminPassword
    ).Apply(values =>
    {
        var (serverName, user, pass) = values;
        return $"Server=tcp:{serverName}.database.windows.net,1433;Initial Catalog={resourcePrefix}-sqldb;Persist Security Info=False;User ID={user};Password={pass};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    });

    // Create Key Vault Secrets
    var sqlAdminUserSecret = new Secret($"{resourcePrefix}-sqladminuser", new SecretArgs
    {
        VaultName = keyVault.Name,
        ResourceGroupName = resourceGroup.Name,
        Properties = new SecretPropertiesArgs { Value = sqlAdminUsername }
    });

    var sqlAdminPwdSecret = new Secret($"{resourcePrefix}-sqladminpwd", new SecretArgs
    {
        VaultName = keyVault.Name,
        ResourceGroupName = resourceGroup.Name,
        Properties = new SecretPropertiesArgs { Value = sqlAdminPassword }
    });

    var sqlConnStrSecret = new Secret($"{resourcePrefix}-sqlconnstr", new SecretArgs
    {
        VaultName = keyVault.Name,
        ResourceGroupName = resourceGroup.Name,
        Properties = new SecretPropertiesArgs { Value = sqlConnectionString }
    });

    // AKS Cluster
    var aksCluster = new ManagedCluster($"{resourcePrefix}-aks", new ManagedClusterArgs
    {
        ResourceGroupName = resourceGroup.Name,
        Location = location,
        DnsPrefix = $"{resourcePrefix}-k8s",
        AgentPoolProfiles =
        {
            new ManagedClusterAgentPoolProfileArgs
            {
                Name = "agentpool",
                Count = 1,
                VmSize = "Standard_B2s",
                Mode = "System",
                OsType = "Linux",
            }
        },
        Identity = new ManagedClusterIdentityArgs
        {
            Type = Pulumi.AzureNative.ContainerService.ResourceIdentityType.SystemAssigned
        },
    });

    // Export outputs
    return new Dictionary<string, object?>
    {
        ["resourceGroupName"] = resourceGroup.Name,
        ["keyVaultName"] = keyVault.Name,
        ["keyVaultUri"] = keyVault.Properties.Apply(p => p.VaultUri),
        ["sqlServerName"] = sqlServer.Name,
        ["sqlDatabaseName"] = sqlDatabase.Name,
        ["sqlAdminUserSecretName"] = sqlAdminUserSecret.Name,
        ["sqlAdminPwdSecretName"] = sqlAdminPwdSecret.Name,
        ["sqlConnStrSecretName"] = sqlConnStrSecret.Name
    };
});
