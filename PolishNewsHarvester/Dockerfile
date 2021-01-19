#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["PolishNewsHarvester/PolishNewsHarvester.csproj", "PolishNewsHarvester/"]
RUN dotnet restore "PolishNewsHarvester/PolishNewsHarvester.csproj"
COPY . .
WORKDIR "/src/PolishNewsHarvester"
RUN dotnet build "PolishNewsHarvester.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PolishNewsHarvester.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PolishNewsHarvester.dll"]