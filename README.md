# Authorization

  - [![Build Status](https://dev.azure.com/chustasoft/BaseProfiler/_apis/build/status/Release/RELEASE%20-%20NuGet%20-%20ChustaSoft%20Authorization%20AspNet?branchName=master)](https://dev.azure.com/chustasoft/BaseProfiler/_build/latest?definitionId=7&branchName=master)
  - [![NuGet](https://img.shields.io/nuget/v/ChustaSoft.Authorization)](https://www.nuget.org/packages/ChustaSoft.Authorization)

Description
---
Authorization NuGet solution based on Microsoft Identity and JWT


Current implementation allow to configure the project inside an API, providing methods for register and login users; retriving a JWT token for authenticate the rest of secured Controllers.

For getting started, visit the home page of our project wiki:
- https://github.com/ChustaSoft/Authorization/wiki

To know how to use this version, take a look on 
- ChustaSoft.Tools.Authorization.TestBasic.WebAPI project to see how is using it in a simple scenario
- ChustaSoft.Tools.Authorization.TestCustom.WebAPI project to see how is using it in a custom scenario with defined User and/or Role and an specific context.



NOTE: It is not neccesary to add any controller for authenticate, the target project could use the embedded controllers from the NuGet


*Thanks for using and contributing*
---
[![Twitter Follow](https://img.shields.io/twitter/follow/ChustaSoft?label=Follow%20us&style=social)](https://twitter.com/ChustaSoft)