FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
ARG SONAR_PROJECT_KEY=testninja
ARG SONAR_ORGANIZAION_KEY=''
ARG SONAR_HOST_URL=http://192.168.1.8/
ARG SONAR_TOKEN=0a2fd59fc7fd179b3a6b320ab64b4f6754d1e635
WORKDIR /src

RUN apt-get update && apt-get install -y openjdk-11-jdk
RUN dotnet tool install --global dotnet-sonarscanner
RUN dotnet tool install --global coverlet.console
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY ["TestNinja/TestNinja.csproj", "."]
RUN dotnet sonarscanner begin \
  /k:"$SONAR_PROJECT_KEY" \
  /o:"$SONAR_OGRANIZAION_KEY" \
  /d:sonar.host.url="$SONAR_HOST_URL" \
  /d:sonar.login="$SONAR_TOKEN" \
  #/d:sonar.verbose=true \
  /d:sonar.cs.opencover.reportsPaths=Tests/TestNinja.UnitTests/Tests/TestNinja.UnitTests/coverage.opencover.xml \
  /d:sonar.coverage.exclusions="**Tests*.cs,**/wwwroot/**" 
RUN dotnet restore "TestNinja.csproj" -r linux-musl-x64
RUN dotnet build
RUN dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput="coverage.opencover.xml"
RUN dotnet sonarscanner end /d:sonar.login="$SONAR_TOKEN"
COPY . .
#RUN dotnet build "TestNinja.csproj" -c Release -r linux-musl-x64 -o /app
FROM build AS publish
RUN dotnet publish "TestNinja.csproj" -c Release -o /app
#RUN rm -r /app/cs /app/de /app/es /app/fr /app/it /app/ja /app/ko /app/pl /app/pt-BR /app/ru /app/tr /app/zh-Hans /app/zh-Hant
FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "TestNinja.dll"]
