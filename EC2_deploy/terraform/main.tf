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

# Create Security Group for EC2 Instance
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

# Create EC2 Instance for Hosting Angular and .NET Core API
# Add the key_name attribute to use an existing key pair
resource "aws_instance" "web_server_instance" {
  ami                    = data.aws_ami.amzn2.id
  instance_type          = "t3.micro"
  vpc_security_group_ids = [aws_security_group.ec2_security_group.id]
  subnet_id              = data.aws_subnets.default.ids[0]
  associate_public_ip_address = true
  key_name               = "spicecraft_key_pair"  # Name of the existing key pair

   user_data = <<-EOF
    #!/bin/bash
    yum update -y
    amazon-linux-extras install nginx1 -y
    systemctl start nginx
    systemctl enable nginx

    # Install .NET SDK
    rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm
    yum install -y dotnet-sdk-8.0

    # Install EC2 Instance Connect
    yum install -y ec2-instance-connect

    # Copy Angular and .NET Core API files (placeholder)
    mkdir -p /var/www/angular
    mkdir -p /var/www/api
    # You need to replace this with your actual deployment script to copy files to these directories

    # Configure Nginx
    cat > /etc/nginx/nginx.conf << 'EOF2'
    server {
        listen       80;
        server_name  localhost;

        location / {
            root   /var/www/angular;
            index  index.html index.htm;
            try_files $uri $uri/ /index.html;
        }

        location /api/ {
            proxy_pass http://localhost:5000/;
            proxy_http_version 1.1;
            proxy_set_header Upgrade $http_upgrade;
            proxy_set_header Connection keep-alive;
            proxy_set_header Host $host;
            proxy_cache_bypass $http_upgrade;
            proxy_set_header X-Real-IP $remote_addr;
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header X-Forwarded-Proto $scheme;
        }
    }
    EOF2

    systemctl restart nginx
  EOF

  tags = {
    Name = "web-server-instance"
  }
}
