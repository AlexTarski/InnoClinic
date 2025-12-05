# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 12287

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["InnoClinic.Appointments.API/InnoClinic.Appointments.API.csproj", "InnoClinic.Appointments.API/"]
COPY ["InnoClinic.Appointments.Business/InnoClinic.Appointments.Business.csproj", "InnoClinic.Appointments.Business/"]
COPY ["InnoClinic.Appointments.Domain/InnoClinic.Appointments.Domain.csproj", "InnoClinic.Appointments.Domain/"]
COPY ["InnoClinic.Appointments.Infrastructure/InnoClinic.Appointments.Infrastructure.csproj", "InnoClinic.Appointments.Infrastructure/"]
COPY ["InnoClinic.Shared/InnoClinic.Shared.csproj", "InnoClinic.Shared/"]


RUN dotnet restore "./InnoClinic.Appointments.API/InnoClinic.Appointments.API.csproj"
COPY . .

WORKDIR "/src/InnoClinic.Appointments.API"
RUN dotnet build "./InnoClinic.Appointments.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InnoClinic.Appointments.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InnoClinic.Appointments.API.dll"]