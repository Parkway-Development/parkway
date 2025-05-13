resource "azurerm_mssql_server" "sql" {
  name                         = "${var.prefix}-sqlserver"
  location                     = var.location
  resource_group_name          = var.resource_group_name
  version                      = "12.0"
  administrator_login          = var.sql_admin_user
  administrator_login_password = var.sql_admin_pass
  minimum_tls_version          = "1.2"
  public_network_access_enabled = true
}

resource "azurerm_mssql_database" "db" {
  name           = "${var.prefix}-sqldb"
  server_id      = azurerm_mssql_server.sql.id
  sku_name       = "Basic"
  storage_account_type = "Geo"
  geo_backup_enabled   = true
}
