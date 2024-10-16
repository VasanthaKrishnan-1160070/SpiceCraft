#!/bin/bash

# Variables
ANGULAR_BUILD_DIR="../../spicecraft.client/dist/spicecraft.client/browser"
API_BUILD_DIR="../../SpiceCraft.Server/publish"
EC2_USER="ec2-user"
EC2_HOST="ec2-54-206-237-126.ap-southeast-2.compute.amazonaws.com"
PEM_FILE="./spicecraft_key_pair.pem"

echo "Creating directories and setting permissions on EC2..."
ssh -i ${PEM_FILE} ${EC2_USER}@${EC2_HOST} "sudo mkdir -p /var/www/angular /var/www/api && sudo chmod -R 777 /var/www/angular /var/www/api"


# Step 1: Build Angular Project
echo "Building Angular Project..."
npm run build --prefix ../../spicecraft.client

# Step 2: Publish .NET Core API
echo "Publishing .NET Core API..."
dotnet publish ../../Spicecraft.Server -c Release -o ${API_BUILD_DIR}

# Step 3: Delete existing files in Angular and API directories on EC2
echo "Deleting existing files in /var/www/angular and /var/www/api on EC2..."
ssh -i ${PEM_FILE} ${EC2_USER}@${EC2_HOST} "sudo rm -rf /var/www/angular/* && sudo rm -rf /var/www/api/*"

# Step 4: Upload Angular Build to EC2 (only contents of browser directory)
echo "Uploading Angular Build to EC2..."
scp -i ${PEM_FILE} -r ${ANGULAR_BUILD_DIR}/* ${EC2_USER}@${EC2_HOST}:/var/www/angular
NUM_FILES_UPLOADED_ANGULAR=$(ssh -i ${PEM_FILE} ${EC2_USER}@${EC2_HOST} "find /var/www/angular -type f | wc -l")
echo "Number of files uploaded to /var/www/angular: ${NUM_FILES_UPLOADED_ANGULAR}"

# Step 5: Upload .NET Core API Build to EC2 (only contents of publish directory)
echo "Uploading .NET Core API Build to EC2..."
scp -i ${PEM_FILE} -r ${API_BUILD_DIR}/* ${EC2_USER}@${EC2_HOST}:/var/www/api
NUM_FILES_UPLOADED_API=$(ssh -i ${PEM_FILE} ${EC2_USER}@${EC2_HOST} "find /var/www/api -type f | wc -l")
echo "Number of files uploaded to /var/www/api: ${NUM_FILES_UPLOADED_API}"

# Step 6: Restart Services on EC2
echo "Restarting services on EC2..."
ssh -i ${PEM_FILE} ${EC2_USER}@${EC2_HOST} "sudo systemctl restart nginx && sudo pkill -f dotnet || true && nohup dotnet /var/www/api/SpiceCraft.Server.dll &"

echo "Deployment completed successfully."
