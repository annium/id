FROM node:alpine as installer
COPY package.json yarn.lock .npmrc /src/
WORKDIR /src/
RUN yarn install --frozen-lockfile

FROM node:alpine as builder
COPY . /src/
COPY --from=installer /src/node_modules/ /src/node_modules/
WORKDIR /src/
RUN yarn build


FROM nginx:alpine
RUN rm /etc/nginx/conf.d/default.conf
COPY --from=builder /src/build /src/docker-entrypoint.sh /site/
COPY ./site.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
WORKDIR /site
ENTRYPOINT [ "./docker-entrypoint.sh" ]