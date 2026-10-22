# --- Etapa de compilación ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar solo el csproj primero para aprovechar la caché
COPY ["Citas.Api.csproj", "./"]
RUN dotnet restore "Citas.Api.csproj"

# Copiar el resto del código
COPY . .
RUN dotnet publish "Citas.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Etapa de ejecución final ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Usar puertos estándar dentro del contenedor
EXPOSE 80
EXPOSE 443

COPY --from=build /app/publish .

# Usar CMD en lugar de ENTRYPOINT para flexibilidad
CMD ["dotnet", "Citas.Api.dll"]
