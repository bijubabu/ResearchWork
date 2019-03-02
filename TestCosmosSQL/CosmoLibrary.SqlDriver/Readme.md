# SDK for CosmosDb

Azure CosmosDB is a fully managed, scalable, queryable, schema free JSON document database service built for modern applications. This SDK is provided as nuget package to use in your .net core application.

## Getting Started

This library lets you to interact with azure cosmosdb with all CURD operations required to interact with .net core application. Install this nuget package to your application and follow the steps to configure the cosmosDb interaction. The nuget package is available in Conduent DevOps. 

In order to download the nuget, configure the Nuget Package Source with below address:
https://pkgs.dev.azure.com/ConduentDevOps/_packaging/CAP-Artifacts/nuget/v3/index.json

### Prerequisites

This nuget package can be consumed by any .net core v2.2 application.

### Usage

Once the nuget is installed, we need to follow the below steps to start using the Api.

#### 1. Add a new configuration section in appsettings.json

This configuration is specific to CosmosDb settings.

```sh
{
  "CosmosConfiguration": {
    "ConnectionString": "cosmosconnectionstring",
    "CosmoOfferThroughput": "500",
    "DatabaseName": "dbname",
    "CollectionName": "collectionname"
  }
}
```
> Note: 
> 
> ***CosmosConfiguration*** is the section name and it could be anything of your choice.
> 
> ***ConnectionString*** is the connection string to connect to CosmosDb.
> 
> ***CosmoOfferThroughput*** is the throughput value. It is decided by number of request units per second. Please find more details on [cosmosdb documentation](https://docs.microsoft.com/en-us/azure/cosmos-db/request-units).
> 
> ***DatabaseName*** is the name of database to be used.
> 
> ***CollectionName*** is the name of collection to be used.

#### 2. Configure Service

In the startup class of .net core WebApi service (plug-in service), under ConfigureServices method:

* Bind the appsettings configuration with configuration class (DocumentDbConfig)

```
var configuration = new DocumentDbConfig();
Configuration.GetSection("CosmosConfiguration").Bind(configuration);
configuration.Validate();
```
* Add the configuration object so that it can be injected to APIs

```
services.Configure<DocumentDbConfig>(
	Configuration.GetSection("CosmosConfiguration"));
```
* Register the interface in dependency injection container (IServiceCollection) provided by .net core.

```
services.AddSingleton<IDocumentDbRepository<EntityObject>, 
	GenericRepository<EntityObject>>();
```
> Note: ***EntityObject*** is the document object derived from ***DocumentEntity*** on which the CosmosDb is going to work. Please replace with valid entity object.

#### 3. Inject the interface

* In the controller constructor, add the interface which is intended to be used in the controller.

```
private readonly IDocumentDbRepository<EntityObject> _documentRepository;
private readonly IOptions<DocumentDbConfig> _dbConfiguration;

public PlacesController(IDocumentDbRepository<EntityObject> documentDbRepository,
    IOptions<DocumentDbConfig> configuration)
{
    _documentRepository = documentDbRepository;
    _dbConfiguration = configuration;
}
```


## APIs
This SDK has the APIs which does all CRUD operations on CosmosDb. Following are the supported APIs.

**SetupAsync** - Create the database and collection, if it doesn't exists.
> Note: The Database name and Collection Name must be set before calling this method.

**UpsertAsync** - Upserts a Document as an asychronous operation in the Azure Cosmos DB service.

```
EntityObject entModel = new EntityObject()
{
    Id = Guid.NewGuid(),
    Type = "Place",
    Name = "Bob"
};

await _documentRepository.UpsertAsync(entModel);
```

**GetAsync** - Reads Documents from the Azure Cosmos DB service as an asynchronous operation. 

This Api has few overloads mentioned as shown in the example.

* Get all documents without any condition.
```
var entityCollection = await _documentRepository.GetAsync();

return entityCollection;
```

* Filter Get result based on certain criteria -use overload method which uses expression as a parameter. It also includes include parameter to give the result in page.
```
string expressionString = $"(entModel.Name.Contains(\"{searchString}\"))";
var paramExp = Expression.Parameter(typeof(EntityModel), "EntityModel");
var expression = (Expression<Func<EntityModel, bool>>)DynamicExpressionParser.ParseLambda(
		new[] { paramExp }, null, expressionString);

// Calling the first time for first page
var pageResults = _documentRepository.GetAsync(expression, null, 10).Result;
return entityCollection;

// Calling for next page
pageResults = _documentRepository.GetAsync(experssion, pageResults.NextPageToken, 10).Result;

```

* Get a specific document from collection.

```
var entityObject = await _documentRepository.GetAsync(id);

return entityObject;
```


**DeleteAsync** - Delete a Document from the Azure Cosmos DB service as an asynchronous operation.

```
await _documentRepository.DeleteAsync(id);
```

**ExistAsync** - Check if the document exists in Cosmodb collection.
```
var isExist = await _documentRepository.Exist(id);
```

## Declaration
This library is intended to use within Conduent Service.
