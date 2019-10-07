FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine as builder
COPY . /src
RUN dotnet publish -c release -o /app /src/Annium.Id.Api

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine
WORKDIR /app
COPY --from=builder /app /app
COPY --from=builder /src/ef.* /app/
VOLUME [ "/app/configuration", "/app/data" ]
CMD ["dotnet", "/app/Annium.Id.Api.dll"]