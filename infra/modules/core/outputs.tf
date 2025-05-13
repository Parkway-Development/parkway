output "resource_group_name" {
  value = azurerm_resource_group.main.name
}

output "key_vault_id" {
  value = azurerm_key_vault.kv.id
}
