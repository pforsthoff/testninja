# Stage 1
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /build
#map a volume to workspace/TestNinja
RUN --rm --name dotnet-runner -v /var/jenkins_home/workspace/TestNinja:/build dotnet restore dotnet publish -c Release -o /app

#COPY . .
#RUN dotnet restore
#RUN dotnet publish -c Release -o /app
# Stage 2
#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "TestNinja.dll"]