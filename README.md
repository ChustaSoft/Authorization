# Authorization

## Packages available

| Package                                      | Pipeline                                                                                                                                                                                                                                                                                         |  NuGet version                                                                                                                                                                                     |    Downloads                                                                                  | .Net Core 3.1      | .NET 5.0           | TypeScript         |
|----------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------|--------------------|--------------------|--------------------|
| ChustaSoft.Tools.Authorization.Abstractions  | [![Build Status](https://dev.azure.com/chustasoft/SocialNET/_apis/build/status/OpenStack/Authorization/%5BRELEASE%5D%20-%20ChustaSoft%20Authorization%20Abstractions%20(NuGet)?branchName=master)](https://dev.azure.com/chustasoft/SocialNET/_build/latest?definitionId=22&branchName=master)   | [![NuGet](https://img.shields.io/nuget/v/ChustaSoft.Tools.Authorization.Abstractions?label=NuGet%20AspNet%20package)](https://www.nuget.org/packages/ChustaSoft.Tools.Authorization.Abstractions)  | ![Nuget](https://img.shields.io/nuget/dt/ChustaSoft.Tools.Authorization.Abstractions)         | :heavy_check_mark: | :heavy_check_mark: | :x:                |
| ChustaSoft.Tools.Authorization               | [![Build Status](https://dev.azure.com/chustasoft/SocialNET/_apis/build/status/OpenStack/Authorization/%5BRELEASE%5D%20-%20ChustaSoft%20Authorization%20(NuGet)?branchName=master)](https://dev.azure.com/chustasoft/SocialNET/_build/latest?definitionId=6&branchName=master)                   | [![NuGet](https://img.shields.io/nuget/v/ChustaSoft.Tools.Authorization?label=NuGet%20Main%20package)](https://www.nuget.org/packages/ChustaSoft.Tools.Authorization)                              | ![Nuget](https://img.shields.io/nuget/dt/ChustaSoft.Tools.Authorization)                      | :heavy_check_mark: | :heavy_check_mark: | :x:                |
| ChustaSoft.Tools.Authorization.SqlServer     | [![Build Status](https://dev.azure.com/chustasoft/SocialNET/_apis/build/status/OpenStack/Authorization/%5BRELEASE%5D%20-%20ChustaSoft%20Authorization%20AspNet%20(NuGet)?branchName=master)](https://dev.azure.com/chustasoft/SocialNET/_build/latest?definitionId=7&branchName=master)          | [![NuGet](https://img.shields.io/nuget/v/ChustaSoft.Tools.Authorization.AspNet?label=NuGet%20AspNet%20package)](https://www.nuget.org/packages/ChustaSoft.Tools.Authorization.AspNet)              | ![Nuget](https://img.shields.io/nuget/dt/ChustaSoft.Tools.Authorization.SqlServer)            | :heavy_check_mark: | :heavy_check_mark: | :x:                |
| ChustaSoft.Tools.Authorization.AspNet        | [![Build Status](https://dev.azure.com/chustasoft/SocialNET/_apis/build/status/OpenStack/Authorization/%5BRELEASE%5D%20-%20ChustaSoft%20Authorization%20SqlServer%20(NuGet)?branchName=master)](https://dev.azure.com/chustasoft/SocialNET/_build/latest?definitionId=23&branchName=master)      | [![NuGet](https://img.shields.io/nuget/v/ChustaSoft.Tools.Authorization.SqlServer?label=NuGet%20AspNet%20package)](https://www.nuget.org/packages/ChustaSoft.Tools.Authorization.SqlServer)        | ![Nuget](https://img.shields.io/nuget/dt/ChustaSoft.Tools.Authorization.AspNet)               | :heavy_check_mark: | :heavy_check_mark: | :x:                |
| @chustasoft/cs-authorization-connector       | [![Build Status](https://dev.azure.com/chustasoft/SocialNET/_apis/build/status/OpenStack/Authorization/%5BRELEASE%5D%20-%20ChustaSoft%20authorization-connector%20(npm)?branchName=master)](https://dev.azure.com/chustasoft/SocialNET/_build/latest?definitionId=29&branchName=master)          | ![npm](https://img.shields.io/npm/v/@chustasoft/cs-authorization-connector?label=npm%20connector)                                                                                                  | ![npm](https://img.shields.io/npm/dt/@chustasoft/cs-authorization-connector)                  | :x:                | :x:                | :heavy_check_mark: |


### Compatibility table

| Framework     | From   | Latest  | Current support    |
|---------------|--------|---------|--------------------|
| .Net Core 2.2 | 1.0.0  | 1.0.0   | :x:                |
| .Net Core 3.1 | 1.0.1  | Current | :heavy_check_mark: |
| .Net 5.0      | 3.3.1  | Current | :heavy_check_mark: |


## Description
---
Authorization project solution based on Microsoft Identity and JWT for .NET Core projects
Additionally, a Javascript connector developed with TypeScript is provided ready to be installed in the project


Current implementation allow to configure the project inside an API, providing methods for managing user access; retriving a JWT token for authenticate the rest of secured Controllers.

## Documentation

For getting started, visit the home page of our project [wiki](https://github.com/ChustaSoft/Authorization/wiki)

To know how to use this version, take a look on 

- NET 5.0_
  - [Basic API Example without custom properties](https://github.com/ChustaSoft/Authorization/tree/master/Examples/.NET%205.0/ChustaSoft.Tools.Authorization.TestBasic.WebAPI)
  
- .Net Core 3.1:
  - [API usage and frontend connector example](https://github.com/ChustaSoft/Authorization/tree/master/Examples/.NetCore%203.1/ChustaSoft.Tools.Authorization.TestConsumer.WebUI)
  - [Basic API Example without custom properties](https://github.com/ChustaSoft/Authorization/tree/master/Examples/.NetCore%203.1/ChustaSoft.Tools.Authorization.TestBasic.WebAPI)
  - [Custom example extending context](https://github.com/ChustaSoft/Authorization/tree/master/Examples/.NetCore%203.1/ChustaSoft.Tools.Authorization.TestCustom.WebAPI)



_*NOTE*: Code is cross comptaible between framework versions, so examples are implicitly valid for all versions taking into account the multiple configuration options available_

*Thanks for using and contributing*
---
[![Twitter Follow](https://img.shields.io/twitter/follow/ChustaSoft?label=Follow%20us&style=social)](https://twitter.com/ChustaSoft)
