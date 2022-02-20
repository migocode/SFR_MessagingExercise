#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CoreBankingSystem/CoreBankingSystem.csproj", "CoreBankingSystem/"]
RUN dotnet restore "CoreBankingSystem/CoreBankingSystem.csproj"
COPY . .
WORKDIR "/src/CoreBankingSystem"
RUN dotnet build "CoreBankingSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoreBankingSystem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreBankingSystem.dll"]
