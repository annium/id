FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine
COPY . /code
WORKDIR /code
RUN dotnet build /code/src/Annium.Id.Api && \
    dotnet tool install -g dotnet-ef --version 3.1.2 && \
    dotnet tool restore
VOLUME [ "/code/src/Annium.Id.Api/configuration" ]
CMD ["/bin/sh"]