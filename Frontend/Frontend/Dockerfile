FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Frontend/Frontend.csproj", "Frontend/"]
RUN dotnet restore "Frontend/Frontend.csproj"
COPY . .
WORKDIR "/src/Frontend"
RUN dotnet publish "Frontend.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]
