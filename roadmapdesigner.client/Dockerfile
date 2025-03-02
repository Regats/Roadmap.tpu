# Сборка
FROM node:20 AS build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build

# Сервер nginx для статики
FROM nginx:1.25
COPY --from=build /app/dist /usr/share/nginx/html
