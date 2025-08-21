FROM nginx:alpine

# Instala envsubst
RUN apk add --no-cache gettext

# Copia configs e entrypoint
COPY conf.d /etc/nginx/conf.d
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

ENTRYPOINT ["/entrypoint.sh"]