# Stage 1: Build Angular App
FROM node:20.15.1 AS node-build
WORKDIR /app
COPY ./spicecraft.client/package*.json ./
RUN npm install
COPY ./spicecraft.client/ ./
RUN npm run build --prod

# Stage 2: Build .NET Core API with Node.js
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SpiceCraft.Server/SpiceCraft.Server.csproj", "SpiceCraft.Server/"]
# Install Node.js 20.15.1
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - && \
    apt-get install -y nodejs
RUN dotnet restore "./SpiceCraft.Server/SpiceCraft.Server.csproj"
COPY . .
WORKDIR "/src/SpiceCraft.Server"
RUN dotnet build "./SpiceCraft.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish .NET Core API and Copy Angular Build
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SpiceCraft.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Combine Builds
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=node-build /app/dist ./wwwroot

# Expose Ports
EXPOSE 8080
EXPOSE 8081

# Entry Point
ENTRYPOINT ["dotnet", "SpiceCraft.Server.dll"]
