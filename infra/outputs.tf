output "resource_group_name" {
  value = module.core.resource_group_name
}

output "sql_server_name" {
  value = module.sql.sql_server_name
}

output "aks_cluster_name" {
  value = module.aks.aks_cluster_name
}

output "frontend_url" {
  value = module.app.frontend_app_url
}

output "api_url" {
  value = module.app.api_app_url
}
