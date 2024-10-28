FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["ScheduleService.API/ScheduleService.API.csproj", "ScheduleService.API/"]
RUN dotnet restore "ScheduleService.API/ScheduleService.API.csproj"
COPY . .
WORKDIR "/src/ScheduleService.API"
RUN dotnet build "ScheduleService.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "ScheduleService.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScheduleService.API.dll"]
