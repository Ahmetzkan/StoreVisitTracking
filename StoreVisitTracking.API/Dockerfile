FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["StoreVisitTracking.API/StoreVisitTracking.API.csproj", "StoreVisitTracking.API/"]
COPY ["StoreVisitTracking.Application/StoreVisitTracking.Application.csproj", "StoreVisitTracking.Application/"]
COPY ["StoreVisitTracking.Domain/StoreVisitTracking.Domain.csproj", "StoreVisitTracking.Domain/"]
COPY ["StoreVisitTracking.Infrastructure/StoreVisitTracking.Infrastructure.csproj", "StoreVisitTracking.Infrastructure/"]
RUN dotnet restore "StoreVisitTracking.API/StoreVisitTracking.API.csproj"
COPY . .
WORKDIR "/src/StoreVisitTracking.API"
RUN dotnet build "StoreVisitTracking.API.csproj" -c Release -o /app/build
RUN dotnet tool install --global dotnet-ef

FROM build AS publish
RUN dotnet publish "StoreVisitTracking.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /root/.dotnet/tools /root/.dotnet/tools
COPY --from=build /src /src

# entrypoint.sh dosyasını kopyalayın
COPY ["StoreVisitTracking.API/entrypoint.sh", "/app/entrypoint.sh"]
RUN chmod +x /app/entrypoint.sh

ENV PATH="${PATH}:/root/.dotnet/tools"
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Scripti çalıştırarak uygulamayı başlatın
ENTRYPOINT ["/app/entrypoint.sh"]
