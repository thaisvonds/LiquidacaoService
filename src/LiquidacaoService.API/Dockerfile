# Estágio de Compilação (Usando SDK do .NET 10)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY ["src/LiquidacaoService.API/LiquidacaoService.API.csproj", "LiquidacaoService.API/"]
RUN dotnet restore "LiquidacaoService.API/LiquidacaoService.API.csproj"

COPY src/LiquidacaoService.API/ LiquidacaoService.API/
WORKDIR "/app/LiquidacaoService.API"
RUN dotnet publish "LiquidacaoService.API.csproj" -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /out .

RUN apt-get update && apt-get install -y \
    libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

RUN mkdir -p /app/logs && chown -R $APP_UID:$APP_UID /app/logs

USER $APP_UID  
 EXPOSE 8080
ENTRYPOINT ["dotnet", "LiquidacaoService.API.dll"]