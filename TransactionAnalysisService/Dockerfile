#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TransactionAnalysisService/TransactionAnalysisService.csproj", "TransactionAnalysisService/"]
RUN dotnet restore "TransactionAnalysisService/TransactionAnalysisService.csproj"
COPY . .
WORKDIR "/src/TransactionAnalysisService"
RUN dotnet build "TransactionAnalysisService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TransactionAnalysisService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransactionAnalysisService.dll"]
