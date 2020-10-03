FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
ARG SONAR_PROJECT_KEY=testninja
ARG SONAR_OGRANIZAION_KEY=''
ARG SONAR_HOST_URL=http://10.0.0.102/
ARG SONAR_TOKEN=0d5fbec78fabe1219adb7f916f008d70073373d6
WORKDIR /src
RUN apt-get update
# To solve add-apt-repository : command not found
RUN apt-get -y install software-properties-common

# Install Java
RUN \
  echo oracle-java8-installer shared/accepted-oracle-license-v1-1 select true | debconf-set-selections && \
  add-apt-repository -y ppa:webupd8team/java && \
  apt-get update && \
  apt-get install -y oracle-java8-installer --allow-unauthenticated && \
  rm -rf /var/lib/apt/lists/* && \
  rm -rf /var/cache/oracle-jdk8-installer


# Define commonly used JAVA_HOME variable
ENV JAVA_HOME /usr/lib/jvm/java-8-oracle
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global dotnet-sonarscanner
RUN dotnet tool install --global coverlet.console
ENV JAVA_HOME=/usr/local/openjdk-8/bin
ENV PATH="$PATH:$JAVA_HOME/bin"  
COPY ["TestNinja/TestNinja.csproj", "."]
RUN dotnet sonarscanner begin \
  /k:"$SONAR_PROJECT_KEY" \
  /o:"$SONAR_OGRANIZAION_KEY" \
  /d:sonar.host.url="$SONAR_HOST_URL" \
  /d:sonar.login="$SONAR_TOKEN" \
  /d:sonar.verbose=true \
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
RUN rm -r /app/cs /app/de /app/es /app/fr /app/it /app/ja /app/ko /app/pl /app/pt-BR /app/ru /app/tr /app/zh-Hans /app/zh-Hant
FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "TestNinja.dll"]
