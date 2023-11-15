FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine as builder
COPY . /code
WORKDIR /code
RUN dotnet restore && \
    find . -type f -name nuget.config | xargs rm && \
    dotnet publish --no-restore -c release -o /app /code/server/src/Server.Host

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
COPY --from=builder /app /app
VOLUME [ "/app/certs", "/app/configuration", "/app/keys" ]
CMD ["/app/Server.Host"]