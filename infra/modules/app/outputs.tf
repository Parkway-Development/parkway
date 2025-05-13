output "api_app_url" {
  value = azurerm_linux_web_app.api.default_hostname
}

output "frontend_app_url" {
  value = azurerm_linux_web_app.frontend.default_hostname
}
