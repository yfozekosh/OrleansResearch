FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY MITeam.OrleansResearch.SiloHost/MITeam.OrleansResearch.SiloHost.csproj MITeam.OrleansResearch.SiloHost/
COPY MITeam.OrleansResearch.GrainContracts/MITeam.OrleansResearch.GrainContracts.csproj MITeam.OrleansResearch.GrainContracts/
COPY MITeam.OrleansResearch.GrainImplementation/MITeam.OrleansResearch.GrainImplementation.csproj MITeam.OrleansResearch.GrainImplementation/
RUN dotnet restore MITeam.OrleansResearch.SiloHost/MITeam.OrleansResearch.SiloHost.csproj
COPY . .
WORKDIR /src/MITeam.OrleansResearch.SiloHost
RUN dotnet build MITeam.OrleansResearch.SiloHost.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish MITeam.OrleansResearch.SiloHost.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MITeam.OrleansResearch.SiloHost.dll"]
