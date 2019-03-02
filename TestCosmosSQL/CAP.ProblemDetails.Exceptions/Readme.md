# CAP Problemdetail Library [![Build status](https://dev.azure.com/ConduentDevOps/CAP/_apis/build/status/Libraries%20-%20CAP%20Problem%20Details)](https://dev.azure.com/ConduentDevOps/CAP/_build/latest?definitionId=26)  

This library extends the application builder to manage the exceptions.

## Getting Started

This nuget comes as part of application template so that the developers need not to worry to manage the exceptions. We can also add the nuget manually to any application by adding the nuget **CAP.ProblemDetails.Exceptions**. 

The nuget package is available in Conduent DevOps. 

In order to download the nuget, configure the Nuget Package Source with below address:
https://pkgs.dev.azure.com/ConduentDevOps/_packaging/CAP-Artifacts/nuget/v3/index.json

### Prerequisites

This nuget package can be consumed by any .net core v2.2 application.

### Usage

Once the nuget is installed, we need to follow the below steps to start using the Api.

#### 1. Configure Problem Details

In the startup class of .net core WebApi service, under ConfigureServices method:


* Use Problem detail handler by adding it to request pipeline.

```
services.AddCapProblemDetails(MapCustomExceptionsToKnownHttpStatusCode(), _environment.IsDevelopment());

app.UseCAPProblemDetails();
            
```            
* and if you want NullReferenceException to be treated as 207 add the following, you shall add multiple options.Map<>
* 
```
private static Action<ProblemDetailsOptions> MapCustomExceptionsToKnownHttpStatusCode()
        {
           return option => { option.Map<NullReferenceException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status207MultiStatus)); };

            
        }

```

## Declaration
This library is intended to use within Conduent Service.

#### Dev-ops Status 

| Detail        |      Develop      |  Master |
|---------------|:-----------------|:--------:|
| Code Coverage |                  |    -     |
| Build         | [![Build status](https://dev.azure.com/ConduentDevOps/CAP/_apis/build/status/Libraries%20-%20CAP%20Problem%20Details)](https://dev.azure.com/ConduentDevOps/CAP/_build/latest?definitionId=26)       |         |
| Nuget       | [![CAP.ProblemDetails.Exceptions package in CAP-Artifacts feed in Azure Artifacts](https://feeds.dev.azure.com/ConduentDevOps/_apis/public/Packaging/Feeds/60fc0779-ebb1-42f9-81b2-2a28afbbd1e7/Packages/d9a1ab62-f5ce-4dce-8c9b-43e6ca664f70/Badge)](https://dev.azure.com/ConduentDevOps/CAP/_packaging?_a=package&feed=60fc0779-ebb1-42f9-81b2-2a28afbbd1e7&package=d9a1ab62-f5ce-4dce-8c9b-43e6ca664f70&preferRelease=true)    |         |

