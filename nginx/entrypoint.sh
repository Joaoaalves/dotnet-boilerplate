#!/bin/sh
set -e

# Substitui a variável $DOMAIN nos arquivos de conf e gera conf final
echo "Replacing DOMAIN in Nginx configuration..."
envsubst '$DOMAIN' < /etc/nginx/conf.d/reverse.conf > /etc/nginx/conf.d/reverse-subs.conf
rm /etc/nginx/conf.d/reverse.conf
mv /etc/nginx/conf.d/reverse2.conf /etc/nginx/conf.d/reverse.conf

# Testa configuração
nginx -t

# Inicia o Nginx em foreground
exec nginx -g 'daemon off;'