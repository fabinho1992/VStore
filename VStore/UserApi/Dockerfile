FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os .csproj primeiro
COPY ["UserApi/UserApi.csproj", "UserApi/"]
COPY ["VStore.Shared/VStore.Shared.csproj", "VStore.Shared/"]

# Restaura as dependências
RUN dotnet restore "UserApi/UserApi.csproj"

# Copia o resto do código
COPY . .

WORKDIR "/src/UserApi"
RUN dotnet build "UserApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserApi.dll"]