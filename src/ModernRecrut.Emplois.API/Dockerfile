#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ModernRecrut.Emplois.API/ModernRecrut.Emplois.API.csproj", "src/ModernRecrut.Emplois.API/"]
COPY ["src/ModernRecrut.Emplois.ApplicationCore/ModernRecrut.Emplois.ApplicationCore.csproj", "src/ModernRecrut.Emplois.ApplicationCore/"]
COPY ["src/ModernRecrut.Emplois.Infrastructure/ModernRecrut.Emplois.Infrastructure.csproj", "src/ModernRecrut.Emplois.Infrastructure/"]
RUN dotnet restore "./src/ModernRecrut.Emplois.API/./ModernRecrut.Emplois.API.csproj"
COPY . .
WORKDIR "/src/src/ModernRecrut.Emplois.API"
RUN dotnet build "./ModernRecrut.Emplois.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ModernRecrut.Emplois.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ModernRecrut.Emplois.API.dll"]