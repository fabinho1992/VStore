FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os .csproj primeiro
COPY ["VStore.EmailApi/VStore.EmailApi.csproj", "VStore.EmailApi/"]
COPY ["VStore.Shared/VStore.Shared.csproj", "VStore.Shared/"]

# Restaura as dependências
RUN dotnet restore "VStore.EmailApi/VStore.EmailApi.csproj"

# Copia o resto do código
COPY . .

WORKDIR "/src/VStore.EmailApi"
RUN dotnet build "VStore.EmailApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VStore.EmailApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VStore.EmailApi.dll"]