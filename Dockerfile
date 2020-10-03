FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
ARG SONAR_PROJECT_KEY=testninja
ARG SONAR_OGRANIZAION_KEY=''
ARG SONAR_HOST_URL=http://10.0.0.102/
ARG SONAR_TOKEN=0d5fbec78fabe1219adb7f916f008d70073373d6
WORKDIR /src
ENV JAVA_VER 8
ENV JAVA_HOME /usr/lib/jvm/java-8-oracle

RUN echo 'deb http://ppa.launchpad.net/webupd8team/java/ubuntu trusty main' >> /etc/apt/sources.list && \
    echo 'deb-src http://ppa.launchpad.net/webupd8team/java/ubuntu trusty main' >> /etc/apt/sources.list && \
    apt-key adv --keyserver keyserver.ubuntu.com --recv-keys C2518248EEA14886 && \
    apt-get update && \
    echo oracle-java${JAVA_VER}-installer shared/accepted-oracle-license-v1-1 select true | sudo /usr/bin/debconf-set-selections && \
    apt-get install -y --force-yes --no-install-recommends oracle-java${JAVA_VER}-installer oracle-java${JAVA_VER}-set-default && \
    apt-get clean && \
    rm -rf /var/cache/oracle-jdk${JAVA_VER}-installer
RUN update-java-alternatives -s java-8-oracle
RUN echo "export JAVA_HOME=/usr/lib/jvm/java-8-oracle" >> ~/.bashrc
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
