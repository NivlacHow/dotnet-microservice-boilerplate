#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Services/SampleFrontendSvc/SampleFrontendSvc.csproj", "Services/SampleFrontendSvc/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "BuildingBlocks/Common.Logging/"]
COPY ["BuildingBlocks/Sandline.RMQLibs/Sandline.RMQLibs.csproj", "BuildingBlocks/Sandline.RMQLibs/"]
RUN dotnet restore "Services/SampleFrontendSvc/SampleFrontendSvc.csproj"
COPY . .
WORKDIR "/src/Services/SampleFrontendSvc"
RUN dotnet build "SampleFrontendSvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleFrontendSvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleFrontendSvc.dll"]