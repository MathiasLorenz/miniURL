# miniURL

My implementation of a URL shortening service.


## Migrations

To add a migration run the command

`dotnet ef migrations add "INSERTMigrationName" --project src\Infrastructure --startup-project src\API --output-dir Persistence\Migrations`,

to update the database run

`dotnet ef database update --project src\Infrastructure --startup-project src\API`

and to remove the last migration use

`dotnet ef migrations remove --project src\Infrastructure --startup-project src\API`
