FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CarWorkshopAppASP.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

RUN mkdir -p /app/certs && \
    chown -R 1000:1000 /app/certs

COPY ./certs/certificate.crt /app/certs/
COPY ./certs/certificate.key /app/certs/

RUN chmod 600 /app/certs/certificate.key && \
    chmod 644 /app/certs/certificate.crt

ENV CERTIFICATE_PATH=/app/certs/certificate.crt
ENV CERTIFICATE_KEY_PATH=/app/certs/certificate.key
ENV ASPNETCORE_ENVIRONMENT=Docker

EXPOSE 5001

ENTRYPOINT ["dotnet", "CarWorkshopAppASP.dll"]