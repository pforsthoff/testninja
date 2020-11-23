FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

COPY /TestNinja/bin/Release/netcoreapp3.1/. /app
#RUN dotnet restore "TestNinja.csproj" -r linux-musl-x64
#COPY . .
WORKDIR /app
#RUN dotnet build "TestNinja.csproj" -c Release -r linux-musl-x64 -o /app
#RUN dotnet publish "TestNinja.csproj" -c Release -r linux-musl-x64 -o /app
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENTRYPOINT ["dotnet", "TestNinja.dll"]