# miniURL

My for-fun implementation of a URL shortening service through and API.

Supports creation of shortened URLs by either randomly generated identifiers or by a specified, desired short URL. And of course retrieving these (as a redirect) through the API. Only has a couple of hard coded users, as user administration has not been a priority yet.

Things I want to do in the future (but no plans right now): User handling with IdentityServer4 and web front-end (Blazor?).

## Technologies and frameworks

* .NET Core 3.1
* ASP.NET Core 3.1
* Entity Framework Core 3.1 with providers
  * Microsoft SQL Server
  * Microsoft In Memory
* MediatR for lean controllers, CQRS and pipeline behaviors
* Microsoft dependency injection for DI and IoC
* Microsoft console logging
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
