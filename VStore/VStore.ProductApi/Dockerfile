FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os .csproj primeiro
COPY ["VStore.ProductApi/VStore.ProductApi.csproj", "VStore.ProductApi/"]
COPY ["VStore.Shared/VStore.Shared.csproj", "VStore.Shared/"]

# Restaura as dependências
RUN dotnet restore "VStore.ProductApi/VStore.ProductApi.csproj"

# Copia o resto do código
COPY . .

WORKDIR "/src/VStore.ProductApi"
RUN dotnet build "VStore.ProductApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VStore.ProductApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VStore.ProductApi.dll"]