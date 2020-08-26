# miniURL

My implementation of a URL shortening service.

Still very much a work in progress. The idea is to first create the full API and afterwards maybe build a Blazor front-end on top. As I'm no front-end engineer, this will only be done if time allows and I find the motivation.

## Technologies and frameworks

* .NET Core 3.1
* ASP.NET Core 3.1
* Entity Framework Core 3.1 with Microsoft SQL Server
* MediatR for lean controllers and CQRS
* Microsoft DependencyInjection for DI and IoC
* Shouldly for testing
* Moq for mocking in testing
* FluentValidation for request validation

## Structure and organization

The structure is heavily inspired from Jason Taylor's excellent work with 'clean code' style orginization within .NET projects. The main inspirations are from the repositories [Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture), [Northwind Traders](https://github.com/jasontaylordev/NorthwindTraders) and various talks from Jason Taylor. I don't need use the clean architecture template, because I didn't want the Angular front-end, so I have build everything up myself drawing heavy inspiration from the linked repositories.

## Migrations

To add a migration run the command

`dotnet ef migrations add "INSERTMigrationName" --project src\Infrastructure --startup-project src\API --output-dir Persistence\Migrations`,

to update the database run

`dotnet ef database update --project src\Infrastructure --startup-project src\API`

and to remove the last migration use

`dotnet ef migrations remove --project src\Infrastructure --startup-project src\API`
