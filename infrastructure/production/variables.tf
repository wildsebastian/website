variable "db_admin_password" {
  type = string
  description = "The database password for the admin user"
}

variable "environment" {
  type = string
  default = "Production"
  description = "The environment name"
}

variable "github_client_id" {
  type = string
  description = "The GitHub OAuth client ID"
}

variable "github_client_secret" {
  type = string
  description = "The GitHub OAuth client secret"
}

variable "forwarded_headers_enabled" {
  type = bool
  default = true
  description = "Whether to enable forwarded headers"
}