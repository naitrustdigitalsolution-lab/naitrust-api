FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Directory.Build.props .
COPY *.sln .
COPY src/Naitrust.Domain/Naitrust.Domain.csproj src/Naitrust.Domain/
COPY src/Naitrust.Infrastructure/Naitrust.Infrastructure.csproj src/Naitrust.Infrastructure/
COPY src/Naitrust.Application/Naitrust.Application.csproj src/Naitrust.Application/
COPY src/Naitrust.Api/Naitrust.Api.csproj src/Naitrust.Api/
RUN dotnet restore src/Naitrust.Api/Naitrust.Api.csproj

COPY . .
RUN dotnet publish src/Naitrust.Api/Naitrust.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Naitrust.Api.dll"]
