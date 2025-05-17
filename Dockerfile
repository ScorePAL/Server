# Étape 1 : image de base pour l'exécution
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Étape 2 : build de l'application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copier les fichiers projet individuellement pour une meilleure gestion du cache Docker
COPY ["Controller/Controller.csproj", "Controller/"]
COPY ["Model/Model.csproj", "Model/"]
COPY ["Service/Service.csproj", "Service/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY Server.sln .

# Restore de la solution
RUN dotnet restore "Server.sln"

# Copier tout le reste
COPY . .

# Build du projet principal (Controller.csproj)
WORKDIR "/src/Controller"
RUN dotnet build "Controller.csproj" -c Release -o /app/build

# Étape 3 : publication de l'application
FROM build AS publish
RUN dotnet publish "Controller.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Étape 4 : image finale pour l'exécution
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Controller.dll"]
