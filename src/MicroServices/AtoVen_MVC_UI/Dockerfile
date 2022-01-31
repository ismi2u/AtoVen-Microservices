#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AtoVen_MVC_UI/AtoVen_MVC_UI.csproj", "AtoVen_MVC_UI/"]
RUN dotnet restore "AtoVen_MVC_UI/AtoVen_MVC_UI.csproj"
COPY . .
WORKDIR "/src/AtoVen_MVC_UI"
RUN dotnet build "AtoVen_MVC_UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AtoVen_MVC_UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AtoVen_MVC_UI.dll"]