FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY BlazorizedWebClient.csproj .
    RUN dotnet restore BlazorizedWebClient.csproj
COPY . .
    RUN dotnet build BlazorizedWebClient.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish BlazorizedWebClient.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
    COPY nginx.conf /etc/nginx/nginx.conf