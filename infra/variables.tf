variable "prefix" {
  description = "Prefix used for all resources (e.g., parkway-dev)"
  type        = string
}

variable "environment" {
  description = "Environment name (e.g., dev, prod)"
  type        = string
}

variable "location" {
  description = "Primary Azure region"
  type        = string
  default     = "southcentralus"
}

variable "sql_location" {
  description = "Region for Azure SQL (can differ from primary location)"
  type        = string
  default     = ""
}

# Azure authentication via environment or tfvars
variable "subscription_id" {
  description = "Azure subscription ID"
  type        = string
}

variable "tenant_id" {
  description = "Azure tenant ID"
  type        = string
}

variable "client_id" {
  description = "Azure service principal client ID"
  type        = string
  sensitive   = true
}

variable "client_secret" {
  description = "Azure service principal client secret"
  type        = string
  sensitive   = true
}
