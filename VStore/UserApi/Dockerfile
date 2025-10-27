# Fase base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Fase de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia e restaura dependências
COPY ["UserApi.csproj", "."]
RUN dotnet restore "UserApi.csproj"

# Copia todo o código e builda
COPY . .
WORKDIR "/src"
RUN dotnet build "UserApi.csproj" -c Release -o /app/build

# Fase de publish
FROM build AS publish
RUN dotnet publish "UserApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Fase final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserApi.dll"]