# Build container. Load full SDK as base image.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# TODO: Adapt the directories!
COPY fit.backend/FitManager.Application FitManager.Application
COPY fit.backend/FitManager.Webapi      FitManager.Webapi

# Compile the app
RUN dotnet restore "FitManager.Webapi"
RUN dotnet build   "FitManager.Webapi" -c Release -o /app/build
RUN dotnet publish "FitManager.Webapi" -c Release -o /app/publish /p:UseAppHost=false

# App container. Only needs runtime (smaller image)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
EXPOSE 80
EXPOSE 443
WORKDIR /app    

COPY --from=build /app/publish .
COPY fit.backend/FitManager.Webapi/admin /app/admin
COPY fit.backend/FitManager.Webapi/wwwroot /app/wwwroot
ENTRYPOINT ["dotnet", "FitManager.Webapi.dll"]