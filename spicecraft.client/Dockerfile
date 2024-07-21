# Use a node image to build the Angular application
FROM node:20.15.1 AS node-build

WORKDIR /app

# Install dependencies
COPY package.json package-lock.json ./
RUN npm install

# Copy the rest of the application code
COPY . .

# Build the application
RUN npm run build --prod

# Use an Nginx image to serve the application
FROM nginx:alpine

COPY --from=build /app/dist/spicecraft.client /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
