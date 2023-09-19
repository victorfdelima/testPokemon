FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 44343

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PokeAPI/PokeAPI.csproj", "PokeAPI/"]
RUN dotnet restore "PokeAPI/PokeAPI.csproj"
COPY . .
WORKDIR "/src/PokeAPI"
RUN dotnet build "PokeAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PokeAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PokeAPI.dll"]