FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /source
RUN touch test.txt
RUN dotnet build
ENTRYPOINT ["dotnet", "TestNinja.dll"]