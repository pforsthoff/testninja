# Stage 1
FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /TestNinja/build
#map a volume to workspace/TestNinja

COPY . .

ENTRYPOINT ["dotnet", "TestNinja.dll"]