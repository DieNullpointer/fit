docker rm -f fitmanager_sqlserver2019 &> /dev/nul
docker run -d -p 1433:1433 --name fitmanager_sqlserver2019 -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SqlServer2019" mcr.microsoft.com/azure-sql-edge

dotnet build fit.backend/FitManager.Webapi --no-incremental --force
dotnet watch run -c Debug --project fit.backend/FitManager.Webapi
