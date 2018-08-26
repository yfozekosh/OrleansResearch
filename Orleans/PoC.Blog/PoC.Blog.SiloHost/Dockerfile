FROM microsoft/dotnet:2.1-runtime AS base
Copy Solution-Items/db-settings.json /config/
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
Copy . .
RUN dotnet restore PoC.Blog.SiloHost/PoC.Blog.SiloHost.csproj
COPY . .
WORKDIR /src/PoC.Blog.SiloHost
RUN dotnet build PoC.Blog.SiloHost.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish PoC.Blog.SiloHost.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PoC.Blog.SiloHost.dll"]
