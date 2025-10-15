# Stage 1: Build Angular app
FROM node:20 AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build -- --configuration production

# Stage 2: Serve with Nginx on port 4200
FROM nginx:alpine
# Copy Angular build output
COPY --from=build /app/dist/InnoClinic.ClientUI/browser /usr/share/nginx/html
# Nginx config to listen on 4200
COPY nginx.conf /etc/nginx/nginx.conf
# Runtime config injection
COPY docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh

EXPOSE 4200
ENTRYPOINT ["/docker-entrypoint.sh"]
CMD ["nginx", "-g", "daemon off;"]
