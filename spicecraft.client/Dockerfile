# Stage 1: Build Angular App
FROM node:20.15.1 AS build
WORKDIR /app
COPY package*.json ./

# install dependencies
RUN npm install

# install Angular CLI
RUN npm install -g @angular/cli

# Copy project files
COPY . .

# Build the Angular app in production mode
RUN npm run build --prod

# Stage 2: Serve Angular App with Nginx
FROM nginx:latest

# Copy built Angular app to Nginx's default html directory
COPY --from=build /app/dist/spicecraft.client/browser /usr/share/nginx/html

# Copy custom Nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose port 80 to the outside world
EXPOSE 80

# Start Nginx when the container launches
CMD ["nginx", "-g", "daemon off;"]
