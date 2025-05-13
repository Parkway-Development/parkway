terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.0"
    }
  }

  required_version = ">= 1.3.0"
}

provider "azurerm" {
  features {
    resource_group {
      prevent_deletion_if_contains_resources = false
    }
  }

  subscription_id = var.subscription_id
  tenant_id       = var.tenant_id
  client_id       = var.client_id
  client_secret   = var.client_secret
}


data "azurerm_client_config" "current" {}

module "core" {
  source     = "./modules/core"
  prefix     = var.prefix
  location   = var.location
  tenant_id  = var.tenant_id
  object_id  = data.azurerm_client_config.current.object_id
}

data "azurerm_key_vault_secret" "sql_admin_user" {
  name         = "${var.prefix}-sqladminuser"
  key_vault_id = module.core.key_vault_id
}

data "azurerm_key_vault_secret" "sql_admin_pass" {
  name         = "${var.prefix}-sqladminpass"
  key_vault_id = module.core.key_vault_id
}

module "sql" {
  source              = "./modules/sql"
  prefix              = var.prefix
  location            = var.sql_location != "" ? var.sql_location : var.location
  resource_group_name = module.core.resource_group_name
  sql_admin_user      = data.azurerm_key_vault_secret.sql_admin_user.value
  sql_admin_pass      = data.azurerm_key_vault_secret.sql_admin_pass.value
}

module "aks" {
  source              = "./modules/aks"
  prefix              = var.prefix
  location            = var.location
  environment         = var.environment
  resource_group_name = module.core.resource_group_name
}

module "app" {
  source              = "./modules/app"
  prefix              = var.prefix
  location            = var.location
  resource_group_name = module.core.resource_group_name
}