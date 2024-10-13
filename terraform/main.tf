# Create ECS Task Definition for Fargate with EFS Mount
resource "aws_ecs_task_definition" "spicecraft_task" {
  family                   = "spicecraft-task"
  network_mode             = "awsvpc"  # Required for Fargate
  cpu                      = "1024"   # Example CPU configuration for Fargate
  memory                   = "2048"   # Example Memory configuration for Fargate
  execution_role_arn       = aws_iam_role.ecs_task_execution_role.arn
  task_role_arn            = aws_iam_role.ecs_task_role.arn
  requires_compatibilities = ["FARGATE"]  # Fargate Compatibility

  # Define EFS volume for Fargate
  volume {
    name = "uploads-volume"

    efs_volume_configuration {
      file_system_id     = aws_efs_file_system.spicecraft_efs.id  # Reference the EFS File System ID
      transit_encryption = "ENABLED"  # Enable encryption in transit
      root_directory     = "/"        # Mount the root directory or specify subfolders if needed
    }
  }

  container_definitions = jsonencode([
    {
      name      = "spicecraft-client-container"
      image     = "${aws_ecr_repository.spicecraft_client.repository_url}:latest"
      cpu       = 256
      memory    = 512
      essential = true
      portMappings = [
        {
          containerPort = 80
          protocol      = "tcp"
        }
      ]
      mountPoints = [
        {
          sourceVolume  = "uploads-volume"
          containerPath = "/uploads/items"
          readOnly      = false
        }
      ]
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          awslogs-group         = aws_cloudwatch_log_group.spicecraft_client_logs.name
          awslogs-region        = var.aws_region
          awslogs-stream-prefix = "spicecraft-client"
        }
      }
    },
    {
      name      = "spicecraft-server-container"
      image     = "${aws_ecr_repository.spicecraft_server.repository_url}:latest"
      cpu       = 256
      memory    = 512
      essential = true
      portMappings = [
        {
          containerPort = 8080
          protocol      = "tcp"
        }
      ]
      mountPoints = [
        {
          sourceVolume  = "uploads-volume"
          containerPath = "/uploads/profiles"
          readOnly      = false
        },
        {
          sourceVolume  = "uploads-volume"
          containerPath = "/uploads/common"
          readOnly      = false
        }
      ]
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          awslogs-group         = aws_cloudwatch_log_group.spicecraft_server_logs.name
          awslogs-region        = var.aws_region
          awslogs-stream-prefix = "spicecraft-server"
        }
      }
    }
  ])
}
