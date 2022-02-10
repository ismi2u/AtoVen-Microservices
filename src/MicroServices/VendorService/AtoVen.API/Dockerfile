#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MicroServices/VendorService/AtoVen.API/AtoVen.API.csproj", "MicroServices/VendorService/AtoVen.API/"]
COPY ["MicroServices/DatabaseService/DataService/DataService.csproj", "MicroServices/DatabaseService/DataService/"]
COPY ["MicroServices/EmailSender/EmailSendService.csproj", "MicroServices/EmailSender/"]
COPY ["MicroServices/ValidationService/ValidationLibrary/ValidationLibrary.csproj", "MicroServices/ValidationService/ValidationLibrary/"]
RUN dotnet restore "MicroServices/VendorService/AtoVen.API/AtoVen.API.csproj"
COPY . .
WORKDIR "/src/MicroServices/VendorService/AtoVen.API"
RUN dotnet build "AtoVen.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AtoVen.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AtoVen.API.dll"]
