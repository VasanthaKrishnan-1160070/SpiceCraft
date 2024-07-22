# Stage 1: Build the Angular application
FROM node:20.15.1 AS build

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json to the working directory
COPY package*.json ./

# Install the dependencies
RUN npm install

# Copy the rest of the application code to the working directory
COPY . .

# Build the Angular application
RUN npm run build --prod

# Stage 2: Serve the application with Node.js
FROM node:20.15.1

# Set the working directory
WORKDIR /app

# Copy the built Angular application from the previous stage
COPY --from=build /app/dist/spicecraft.client/browser ./dist

# Install a simple HTTP server
RUN npm install -g http-server

# Expose port 8080
EXPOSE 80

# Start the HTTP server
CMD ["http-server", "dist", "-p", "8080"]
