#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ThinknInsurTech.Migrator/ThinknInsurTech.Migrator.csproj", "src/ThinknInsurTech.Migrator/"]
COPY ["src/ThinknInsurTech.EntityFrameworkCore/ThinknInsurTech.EntityFrameworkCore.csproj", "src/ThinknInsurTech.EntityFrameworkCore/"]
COPY ["src/ThinknInsurTech.Core/ThinknInsurTech.Core.csproj", "src/ThinknInsurTech.Core/"]
COPY ["src/ThinknInsurTech.Core.Shared/ThinknInsurTech.Core.Shared.csproj", "src/ThinknInsurTech.Core.Shared/"]
RUN dotnet restore "src/ThinknInsurTech.Migrator/ThinknInsurTech.Migrator.csproj"
COPY . .
WORKDIR "/src/src/ThinknInsurTech.Migrator"
RUN dotnet build "ThinknInsurTech.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ThinknInsurTech.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ThinknInsurTech.Migrator.dll", "-s"]
