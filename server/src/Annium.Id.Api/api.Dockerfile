FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine as builder
COPY . /code
WORKDIR /app
RUN dotnet publish -c release -o /app /code/src/Annium.Id.Api

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine
WORKDIR /app
COPY --from=builder /app /app
VOLUME [ "/app/certs", "/app/configuration", "/app/keys" ]
CMD ["/app/Annium.Id.Api"]