# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["RailwayManager.csproj", "./"]
RUN dotnet restore "RailwayManager.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "RailwayManager.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "RailwayManager.csproj" -c Release -o /app/publish

# Final stage - serve with nginx
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Remove default nginx static assets
RUN rm -rf ./*

# Copy published files from build stage
COPY --from=publish /app/publish/wwwroot .

# Copy nginx configuration
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]