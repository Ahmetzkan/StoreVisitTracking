#!/bin/bash
set -e

cd /src/StoreVisitTracking.API
export PATH="$PATH:/root/.dotnet/tools"

# Wait for database to be ready
until dotnet ef database update --project ../StoreVisitTracking.Infrastructure/StoreVisitTracking.Infrastructure.csproj --startup-project .; do
    >&2 echo "Database is starting up - migration pending..."
    sleep 5
done

cd /app
# Start the application
exec dotnet StoreVisitTracking.API.dll
