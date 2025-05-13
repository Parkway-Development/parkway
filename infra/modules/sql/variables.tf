variable "prefix" {
  description = "Prefix for naming resources"
  type        = string
}

variable "location" {
  description = "Azure region"
  type        = string
}

variable "resource_group_name" {
  description = "Resource group where SQL resources will live"
  type        = string
}

variable "sql_admin_user" {
  description = "SQL server admin username"
  type        = string
}

variable "sql_admin_pass" {
  description = "SQL server admin password"
  type        = string
  sensitive   = true
}
