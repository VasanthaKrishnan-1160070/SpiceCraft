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

# Create CloudWatch Log Groups
resource "aws_cloudwatch_log_group" "spicecraft_client_logs" {
  name              = "/ecs/spicecraft-client-logs"
  retention_in_days = 30
}

resource "aws_cloudwatch_log_group" "spicecraft_server_logs" {
  name              = "/ecs/spicecraft-server-logs"
  retention_in_days = 30
}

resource "aws_cloudwatch_log_group" "mssql_logs" {
  name              = "/ecs/mssql-logs"
  retention_in_days = 30
}

# Create ECR Repositories
resource "aws_ecr_repository" "spicecraft_client" {
  name = "spicecraft-client"

  image_tag_mutability = "MUTABLE"

  image_scanning_configuration {
    scan_on_push = true
  }

  tags = {
    Name = "spicecraft-client"
   
  }
}

resource "aws_ecr_repository" "spicecraft_server" {
  name = "spicecraft-server"

  image_tag_mutability = "MUTABLE"

  image_scanning_configuration {
    scan_on_push = true
  }

  tags = {
    Name = "spicecraft-server"
  }
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

resource "aws_ecs_cluster" "spicecraft" {
  name = "spicecraft-cluster"
}

resource "aws_ecs_task_definition" "spicecraft_task" {
  family                   = "spicecraft-task"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "1024"
  memory                   = "2048"
  execution_role_arn       = aws_iam_role.ecs_task_execution_role.arn
  task_role_arn            = aws_iam_role.ecs_task_role.arn

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
          hostPort      = 80
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
          hostPort      = 8080
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
    },
    {
      name      = "mssql-container"
      image     = "mcr.microsoft.com/mssql/server:2022-latest"
      cpu       = 512
      memory    = 1024
      essential = true
      portMappings = [
        {
          containerPort = 1433
          hostPort      = 1433
          protocol      = "tcp"
        }
      ]
      environment = [
        {
          name  = "ACCEPT_EULA"
          value = "Y"
        },
        {
          name  = "MSSQL_SA_PASSWORD"
          value = var.mssql_sa_password
        },
        {
          name  = "MSSQL_PID"
          value = "Developer"
        }
      ]
      logConfiguration = {
        logDriver = "awslogs"
        options = {
          awslogs-group         = aws_cloudwatch_log_group.mssql_logs.name
          awslogs-region        = var.aws_region
          awslogs-stream-prefix = "mssql"
        }
      }
    }
  ])
}

resource "aws_ecs_service" "spicecraft_service" {
  name            = "spicecraft-service"
  cluster         = aws_ecs_cluster.spicecraft.id
  task_definition = aws_ecs_task_definition.spicecraft_task.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    subnets         = data.aws_subnets.default.ids
    security_groups = [aws_security_group.ecs_security_group.id]
    assign_public_ip = true
  }
}

resource "aws_ecr_lifecycle_policy" "spicecraft_client_lifecycle" {
  repository = aws_ecr_repository.spicecraft_client.name

  policy = jsonencode({
    rules = [
      {
        rulePriority = 1
        description  = "Expire untagged images older than 1 day"
        selection    = {
          tagStatus     = "untagged"
          countType     = "sinceImagePushed"
          countUnit     = "days"
          countNumber   = 1
        }
        action       = {
          type = "expire"
        }
      }
    ]
  })
}

resource "aws_ecr_lifecycle_policy" "spicecraft_server_lifecycle" {
  repository = aws_ecr_repository.spicecraft_server.name

  policy = jsonencode({
    rules = [
      {
        rulePriority = 1
        description  = "Expire untagged images older than 1 day"
        selection    = {
          tagStatus     = "untagged"
          countType     = "sinceImagePushed"
          countUnit     = "days"
          countNumber   = 1
        }
        action       = {
          type = "expire"
        }
      }
    ]
  })
}
