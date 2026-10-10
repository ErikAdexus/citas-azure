# --- Etapa de Compilación ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Citas.Api.csproj", "./"]
RUN dotnet restore "Citas.Api.csproj"

COPY . .
RUN dotnet publish "Citas.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Etapa de Ejecución Final ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Citas.Api.dll"]