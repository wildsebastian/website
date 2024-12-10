FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM node:20 AS css_build
ENV NODE_ENV=production
WORKDIR /app
COPY . .
WORKDIR /app/Website/Frontend
RUN npm install
RUN npx tailwindcss -i ./input.css -o ../../app.css
RUN cp -r node_modules/mathjax ../wwwroot/js
RUN cp -r node_modules/@highlightjs/ ../wwwroot/js/highlightjs

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Website/Website.csproj", "Website/"]
RUN dotnet restore "Website/Website.csproj"
COPY . .
WORKDIR "/src/Website"
RUN rm -rf Frontend/
COPY --from=css_build /app/app.css ./wwwroot/css/
RUN dotnet build "Website.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet tool install --global dotnet-ef
RUN PATH="$PATH:/root/.dotnet/tools" dotnet ef migrations bundle

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Website.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
COPY --from=build /src/Website/efbundle /app/publish
COPY infrastructure/entrypoint.sh /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./entrypoint.sh"]
