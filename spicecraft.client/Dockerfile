# Stage 1: Build Angular App
FROM node:20.15.1 AS build
WORKDIR /app
COPY package*.json ./
RUN npm cache clean --force
#### install angular cli
RUN npm install -g @angular/cli
RUN npm install
COPY . .
#installing deps for project
RUN npm install
RUN npm run build --prod

# Stage 2: Serve Angular App with Nginx
FROM nginx:latest
COPY --from=build /app/dist/spicecraft.client/browser /usr/share/nginx/html
# copying nginx config from local to image
COPY /nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
