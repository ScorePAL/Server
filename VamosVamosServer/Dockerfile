# Consultez https://aka.ms/customizecontainer pour savoir comment personnaliser votre conteneur de débogage et comment Visual Studio utilise ce Dockerfile pour générer vos images afin d’accélérer le débogage.

# Cet index est utilisé lors de l’exécution à partir de VS en mode rapide (par défaut pour la configuration de débogage)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_URLS="https://*:8081;http://*:8080"


# Cette phase est utilisée pour générer le projet de service
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
RUN dotnet dev-certs https --trust
WORKDIR /src
COPY ["VamosVamosServer/VamosVamosServer.csproj", "VamosVamosServer/"]
RUN dotnet restore "./VamosVamosServer/VamosVamosServer.csproj"
COPY . .
WORKDIR "/src/VamosVamosServer"
RUN dotnet build "./VamosVamosServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Cette étape permet de publier le projet de service à copier dans la phase finale
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VamosVamosServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Cette phase est utilisée en production ou lors de l’exécution à partir de VS en mode normal (par défaut quand la configuration de débogage n’est pas utilisée)
FROM base AS final
WORKDIR /app
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/https/thecertif.pfx"
COPY --from=publish --chmod=444 /root/.dotnet/corefx/cryptography/x509stores/my/* /https/thecertif.pfx
COPY --from=publish /app/publish .
COPY --chmod=666 ["VamosVamosServer/database.db", "."]
ENTRYPOINT ["dotnet", "VamosVamosServer.dll"]