output "execution_role_arn" {
  value = aws_iam_role.ecs_task_execution_role.arn
}

output "task_role_arn" {
  value = aws_iam_role.ecs_task_role.arn
}

output "cluster_id" {
  value = aws_ecs_cluster.spicecraft.id
}

output "service_name" {
  value = aws_ecs_service.spicecraft_service.name
}

output "security_group_id" {
  value = aws_security_group.ecs_security_group.id
}
