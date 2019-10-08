FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine as builder
COPY . /code
RUN dotnet publish -c release -o /app /code/src/Annium.Id.Api && \
    dotnet tool restore --tool-manifest /code/.config/dotnet-tools.json && \
    find ~/.nuget/packages/dotnet-ef -name dotnet-ef.dll | xargs -I% cp % /code/ef.dll

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine
WORKDIR /app
COPY --from=builder /app /app
COPY --from=builder /code/ef.* /app/
VOLUME [ "/app/certs", "/app/configuration", "/app/keys" ]
CMD ["/app/Annium.Id.Api"]