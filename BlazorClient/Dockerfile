FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
EXPOSE 82

COPY BlazorClient/BlazorClient.csproj .
RUN dotnet restore BlazorClient.csproj
COPY . .
RUN dotnet build BlazorClient.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish BlazorClient.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY BlazorClient/nginx.conf /etc/nginx/nginx.conf

