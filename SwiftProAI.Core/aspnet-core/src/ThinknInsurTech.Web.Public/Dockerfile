#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ThinknInsurTech.Web.Public/ThinknInsurTech.Web.Public.csproj", "src/ThinknInsurTech.Web.Public/"]
COPY ["src/ThinknInsurTech.Web.Core/ThinknInsurTech.Web.Core.csproj", "src/ThinknInsurTech.Web.Core/"]
COPY ["src/ThinknInsurTech.Application/ThinknInsurTech.Application.csproj", "src/ThinknInsurTech.Application/"]
COPY ["src/ThinknInsurTech.Application.Shared/ThinknInsurTech.Application.Shared.csproj", "src/ThinknInsurTech.Application.Shared/"]
COPY ["src/ThinknInsurTech.Core.Shared/ThinknInsurTech.Core.Shared.csproj", "src/ThinknInsurTech.Core.Shared/"]
COPY ["src/ThinknInsurTech.Core/ThinknInsurTech.Core.csproj", "src/ThinknInsurTech.Core/"]
COPY ["src/ThinknInsurTech.EntityFrameworkCore/ThinknInsurTech.EntityFrameworkCore.csproj", "src/ThinknInsurTech.EntityFrameworkCore/"]
COPY ["src/ThinknInsurTech.GraphQL/ThinknInsurTech.GraphQL.csproj", "src/ThinknInsurTech.GraphQL/"]
RUN dotnet restore "src/ThinknInsurTech.Web.Public/ThinknInsurTech.Web.Public.csproj"
COPY . .
WORKDIR "/src/src/ThinknInsurTech.Web.Public"
RUN dotnet build "ThinknInsurTech.Web.Public.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ThinknInsurTech.Web.Public.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ThinknInsurTech.Web.Public.dll"]
