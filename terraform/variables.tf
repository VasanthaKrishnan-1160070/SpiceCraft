variable "aws_region" {
  description = "AWS region"
  type        = string
  default     = "us-west-2"
}

variable "aws_account_id" {
  description = "AWS account ID"
  type        = string
}

variable "image_tag" {
  description = "Docker image tag"
  type        = string
  default     = "latest"
}

variable "execution_role_arn" {
  description = "ARN of the execution role"
  type        = string
}

variable "task_role_arn" {
  description = "ARN of the task role"
  type        = string
}

variable "subnets" {
  description = "List of subnets IDs"
  type        = list(string)
}

variable "security_group" {
  description = "Security group ID"
  type        = string
}
