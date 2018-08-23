FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 63061
EXPOSE 44362

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY MITeam.OrleansResearch.WebClient/MITeam.OrleansResearch.WebClient.csproj MITeam.OrleansResearch.WebClient/
COPY MITeam.OrleansResearch.GrainContracts/MITeam.OrleansResearch.GrainContracts.csproj MITeam.OrleansResearch.GrainContracts/
RUN dotnet restore MITeam.OrleansResearch.WebClient/MITeam.OrleansResearch.WebClient.csproj
COPY . .
WORKDIR /src/MITeam.OrleansResearch.WebClient
RUN dotnet build MITeam.OrleansResearch.WebClient.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish MITeam.OrleansResearch.WebClient.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MITeam.OrleansResearch.WebClient.dll"]
