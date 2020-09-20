FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR testninja
RUN dotnet --version