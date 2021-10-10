# Stage 1 -- Build CSS
FROM node:14-alpine as builder

ENV NODE_ENV production
WORKDIR /app
COPY . .
RUN npm install
RUN npx tailwindcss -o tailwind.css

# Stage 2
FROM nginx:1.21-alpine

RUN mkdir -p /usr/share/nginx/html/css

COPY index.html /usr/share/nginx/html/
COPY webfonts /usr/share/nginx/html/webfonts
COPY all.min.css /usr/share/nginx/html/css/
COPY --from=builder /app/tailwind.css /usr/share/nginx/html/css/
