#!/bin/sh
set -e

# Create config folder
mkdir -p /usr/share/nginx/html/assets/config

# Write runtime config
cat > /usr/share/nginx/html/assets/config/config.json <<EOF
{
  "Profiles_API_Url": "${PROFILES_API_URL:-http://profiles-api:7036}",
  "Offices_API_Url": "${OFFICES_API_URL:-http://offices-api:8269}",
  "Auth_API_Url": "${AUTH_API_URL:-http://auth-api:10036}",
  "Docs_API_Url": "${DOCS_API_URL:-http://documents-api:9096}",
  "Employee_UI_Url": "${EMPLOYEE_UI_URL:-http://documents-api:9096}"
}
EOF

# Start Nginx
exec "$@"
