FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /source
dotnet build

ENTRYPOINT ["dotnet", "TestNinja.dll"]