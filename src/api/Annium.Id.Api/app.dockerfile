FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as builder
COPY . /code
RUN dotnet publish -c release -o /app /code/src/api/Annium.Id.Api

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app
COPY --from=builder /app /app
VOLUME [ "/app/certs", "/app/configuration", "/app/keys" ]
CMD ["/app/Annium.Id.Api"]