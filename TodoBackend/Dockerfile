FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TodoBackend/TodoBackend.csproj", "TodoBackend/"]
RUN dotnet restore "TodoBackend/TodoBackend.csproj"
COPY . .
WORKDIR "/src/TodoBackend"
RUN dotnet build "TodoBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoBackend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoBackend.dll"]
