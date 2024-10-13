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

# Create Security Group for ECS and EFS
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

  ingress {
    from_port   = 1433
    to_port     = 1433
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 2049
    to_port     = 2049
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]  # Required for EFS
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

# Create EFS File System
resource "aws_efs_file_system" "spicecraft_efs" {
  creation_token = "spicecraft-efs"
  lifecycle_policy {
    transition_to_ia = "AFTER_30_DAYS"  # Transition to Infrequent Access after 30 days
  }
  tags = {
    Name = "spicecraft-efs"
  }
}

# Create EFS Mount Targets in each Subnet
resource "aws_efs_mount_target" "spicecraft_efs_mount" {
  for_each = toset(data.aws_subnets.default.ids)

  file_system_id  = aws_efs_file_system.spicecraft_efs.id
  subnet_id       = each.value
  security_groups = [aws_security_group.ecs_security_group.id]
}

# Create ECS Cluster
resource "aws_ecs_cluster" "spicecraft" {
  name = "spicecraft-cluster"
}

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
      file_system_id     = aws_efs_file_system.spicecraft_efs.id
      transit_encryption = "ENABLED"
      root_directory     = "/"  # Or specify a specific directory like "/uploads" if needed
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

# Create ECS Service for Fargate
resource "aws_ecs_service" "spicecraft_service" {
  name            = "spicecraft-service"
  cluster         = aws_ecs_cluster.spicecraft.id
  task_definition = aws_ecs_task_definition.spicecraft_task.arn
  desired_count   = 1
  launch_type     = "FARGATE"  # Changed to Fargate

  network_configuration {
    subnets          = data.aws_subnets.default.ids
    security_groups  = [aws_security_group.ecs_security_group.id]
    assign_public_ip = true  # Set to true or false based on your requirements
  }

  tags = {
    Name = "spicecraft-service"
  }
}

# Create RDS instance for SQL Server Express
resource "aws_db_instance" "spicecraft_db" {
  allocated_storage    = 20
  storage_type         = "gp2"
  engine               = "sqlserver-ex"  
  engine_version       = "15.00.4043.16.v1"  # Example version; adjust as needed
  instance_class       = "db.t3.micro"
  username             = var.mssql_username
  password             = var.mssql_sa_password
  parameter_group_name = "default.sqlserver-ex-15.0"
  publicly_accessible  = true
  vpc_security_group_ids = [aws_security_group.ecs_security_group.id]
  db_subnet_group_name = aws_db_subnet_group.spicecraft_subnet_group.name

  # Backup settings
  backup_retention_period = 7
  backup_window           = "03:00-06:00"

  tags = {
    Name = "spicecraft-sql-server-express"
  }
}

# Create DB Subnet Group for RDS
resource "aws_db_subnet_group" "spicecraft_subnet_group" {
  name       = "spicecraft-subnet-group"
  subnet_ids = data.aws_subnets.default.ids

  tags = {
    Name = "spicecraft-subnet-group"
  }
}
