FROM nginx:alpine

ARG ENABLE_MONITORING=false
ARG ENABLE_LOGGING=false

COPY templates/ /tmp/templates/

RUN mkdir -p /etc/nginx/templates

RUN if [ "$ENABLE_MONITORING" = "true" ]; then \
    echo "Adding monitoring.conf.template"; \
    cp /tmp/templates/monitoring.conf.template /etc/nginx/templates/; \
    fi && \
    if [ "$ENABLE_LOGGING" = "true" ]; then \
    echo "Adding logging.conf.template"; \
    cp /tmp/templates/logging.conf.template /etc/nginx/templates/; \
    fi
