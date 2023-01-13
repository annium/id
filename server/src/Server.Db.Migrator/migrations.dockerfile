FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine
COPY . /code
WORKDIR /code
RUN dotnet build /code/src/infrastructure/Annium.Id.Infrastructure.DbMigrator && \
    dotnet tool restore
VOLUME [ "/code/src/infrastructure/Annium.Id.Infrastructure.DbMigrator/configuration" ]
CMD ["/bin/sh"]