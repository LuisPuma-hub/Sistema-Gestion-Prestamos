FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/SistemaPrestamos.API/SistemaPrestamos.API.csproj", "src/SistemaPrestamos.API/"]
COPY ["src/SistemaPrestamos.Application/SistemaPrestamos.Application.csproj", "src/SistemaPrestamos.Application/"]
COPY ["src/SistemaPrestamos.Domain/SistemaPrestamos.Domain.csproj", "src/SistemaPrestamos.Domain/"]
COPY ["src/SistemaPrestamos.Infrastructure/SistemaPrestamos.Infrastructure.csproj", "src/SistemaPrestamos.Infrastructure/"]
RUN dotnet restore "src/SistemaPrestamos.API/SistemaPrestamos.API.csproj"
COPY . .
WORKDIR "/src/src/SistemaPrestamos.API"
RUN dotnet publish "SistemaPrestamos.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SistemaPrestamos.API.dll"]
