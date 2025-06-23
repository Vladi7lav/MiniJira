FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MiniJira.Presentation/MiniJira.Presentation.csproj", "MiniJira.Presentation/"]
RUN dotnet restore "MiniJira.Presentation/MiniJira.Presentation.csproj"
COPY . .
WORKDIR "/src/MiniJira.Presentation"
RUN dotnet build "MiniJira.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MiniJira.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniJira.Presentation.dll"]
