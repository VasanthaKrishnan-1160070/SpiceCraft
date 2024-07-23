output "cluster_id" {
  value = aws_ecs_cluster.spicecraft.id
}

output "service_name" {
  value = aws_ecs_service.spicecraft_service.name
}
