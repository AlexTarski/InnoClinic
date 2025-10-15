# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7036

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["InnoClinic.Profiles.API/InnoClinic.Profiles.API.csproj", "InnoClinic.Profiles.API/"]
COPY ["InnoClinic.Profiles.App/InnoClinic.Profiles.Business.csproj", "InnoClinic.Profiles.App/"]
COPY ["InnoClinic.Profiles.Domain/InnoClinic.Profiles.Domain.csproj", "InnoClinic.Profiles.Domain/"]
COPY ["InnoClinic.Profiles.Infrastructure/InnoClinic.Profiles.Infrastructure.csproj", "InnoClinic.Profiles.Infrastructure/"]
COPY ["InnoClinic.Shared/InnoClinic.Shared.csproj", "InnoClinic.Shared/"]


RUN dotnet restore "./InnoClinic.Profiles.API/InnoClinic.Profiles.API.csproj"
COPY . .

WORKDIR "/src/InnoClinic.Profiles.API"
RUN dotnet build "./InnoClinic.Profiles.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InnoClinic.Profiles.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InnoClinic.Profiles.API.dll"]