#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Services/SampleBackendSvc/SampleBackendSvc.csproj", "Services/SampleBackendSvc/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
COPY ["BuildingBlocks/Sandline.RMQLibs/Sandline.RMQLibs.csproj", "BuildingBlocks/Sandline.RMQLibs/"]
RUN dotnet restore "Services/SampleBackendSvc/SampleBackendSvc.csproj"
COPY . .
WORKDIR "/src/Services/SampleBackendSvc"
RUN dotnet build "SampleBackendSvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleBackendSvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleBackendSvc.dll"]