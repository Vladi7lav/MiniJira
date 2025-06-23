FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MiniJira.DomainServices/MiniJira.DomainServices.csproj", "MiniJira.DomainServices/"]
RUN dotnet restore "MiniJira.DomainServices/MiniJira.DomainServices.csproj"
COPY . .
WORKDIR "/src/MiniJira.DomainServices"
RUN dotnet build "MiniJira.DomainServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MiniJira.DomainServices.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniJira.DomainServices.dll"]
