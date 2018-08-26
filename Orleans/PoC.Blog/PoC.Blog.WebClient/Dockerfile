FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
Copy Solution-Items/db-settings.json /config/
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ../PoC.Blog.WebClient/PoC.Blog.WebClient.csproj ../PoC.Blog.WebClient/
COPY PoC.Blog.GrainContracts/PoC.Blog.GrainContracts.csproj PoC.Blog.GrainContracts/
RUN dotnet restore ../PoC.Blog.WebClient/PoC.Blog.WebClient.csproj
COPY . .
WORKDIR /src/../PoC.Blog.WebClient
RUN dotnet build PoC.Blog.WebClient.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish PoC.Blog.WebClient.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PoC.Blog.WebClient.dll"]
