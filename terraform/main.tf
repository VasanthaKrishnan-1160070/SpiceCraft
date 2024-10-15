# Updated ECS Task Definition with Memory Reset to Original Values and ALB Integration

provider "aws" {
  region = var.aws_region
}

# Fetch the Default VPC
data "aws_vpc" "default" {
  default = true
}

# Fetch the Default Subnets for the Default VPC
data "aws_subnets" "default" {
  filter {
    name   = "vpc-id"
    values = [data.aws_vpc.default.id]
  }
}

# Create CloudWatch Log Groups for ECS Containers
resource "aws_cloudwatch_log_group" "spicecraft_client_logs" {
  name              = "/ecs/spicecraft-client-logs"
  retention_in_days = 30
}

resource "aws_cloudwatch_log_group" "spicecraft_server_logs" {
  name              = "/ecs/spicecraft-server-logs"
  retention_in_days = 30
}

# Create ECR Repositories for Images
resource "aws_ecr_repository" "spicecraft_client" {
  name = "spicecraft-client"
}

resource "aws_ecr_repository" "spicecraft_server" {
  name = "spicecraft-server"
}

# Create IAM Role for ECS Task Execution
resource "aws_iam_role" "ecs_task_execution_role" {
  name = "spiceCraftEcsTaskExecutionRole"

  assume_role_policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Effect = "Allow",
        Principal = {
          Service = "ecs-tasks.amazonaws.com"
        },
        Action = "sts:AssumeRole"
      }
    ]
  })

  tags = {
    Name = "ecs_task_execution_role"
  }
}

resource "aws_iam_role_policy_attachment" "ecs_task_execution_policy" {
  role       = aws_iam_role.ecs_task_execution_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
}

# Create IAM Role for ECS Task
resource "aws_iam_role" "ecs_task_role" {
  name = "spiceCraftEcsTaskRole"

  assume_role_policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Effect = "Allow",
        Principal = {
          Service = "ecs-tasks.amazonaws.com"
        },
        Action = "sts:AssumeRole"
      }
    ]
  })

  tags = {
    Name = "ecs_task_role"
  }
}

# Create ECS Cluster
resource "aws_ecs_cluster" "spicecraft" {
  name = "spicecraft-cluster"
}

# Create Security Group for ECS and SQL Server
resource "aws_security_group" "ecs_security_group" {
  vpc_id = data.aws_vpc.default.id

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 8080
    to_port     = 8080
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  # Allow inbound traffic to SQL Server from any IP address
  ingress {
    from_port   = 1433
    to_port     = 1433
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "ecs_security_group"
  }
}

# Create Application Load Balancer
resource "aws_lb" "spicecraft_alb" {
  name               = "spicecraft-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.ecs_security_group.id]
  subnets            = data.aws_subnets.default.ids

  tags = {
    Name = "spicecraft-alb"
  }
}

# Create Target Group for ALB
resource "aws_lb_target_group" "spicecraft_tg" {
  name     = "spicecraft-target-group"
  port     = 80
  protocol = "HTTP"
  vpc_id   = data.aws_vpc.default.id
  target_type = "ip"

  health_check {
    path                = "/"
    protocol            = "HTTP"
    interval            = 30
    timeout             = 5
    healthy_threshold   = 5
    unhealthy_threshold = 2
  }
}

# Create Listener for ALB
resource "aws_lb_listener" "spicecraft_listener" {
  load_balancer_arn = aws_lb.spicecraft_alb.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.spicecraft_tg.arn
  }
}

# Create ECS Task Definition for Fargate
resource "aws_ecs_task_definition" "spicecraft_task" {
  family                   = "spicecraft-task"
  network_mode             = "awsvpc"  # Required for Fargate
  cpu                      = "1024"
  memory                   = "2048"
  execution_role_arn       = aws_iam_role.ecs_task_execution_role.arn
  task_role_arn            = aws_iam_role.ecs_task_role.arn
  requires_compatibilities = ["FARGATE"]

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

# Create ECS Service to Run on Fargate
resource "aws_ecs_service" "spicecraft_service" {
  name            = "spicecraft-service"
  cluster         = aws_ecs_cluster.spicecraft.id
  task_definition = aws_ecs_task_definition.spicecraft_task.arn
  desired_count   = 1
  launch_type     = "FARGATE"
  network_configuration {
    subnets         = data.aws_subnets.default.ids
    security_groups = [aws_security_group.ecs_security_group.id]
  }
  load_balancer {
    target_group_arn = aws_lb_target_group.spicecraft_tg.arn
    container_name   = "spicecraft-client-container"
    container_port   = 80
  }
}