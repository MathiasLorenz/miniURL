# miniURL

My implementation of a URL shortening service.

Still very much a work in progress. The idea is to first create the full API and afterwards maybe build a Blazor front-end on top. As I'm no front-end engineer, this will only be done if time allows and I find the motivation.


## Migrations

To add a migration run the command

`dotnet ef migrations add "INSERTMigrationName" --project src\Infrastructure --startup-project src\API --output-dir Persistence\Migrations`,

to update the database run

`dotnet ef database update --project src\Infrastructure --startup-project src\API`

and to remove the last migration use

`dotnet ef migrations remove --project src\Infrastructure --startup-project src\API`
