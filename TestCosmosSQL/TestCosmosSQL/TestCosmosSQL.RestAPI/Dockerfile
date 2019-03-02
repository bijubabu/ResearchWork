FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["TestCosmosSQL.RestApi/TestCosmosSQL.RestApi.csproj", "TestCosmosSQL.RestApi/"]
COPY ["TestCosmosSQL.RestApi/NuGet.Config", "TestCosmosSQL.RestApi/"]

COPY ["TestCosmosSQL.RestApi/TestCosmosSQL.RestApi.Tests.csproj", "TestCosmosSQL.RestApi.Tests/"]

RUN dotnet restore "TestCosmosSQL.RestApi/TestCosmosSQL.RestApi.csproj" --configfile "TestCosmosSQL.RestApi/NuGet.Config"
RUN dotnet restore "TestCosmosSQL.RestApi.Tests/TestCosmosSQL.RestApi.Tests.csproj" --configfile "TestCosmosSQL.RestApi/NuGet.Config"

COPY . .
WORKDIR "/src/TestCosmosSQL.RestApi"
RUN dotnet build "TestCosmosSQL.RestApi.csproj" -c Release -o /app

#--target testrunner with volume mount to get test results.
FROM build AS testrunner
WORKDIR "/src/TestCosmosSQL.RestApi.Tests"
ENTRYPOINT ["dotnet", "test", "--logger:trx"]


FROM build AS test
WORKDIR "/src/TestCosmosSQL.RestApi.Tests"
RUN dotnet test

FROM build AS publish
RUN dotnet publish "TestCosmosSQL.RestApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestCosmosSQL.RestApi.dll"]