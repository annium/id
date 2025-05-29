FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine as builder
COPY . /code
WORKDIR /code
RUN dotnet restore && \
    find . -type f -name nuget.config | xargs rm && \
    dotnet publish --no-restore -c release -o /app /code/web/src/Site

FROM nginx:alpine
COPY --from=builder /app/wwwroot/ /usr/share/nginx/html/
COPY ./web/src/Site/nginx.conf /etc/nginx/nginx.conf