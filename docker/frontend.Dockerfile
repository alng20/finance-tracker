FROM node:24 AS build

WORKDIR /app

COPY frontend/package*.json ./
RUN npm ci

COPY frontend/ ./
RUN npm run build

FROM nginx:alpine AS runtime

COPY --from=build /app/dist /usr/share/nginx/html
COPY docker/nginx/locations.conf /etc/nginx/locations.conf
COPY docker/nginx/nginx.conf.base /etc/nginx/nginx.conf.base
COPY docker/nginx/create_nginx_conf.sh /create_nginx_conf.sh

RUN chmod +x /create_nginx_conf.sh

ENTRYPOINT ["/create_nginx_conf.sh"]
