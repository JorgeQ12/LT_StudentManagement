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
  default = "db.t3.micro"
}

variable "db_allocated_storage" {
  type    = number
  default = 20
}
