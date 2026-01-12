
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ./src/FunctionApp/FunctionApp.csproj ./FunctionApp/
RUN dotnet restore ./FunctionApp/FunctionApp.csproj
COPY ./src/FunctionApp ./FunctionApp
RUN dotnet publish ./FunctionApp/FunctionApp.csproj -c Release -o /out

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0
WORKDIR /home/site/wwwroot
COPY --from=build /out .
ENV APP_PORT=8444 ASPNETCORE_URLS=http://+:8444 FUNCTIONS_WORKER_RUNTIME=dotnet-isolated
EXPOSE 8444
