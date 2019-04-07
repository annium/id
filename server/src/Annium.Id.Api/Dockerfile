FROM microsoft/dotnet:2.2-sdk-alpine as builder
COPY . /src
RUN dotnet publish -c release -o /app /src/Annium.Id.Api

FROM microsoft/dotnet:2.2-runtime-alpine
WORKDIR /app
COPY --from=builder /app /app
COPY --from=builder /src/ef.* /app/
VOLUME [ "/app/configuration", "/app/data" ]
CMD ["dotnet", "/app/Annium.Id.Api.dll"]