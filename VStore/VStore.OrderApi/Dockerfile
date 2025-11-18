FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os .csproj primeiro
COPY ["VStore.OrderApi/VStore.OrderApi.csproj", "VStore.OrderApi/"]
COPY ["VStore.Shared/VStore.Shared.csproj", "VStore.Shared/"]

# Restaura as dependências
RUN dotnet restore "VStore.OrderApi/VStore.OrderApi.csproj"

# Copia o resto do código
COPY . .

WORKDIR "/src/VStore.OrderApi"
RUN dotnet build "VStore.OrderApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VStore.OrderApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VStore.OrderApi.dll"]