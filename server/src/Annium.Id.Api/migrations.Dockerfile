FROM mcr.microsoft.com/dotnet/core/sdk:3.0-alpine
COPY . /app
WORKDIR /app
RUN dotnet build -c release /app/src/Annium.Id.Api && \
    dotnet tool restore

VOLUME [ "/app/configuration" ]
CMD ["/bin/sh"]