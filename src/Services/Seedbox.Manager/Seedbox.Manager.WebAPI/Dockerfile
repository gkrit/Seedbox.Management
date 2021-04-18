FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
# FROM mcr.microsoft.com/dotnet/aspnet:5.0.4-alpine3.13-amd64 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Seedbox.Manager.WebAPI.csproj", "./"]
RUN dotnet restore "Seedbox.Manager.WebAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Seedbox.Manager.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Seedbox.Manager.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Seedbox.Manager.WebAPI.dll"]