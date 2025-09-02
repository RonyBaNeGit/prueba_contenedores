# Usar la imagen base oficial de .NET 8 para producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Copiar todos los archivos del directorio actual al contenedor
COPY . .

# Comando para ejecutar tu API
ENTRYPOINT ["dotnet", "ApiPrueba.dll"]
