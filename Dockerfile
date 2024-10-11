FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY TropicalExpress.Api/*.csproj ./TropicalExpress.Api/
COPY TropicalExpress.Infrastructure/*.csproj ./TropicalExpress.Infrastructure/
COPY TropicalExpress.Domain/*.csproj ./TropicalExpress.Domain/
COPY TropicalExpress.Tests/*.csproj ./TropicalExpress.Tests/
RUN dotnet restore "./TropicalExpress.Api/TropicalExpress.Api.csproj"

COPY . ./
RUN dotnet publish "./TropicalExpress.Api/TropicalExpress.Api.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "TropicalExpress.Api.dll"]