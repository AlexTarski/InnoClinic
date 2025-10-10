# Stage 1: Build Angular app
FROM node:20 AS build
WORKDIR /app

# Copy package files and install dependencies
COPY package*.json ./
RUN npm install

# Copy the rest of the source code
COPY . .

# Expose port 4300
EXPOSE 4300

# Start
CMD ["npm", "start"]