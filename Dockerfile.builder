FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /source
dotnet build
RUN ls
ENTRYPOINT ["dotnet", "TestNinja.dll"]