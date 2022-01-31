#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MicroServices/AtoVen.UI/WebApplication1/AtoVen.MVC.csproj", "MicroServices/AtoVen.UI/WebApplication1/"]
RUN dotnet restore "MicroServices/AtoVen.UI/WebApplication1/AtoVen.MVC.csproj"
COPY . .
WORKDIR "/src/MicroServices/AtoVen.UI/WebApplication1"
RUN dotnet build "AtoVen.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AtoVen.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AtoVen.MVC.dll"]
