FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /source
RUN ls
RUN dotnet build
RUN ls
ENTRYPOINT ["dotnet", "TestNinja.dll"]