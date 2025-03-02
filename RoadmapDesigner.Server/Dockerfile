# Сборка
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем только проект бэкенда
COPY ./RoadmapDesigner.Server/RoadmapDesigner.Server.csproj ./RoadmapDesigner.Server/
WORKDIR /src/RoadmapDesigner.Server
RUN dotnet restore

# Копируем все файлы бэкенда и собираем
COPY ./RoadmapDesigner.Server/ .
RUN dotnet publish -c Release -o /app/publish

# Рантайм
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000
ENTRYPOINT ["dotnet", "RoadmapDesigner.Server.dll"]
