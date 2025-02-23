FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["src/FeeSystem.Web.Host/FeeSystem.Web.Host.csproj", "src/FeeSystem.Web.Host/"]
COPY ["src/FeeSystem.Web.Core/FeeSystem.Web.Core.csproj", "src/FeeSystem.Web.Core/"]
COPY ["src/FeeSystem.Application/FeeSystem.Application.csproj", "src/FeeSystem.Application/"]
COPY ["src/FeeSystem.Core/FeeSystem.Core.csproj", "src/FeeSystem.Core/"]
COPY ["src/FeeSystem.EntityFrameworkCore/FeeSystem.EntityFrameworkCore.csproj", "src/FeeSystem.EntityFrameworkCore/"]
WORKDIR "/src/src/FeeSystem.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/FeeSystem.Web.Host", "src/FeeSystem.Web.Host"]
COPY ["src/FeeSystem.Web.Core", "src/FeeSystem.Web.Core"]
COPY ["src/FeeSystem.Application", "src/FeeSystem.Application"]
COPY ["src/FeeSystem.Core", "src/FeeSystem.Core"]
COPY ["src/FeeSystem.EntityFrameworkCore", "src/FeeSystem.EntityFrameworkCore"]
WORKDIR "/src/src/FeeSystem.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "FeeSystem.Web.Host.dll"]
