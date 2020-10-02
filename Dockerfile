FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
ARG SONAR_PROJECT_KEY=test
ARG SONAR_OGRANIZAION_KEY=test
ARG SONAR_HOST_URL=http://10.0.0.102:9000/sonar
ARG SONAR_TOKEN=a5347b5331137238bd1295a34f309e4d757876d6
WORKDIR /src
RUN dotnet tool install --global dotnet-sonarscanner
RUN dotnet tool install --global coverlet.console
COPY ["TestNinja/TestNinja.csproj", "."]
RUN dotnet restore "TestNinja.csproj" -r linux-musl-x64
RUN dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput="coverage.opencover.xml"
RUN dotnet sonarscanner begin \
  /k:"$SONAR_PROJECT_KEY" \
  /o:"$SONAR_OGRANIZAION_KEY" \
  /d:sonar.host.url="$SONAR_HOST_URL" \
  /d:sonar.login="$SONAR_TOKEN" \
  /d:sonar.cs.opencover.reportsPaths=/coverage.opencover.xml
COPY . .
#RUN dotnet build "TestNinja.csproj" -c Release -r linux-musl-x64 -o /app
FROM build AS publish
RUN dotnet publish "TestNinja.csproj" -c Release -o /app
RUN rm -r /app/cs /app/de /app/es /app/fr /app/it /app/ja /app/ko /app/pl /app/pt-BR /app/ru /app/tr /app/zh-Hans /app/zh-Hant
FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "TestNinja.dll"]
