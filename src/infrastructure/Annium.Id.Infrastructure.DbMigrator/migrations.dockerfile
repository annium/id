FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine
COPY . /code
WORKDIR /code
RUN dotnet build /code/src/infrastructure/Annium.Id.Infrastructure.DbMigrator && \
    dotnet tool restore
VOLUME [ "/code/src/infrastructure/Annium.Id.Infrastructure.DbMigrator/configuration" ]
CMD ["/bin/sh"]