# Stage 1: Build Angular App
FROM node:20.15.1 AS build
WORKDIR /app
COPY ./spicecraft.client/package*.json ./
RUN npm install
COPY ./spicecraft.client/ ./
RUN npm run build --prod

# Stage 2: Serve Angular App with Nginx
FROM nginx:alpine
COPY --from=build /app/dist/spicecraft.client /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
