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

# Allocate Elastic IP
resource "aws_eip" "web_server_eip" {
  instance = aws_instance.web_server_instance.id
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
    from_port   = 5114
    to_port     = 5114
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

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
    Name = "ec2_security_group"
  }
}

# Create IAM Role for EC2 Instance to Access Database
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

# Attach Policies to Allow EC2 Access to Database and External Connectivity
resource "aws_iam_role_policy_attachment" "ec2_instance_policy_attachment" {
  role       = aws_iam_role.ec2_instance_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonRDSFullAccess"
}

resource "aws_iam_role_policy_attachment" "ec2_instance_external_db_access" {
  role       = aws_iam_role.ec2_instance_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonEC2ReadOnlyAccess"
}

# Attach AmazonSSMManagedInstanceCore policy
resource "aws_iam_role_policy_attachment" "ec2_instance_ssm" {
  role       = aws_iam_role.ec2_instance_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonSSMManagedInstanceCore"
}

# Create IAM Instance Profile for EC2
resource "aws_iam_instance_profile" "ec2_instance_profile" {
  name = "ec2_instance_profile"
  role = aws_iam_role.ec2_instance_role.name
}

# Create EC2 Instance for Hosting Angular, .NET Core API, and MSSQL
# Add the key_name attribute to use an existing key pair
resource "aws_instance" "web_server_instance" {
  ami                    = data.aws_ami.amzn2.id
  instance_type = "t2.micro"
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
    export PATH=\$PATH:/root/.dotnet/tools

    # Install EC2 Instance Connect
    yum install -y ec2-instance-connect

    # Install MSSQL Server Express
    # curl -o /etc/yum.repos.d/mssql-server.repo https://packages.microsoft.com/config/rhel/9/mssql-server-2022.repo
    # yum localinstall -y /tmp/mssql_server.rpm
    # ACCEPT_EULA=Y yum install -y mssql-server
    # ACCEPT_EULA=Y MSSQL_SA_PASSWORD='Admin123' /opt/mssql/bin/mssql-conf setup
    # systemctl enable mssql-server
    # systemctl start mssql-server

    # Create directories for Angular and .NET Core API files
    mkdir -p /var/www/angular
    mkdir -p /var/www/api
    # Deployment script will copy files to these directories

    # Configure Nginx
    sudo cat > /etc/nginx/nginx.conf <<-EOF2
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
