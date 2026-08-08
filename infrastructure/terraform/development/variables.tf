variable "aws_region" {
  type = string
}

variable "project_name" {
  type    = string
  default = "student-management"
}

variable "lambda_package_path" {
  type = string
}

variable "db_instance_class" {
  type    = string
  default = "db.t3.medium"
}

variable "db_allocated_storage" {
  type    = number
  default = 20
}

variable "bootstrap_administrator_email" {
  type    = string
  default = "admin@studentmanagement.local"
}

variable "apply_database_migrations" {
  type    = bool
  default = true
}
