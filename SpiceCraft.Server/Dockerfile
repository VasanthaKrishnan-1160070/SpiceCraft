# Stage 1: Build .NET Core API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SpiceCraft.Server/SpiceCraft.Server.csproj", "SpiceCraft.Server/"]
RUN dotnet restore "./SpiceCraft.Server/SpiceCraft.Server.csproj"
COPY . .
WORKDIR "/src/SpiceCraft.Server"
RUN dotnet build "./SpiceCraft.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 2: Publish .NET Core API
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SpiceCraft.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 3: Serve .NET Core API
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "SpiceCraft.Server.dll"]
