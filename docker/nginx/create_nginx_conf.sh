#!/bin/sh

set -e

NGINX_CONF="/etc/nginx/conf.d/default.conf"

if [ -s /etc/nginx/certs/localhost.crt ] &&
[ -s /etc/nginx/certs/localhost.key ]; then
    
    echo "TLS certs are found. Using nginx with HTTP + HTTPS ports."
    
    cp /etc/nginx/nginx.conf.base "$NGINX_CONF"
    
else
    
    echo "TLS certs are not found. Using nginx only with HTTP port."
    
    cat > "$NGINX_CONF" <<'EOF'
server {
    listen 80;

    include /etc/nginx/locations.conf;
}
EOF
    
fi

exec nginx -g "daemon off;"
