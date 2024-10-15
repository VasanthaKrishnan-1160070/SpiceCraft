#!/bin/bash

# Variables
ANGULAR_BUILD_DIR="../../spicecraft.client/dist/spicecraft.client/browser"
API_BUILD_DIR="../../SpiceCraft.Server/publish"
EC2_USER="ec2-user"
EC2_HOST="ec2-52-62-227-63.ap-southeast-2.compute.amazonaws.com"
PEM_FILE="C:/Users/spec1/.ssh/id_rsa"

# Step 1: Build Angular Project
echo "Building Angular Project..."
npm run build --prefix ../../spicecraft.client

# Step 2: Publish .NET Core API
echo "Publishing .NET Core API..."
dotnet publish ../../Spicecraft.Server -c Release -o ${API_BUILD_DIR}

# Step 3: Upload Angular Build to EC2
echo "Uploading Angular Build to EC2..."
scp -i ${PEM_FILE} -r ${ANGULAR_BUILD_DIR} ${EC2_USER}@${EC2_HOST}:/var/www/angular

# Step 4: Upload .NET Core API Build to EC2
echo "Uploading .NET Core API Build to EC2..."
scp -i ${PEM_FILE} -r ${API_BUILD_DIR} ${EC2_USER}@${EC2_HOST}:/var/www/api

# Step 5: Restart Services on EC2
echo "Restarting services on EC2..."
ssh -i ${PEM_FILE} ${EC2_USER}@${EC2_HOST} "sudo systemctl restart nginx && sudo pkill -f dotnet && nohup dotnet /var/www/api/YourApi.dll &"

echo "Deployment completed successfully."