FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o .csproj e restaura dependências
COPY Teste-DockerFile.csproj .
RUN dotnet restore "VStore.OrderApi.csproj"

# Copia o resto do código
COPY . .

# Build da aplicação
RUN dotnet build "VStore.OrderApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VStore.OrderApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VStore.OrderApi.dll"]