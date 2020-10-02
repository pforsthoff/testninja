FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR testninja
RUN dotnet --version
RUN dotnet restore
RUN dotnet build 
RUN dotnet test TestNinja.sln /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput="coverage.opencover.xml"