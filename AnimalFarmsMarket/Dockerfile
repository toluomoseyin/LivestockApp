FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base
WORKDIR /src
COPY *.sln .
COPY AnimalFarmsMarket.Test/*.csproj AnimalFarmsMarket.Test/
COPY AnimalFarmsMarket.Core/*.csproj AnimalFarmsMarket.Core/
COPY AnimalFarmsMarket.Commons/*.csproj AnimalFarmsMarket.Commons/
COPY AnimalFarmsMarket.Data/*.csproj AnimalFarmsMarket.Data/
COPY DevelopmentMigrations/*.csproj DevelopmentMigrations/
COPY ProductionMigrations/*.csproj ProductionMigrations/
RUN dotnet restore

COPY . .

#Testing
FROM base AS testing
WORKDIR /src/AnimalFarmsMarket.Core
WORKDIR /src/AnimalFarmsMarket.Commons
WORKDIR /src/AnimalFarmsMarket.Data
WORKDIR /src/DevelopmentMigrations
WORKDIR /src/ProductionMigrations
RUN dotnet build
WORKDIR /src/AnimalFarmsMarket.Test
RUN dotnet test

#Publishing
FROM base AS publish
WORKDIR /src/AnimalFarmsMarket.Core
RUN dotnet publish -c Release -o /src/publish

#Get the runtime into a folder called app
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .

COPY --from=publish /src/AnimalFarmsMarket.Data/Data.Json/Agent.json .
COPY --from=publish /src/AnimalFarmsMarket.Data/Data.Json/AnimalMarket.json .
COPY --from=publish /src/AnimalFarmsMarket.Data/Data.Json/Broadcast.json .
COPY --from=publish /src/AnimalFarmsMarket.Data/Data.Json/DeliveryMode.json .
COPY --from=publish /src/AnimalFarmsMarket.Data/Data.Json/DeliveryPerson.json .
COPY --from=publish /src/AnimalFarmsMarket.Data/Data.Json/Livestock.json .
COPY --from=publish /src/AnimalFarmsMarket.Data/Data.Json/Partner.json .
COPY --from=publish /src/AnimalFarmsMarket.Data/Data.Json/PaymentMethod.json .
COPY --from=publish /src/AnimalFarmsMarket.Data/Data.Json/Roles.json .
COPY --from=publish /src/AnimalFarmsMarket.Data/Data.Json/ShippingPlan.json .
COPY --from=publish /src/AnimalFarmsMarket.Data/Data.Json/Category.json .
COPY --from=publish /src/AnimalFarmsMarket.Data/Data.Json/User.json .

#ENTRYPOINT ["dotnet", "AnimalFarmsMarket.Core.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet AnimalFarmsMarket.Core.dll
