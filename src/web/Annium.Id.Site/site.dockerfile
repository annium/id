FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
COPY . /code
RUN dotnet publish -c Release -o /dist /code/src/web/Annium.Id.Site

FROM nginx:alpine
COPY --from=build /dist/wwwroot/ /usr/share/nginx/html/
COPY ./src/web/Annium.Id.Site/nginx.conf /etc/nginx/nginx.conf