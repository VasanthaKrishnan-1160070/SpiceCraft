# Define AWS region
variable "aws_region" {
  description = "The AWS region to deploy resources in"
  type        = string
  default     = "ap-southeast-2"
}

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

# Fetch the Latest Amazon Linux 2 AMI
data "aws_ami" "amzn2" {
  most_recent = true
  owners      = ["amazon"]

  filter {
    name   = "name"
    values = ["amzn2-ami-hvm-*-x86_64-gp2"]
  }
}

# Create Security Group for EC2 Instance and RDS
resource "aws_security_group" "ec2_security_group" {
  vpc_id = data.aws_vpc.default.id

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 5000
    to_port     = 5000
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = 1433  # MSSQL default port
    to_port     = 1433
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]  # Allows connection to RDS from anywhere, modify for your needs
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "ec2_security_group"
  }
}

# Create IAM Role for EC2 Instance
resource "aws_iam_role" "ec2_instance_role" {
  name = "ec2_instance_role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Effect = "Allow",
        Principal = {
          Service = "ec2.amazonaws.com"
        },
        Action = "sts:AssumeRole"
      }
    ]
  })

  tags = {
    Name = "ec2_instance_role"
  }
}

# Attach Policies to allow S3 access
resource "aws_iam_role_policy_attachment" "ec2_instance_policy_s3_access" {
  role       = aws_iam_role.ec2_instance_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonS3FullAccess"
}

# Allow EC2 instance to access RDS (you can add more policies if needed)
resource "aws_iam_role_policy_attachment" "ec2_instance_policy_rds_access" {
  role       = aws_iam_role.ec2_instance_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonRDSFullAccess"
}

# Create IAM Instance Profile for EC2
resource "aws_iam_instance_profile" "ec2_instance_profile" {
  name = "ec2_instance_profile"
  role = aws_iam_role.ec2_instance_role.name
}

# Create S3 Buckets for SpiceCraft
resource "aws_s3_bucket" "spicecraft" {
  bucket = "spicecraft"
  acl    = "private"
}

# Create sub-buckets inside SpiceCraft
resource "aws_s3_bucket_object" "items_bucket" {
  bucket = aws_s3_bucket.spicecraft.bucket
  key    = "items/"
}

resource "aws_s3_bucket_object" "profiles_bucket" {
  bucket = aws_s3_bucket.spicecraft.bucket
  key    = "profiles/"
}

resource "aws_s3_bucket_object" "common_bucket" {
  bucket = aws_s3_bucket.spicecraft.bucket
  key    = "common/"
}

# Create a DB Subnet Group for RDS
resource "aws_db_subnet_group" "spicecraft_rds_subnet_group" {
  name       = "spicecraft-rds-subnet-group"
  subnet_ids = data.aws_subnets.default.ids

  tags = {
    Name = "spicecraft-rds-subnet-group"
  }
}

# Create RDS MSSQL Server Express Edition - Free Tier
resource "aws_db_instance" "spicecraft_rds" {
  allocated_storage    = 20   # Free tier allows up to 20GB
  storage_type         = "gp2"  # General-purpose SSD, required for free tier
  db_subnet_group_name = aws_db_subnet_group.spicecraft_rds_subnet_group.name  # Use the subnet group
  engine               = "sqlserver-ex"
  engine_version       = "15.00.4043.16.v1"  # Specific version for free tier MSSQL Express
  instance_class       = "db.t3.micro"  # Free tier eligible instance class 
  username             = "admin"  # Customize the username
  password             = "Admin123"  # Customize the password
  port                 = 1433
  publicly_accessible  = true
  skip_final_snapshot  = true  # Avoids charges for a snapshot upon deletion
  delete_automated_backups = true
  vpc_security_group_ids = [aws_security_group.ec2_security_group.id]

  tags = {
    Name = "spicecraft-rds"
  }
}


# Create EC2 Instance
resource "aws_instance" "web_server_instance" {
  ami                    = data.aws_ami.amzn2.id
  instance_type          = "t2.micro"
  vpc_security_group_ids = [aws_security_group.ec2_security_group.id]
  subnet_id              = data.aws_subnets.default.ids[0]
  associate_public_ip_address = true
  key_name               = "spicecraft_key_pair"  # Name of the existing key pair
  iam_instance_profile   = aws_iam_instance_profile.ec2_instance_profile.name

  user_data = <<-EOF
    #!/bin/bash
    yum update -y
    amazon-linux-extras install nginx1 -y
    systemctl start nginx
    systemctl enable nginx

    # Install .NET SDK and Hosting Bundle for .NET 8
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 8.0 --install-dir /usr/share/dotnet
    ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

    # Install EC2 Instance Connect
    yum install -y ec2-instance-connect

    # Create directories for Angular and .NET Core API files
    mkdir -p /var/www/angular
    mkdir -p /var/www/api
    # Deployment script will copy files to these directories

    # Configure Nginx
    cat > /etc/nginx/nginx.conf <<-EOF2
user nginx;
worker_processes auto;
error_log /var/log/nginx/error.log;
pid /run/nginx.pid;

events {
    worker_connections 1024;
}

http {
    include /etc/nginx/mime.types;
    default_type application/octet-stream;

    sendfile on;
    keepalive_timeout 65;

    server {
        listen       80;
        server_name  localhost;

        location / {
            root   /var/www/angular;
            index  index.html index.htm;
            try_files \$uri \$uri/ /index.html;
        }

        location /api {
            proxy_pass http://localhost:5000;
            proxy_http_version 1.1;
            proxy_set_header Upgrade \$http_upgrade;
            proxy_set_header Connection keep-alive;
            proxy_set_header Host \$host;
            proxy_cache_bypass \$http_upgrade;
            proxy_set_header X-Real-IP \$remote_addr;
            proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
            proxy_set_header X-Forwarded-Proto \$scheme;
        }
    }
}
EOF2

    systemctl restart nginx

    # Create systemd service for .NET Core API
    cat > /etc/systemd/system/spicecraft-api.service <<-EOF3
    [Unit]
    Description=SpiceCraft .NET Core API
    After=network.target

    [Service]
    WorkingDirectory=/var/www/api
    ExecStart=/usr/bin/dotnet /var/www/api/SpiceCraft.Server.dll
    Restart=always
    RestartSec=10
    SyslogIdentifier=spicecraft-api
    User=ec2-user
    Environment=ASPNETCORE_ENVIRONMENT=Production
    Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

    [Install]
    WantedBy=multi-user.target
    EOF3

    # Start and enable the .NET Core API service
    systemctl daemon-reload
    systemctl enable spicecraft-api.service
    systemctl start spicecraft-api.service
  EOF

  tags = {
    Name = "web-server-instance"
  }
}

# Create Application Load Balancer
resource "aws_lb" "spicecraft_lb" {
  name               = "spicecraft-lb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.ec2_security_group.id]
  subnets            = data.aws_subnets.default.ids

  tags = {
    Name = "spicecraft-lb"
  }
}

# Create Target Group
resource "aws_lb_target_group" "spicecraft_target_group" {
  name        = "spicecraft-target-group"
  port        = 80
  protocol    = "HTTP"
  vpc_id      = data.aws_vpc.default.id
  target_type = "instance"

  health_check {
    path                = "/"
    port                = "80"
    protocol            = "HTTP"
    interval            = 30
    timeout             = 5
    healthy_threshold   = 5
    unhealthy_threshold = 2
  }

  tags = {
    Name = "spicecraft-target-group"
  }
}

# Attach EC2 Instance to Target Group
resource "aws_lb_target_group_attachment" "spicecraft_attachment" {
  target_group_arn = aws_lb_target_group.spicecraft_target_group.arn
  target_id        = aws_instance.web_server_instance.id
  port             = 80
}

# Create Load Balancer Listener
resource "aws_lb_listener" "spicecraft_listener" {
  load_balancer_arn = aws_lb.spicecraft_lb.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.spicecraft_target_group.arn
  }
}

# Create a Route 53 Hosted Zone for vidhyamohan.com
resource "aws_route53_zone" "spicecraft_hosted_zone" {
  name = "vidhyamohan.com"

  tags = {
    Name = "spicecraft-hosted-zone"
  }
}

# Create Route 53 A Record to point to the Load Balancer
resource "aws_route53_record" "spicecraft_a_record" {
  zone_id = aws_route53_zone.spicecraft_hosted_zone.zone_id
  name    = "www"  # Creates 'www.vidhyamohan.com'
  type    = "A"

  alias {
    name                   = aws_lb.spicecraft_lb.dns_name
    zone_id                = aws_lb.spicecraft_lb.zone_id
    evaluate_target_health = false
  }
}

# Create Route 53 A Record for root domain
resource "aws_route53_record" "spicecraft_root_a_record" {
  zone_id = aws_route53_zone.spicecraft_hosted_zone.zone_id
  name    = "vidhyamohan.com"
  type    = "A"

  alias {
    name                   = aws_lb.spicecraft_lb.dns_name
    zone_id                = aws_lb.spicecraft_lb.zone_id
    evaluate_target_health = false
  }
}
