#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Stock/StockService/StockMonitorService.csproj", "Stock/StockService/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Messaging/Messaging.csproj", "Messaging/"]
RUN dotnet restore "Stock/StockService/StockMonitorService.csproj"
COPY . .
WORKDIR "/src/Stock/StockService"
RUN dotnet build "StockMonitorService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StockMonitorService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockMonitorService.dll"]