﻿# Base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["testudv.API/testudv.API.csproj", "testudv.API/"]
RUN dotnet restore "testudv.API/testudv.API.csproj"
COPY . .
WORKDIR "/src/testudv.API"
RUN dotnet build "testudv.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "testudv.API.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "testudv.API.dll"]