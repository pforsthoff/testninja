FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["TestNinja/TestNinja.csproj", "."]
RUN dotnet restore "TestNinja.csproj" -r linux-musl-x64
RUN dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput="coverage.opencover.xml"
COPY . .
#RUN dotnet build "TestNinja.csproj" -c Release -r linux-musl-x64 -o /app
FROM build AS publish
RUN dotnet publish "TestNinja.csproj" -c Release -r linux-musl-x64 -o /app
RUN rm -r /app/cs /app/de /app/es /app/fr /app/it /app/ja /app/ko /app/pl /app/pt-BR /app/ru /app/tr /app/zh-Hans /app/zh-Hant
FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "TestNinja.dll"]
