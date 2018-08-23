FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 63061
EXPOSE 44362

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Orle3/Orle3.csproj Orle3/
RUN dotnet restore  Orle3/Orle3.csproj
COPY . .
WORKDIR /src/Orle3
RUN dotnet build Orle3.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Orle3.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 11111
EXPOSE 30000
EXPOSE 8090
RUN mkdir logs
ENTRYPOINT ["dotnet", "Orle3.dll"]
