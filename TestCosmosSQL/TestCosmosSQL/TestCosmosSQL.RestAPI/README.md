# TestCosmosSQL [![Build Status](https://dev.azure.com/ConduentDevOps/CAP/_apis/build/status/TestCosmosSQL?branchName=develop)](https://dev.azure.com/ConduentDevOps/CAP/_build/latest?definitionId=?&branchName=develop)   [![Release Status](https://vsrm.dev.azure.com/ConduentDevOps/_apis/public/Release/badge/99948005-3d11-4bf2-ba1e-e3b3ec37fb47/1/1)](https://vsrm.dev.azure.com/ConduentDevOps/_apis/public/Release/badge/99948005-3d11-4bf2-ba1e-e3b3ec37fb47/?/?)  


## Introduction 

CAP .NET Core Web API is based on .NET Core web API 2.2. This project contains generic controller with Web API Conventions. This solution includes the following features :-  
* Azure application insight enabled with logging and trace. 
* Global exception handling.
* Dockerfile.
* Kubernetes deployment files.
* Health check's.
* Swagger documentation.

## Prerequisite
* .NET Core 2.2+ is required to build this project.
* Add NuGet feed under package manager source as https://pkgs.dev.azure.com/ConduentDevOps/_packaging/CAP-Artifacts/nuget/v3/index.json

#### The projects included are :-


* TestCosmosSQL.RestApi :- This project has the Rest API components like Controllers, WebHost, Settings etc.
* TestCosmosSQL.Application:- This project has the application logics like Services, Dto's etc.
* TestCosmosSQL.Domain :- This project contains the domain specific code (if any)
* TestCosmosSQL.Tests :- This project contains the tests for all projects.


#### Add additional features.

The following Nuget's are available to extend the capabilities of this project. To name few Nuget's are[Continue reading on Refer link below.]
* **CAP.LoggingLibrary.Builder**
* **CAP.ValidationLibrary.Attributes**
* **CAP.ProblemDetails.Exceptions**

#### How to use the project.

This Project is ready to deploy without any change. Add necessary logic to the corresponding project depending on the service you are working on.
At the development stage, you shall check the health of the service using http://localhost:5001/health-ui. Once deployed to Dev Kubernetes
cluster, you shall set/use KubernetesDiscoveryService settings under appsettings.Development.json to view the health. 



* **Exception**:-If there are custom exceptions or any exception added and wanted to respond with a different Http status code, Use the AddCapProblemDetails extension to inject  
exception and corresponding HttpStatusCode.

For example, 

Custom Exception name is InvalidArgumentException and the http status code need to be 400 bad request[instead of default 500], then inject as below 
```
in Startup.cs
private static Action<ProblemDetailsOptions> MapCustomExceptionsToKnownHttpStatusCode()
{
           return option => { option.Map<InvalidArgumentException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status400BadRequest)); };

            
}
```
* **Configuration**:- Application configurations are reloaded automatically when there is a change in the configurations. Depending on the service you are working on, 
 configuration can be injected to  the service or pass as argument from the controller.You can make your services 
 as Scopped dependancy injection under Startup.cs to inject IOptionsSnapshot scopped configuration. This change will make the service instance to be created for each requests though.
``` 
in Startup.cs
 services.Configure<ServiceNameConfiguration>(Configuration.GetSection("ServiceNameConfiguration"));

services.AddScoped(cfg => cfg.GetService<IOptionsSnapshot<ServiceNameConfiguration>>().Value);
services.AddScoped<IService, ServiceName>();

```
If you have multiple configurations to be used then folow the below steps.
1. Add configuration class for each configuration
2. Add the configuration .json files to Secretes/ folder.
3. Update the program.cs to include all the configuration.json using configurationBuilder.AddJsonFile(); for production.
4. Update the Statup.cs to inject all te configuration class to dipendancy injection using services.AddScoped()
5. Update controllers and services to inject/pass all configuration objects.
6. 

Following standards must be met.
* Unit / Integration testing Standards

    * 80% code coverage target for unit testing.
    * Data driven unit test.
    * Xunit as unit testing framework.

* Coding Standards

    * Conduent will utilize ReSharper C# coding guidelines to ensure code quality and consistency.
    * Adhere to naming conventions provide by [Microsoft guidelines](https://github.com/Microsoft/api-guidelines/blob/vNext/Guidelines.md).
    * Dependency Injection.
* DevOps
    * Update the build and release badges from the Devops.

Refer **[README](https://dev.azure.com/ConduentDevOps/CAP/_git/Process%20Template%20Management?path=%2FREADME.md&version=GBmaster&_a=contents)** for more information.

#### Dev-ops Status 

| Detail        |      Develop      |  Master |
|---------------|:-----------------|:--------:|
| Code Coverage |                  |    -     |
| Build         | [![Build Status](https://dev.azure.com/ConduentDevOps/CAP/_apis/build/status/TestCosmosSQL?branchName=develop)](https://dev.azure.com/ConduentDevOps/CAP/_build/latest?definitionId=?&branchName=develop)        |         |
| Release       |  [![Release Status](https://vsrm.dev.azure.com/ConduentDevOps/_apis/public/Release/badge/99948005-3d11-4bf2-ba1e-e3b3ec37fb47/1/1)](https://vsrm.dev.azure.com/ConduentDevOps/_apis/public/Release/badge/99948005-3d11-4bf2-ba1e-e3b3ec37fb47/?/?)      |         |
