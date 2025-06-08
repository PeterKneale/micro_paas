# Dockerfile
FROM nginx:stable-alpine

# Copy your custom nginx config
COPY default.conf /etc/nginx/conf.d/default.conf

# Expose HTTP
EXPOSE 5001

# Run nginx in foreground
CMD ["nginx", "-g", "daemon off;"]