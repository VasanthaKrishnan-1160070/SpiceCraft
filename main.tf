provider "aws" {
  region = var.aws_region
}

# Use the default VPC and its subnets
data "aws_vpc" "default" {
  default = true
}

data "aws_subnets" "default" {
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.default.id]
  }
}

# ECS Cluster
resource "aws_ecs_cluster" "spicecraft" {
  name = "spicecraft-cluster"
}

# ECS Task Definition
resource "aws_ecs_task_definition" "spicecraft_task" {
  family                   = "spicecraft-task"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "512"
  memory                   = "1024"
  execution_role_arn       = var.execution_role_arn
  task_role_arn            = var.task_role_arn

  container_definitions = jsonencode([
    {
      name      = "spicecraft-client-container"
      image     = "${var.aws_account_id}.dkr.ecr.${var.aws_region}.amazonaws.com/spicecraft-client:${var.image_tag}"
      cpu       = 256
      memory    = 512
      essential = true
      portMappings = [
        {
          containerPort = 80
          hostPort      = 80
          protocol      = "tcp"
        }
      ]
    },
    {
      name      = "spicecraft-server-container"
      image     = "${var.aws_account_id}.dkr.ecr.${var.aws_region}.amazonaws.com/spicecraft-server:${var.image_tag}"
      cpu       = 256
      memory    = 512
      essential = true
      portMappings = [
        {
          containerPort = 8080
          hostPort      = 8080
          protocol      = "tcp"
        }
      ]
    }
  ])
}

# ECS Service
resource "aws_ecs_service" "spicecraft_service" {
  name            = "spicecraft-service"
  cluster         = aws_ecs_cluster.spicecraft.id
  task_definition = aws_ecs_task_definition.spicecraft_task.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    subnets         = data.aws_subnets.default.ids
    security_groups = [var.security_group]
    assign_public_ip = true
  }
}

