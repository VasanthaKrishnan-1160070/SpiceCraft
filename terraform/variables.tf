variable "aws_region" {
  description = "AWS region"
  type        = string
  default = "ap-southeast-2"
}

variable "aws_account_id" {
  description = "AWS account ID"
  type        = string
  default = "603763212525"
}

variable "image_tag" {
  description = "Docker image tag"
  type        = string
  default     = "latest"
}

variable "mssql_sa_password" {
  description = "MSSQL server password"
  type        = string
  default     = "Admin@123"
}