# Migrations

**Migrations are automatically run before the API starts up**  
(See `Program.cs`, and the corrosponding `SeedDatabaseAsync` extension method for the implementation)

**The site will only startup once the migrations are complete.**

1. [Adding new migrations using the package manager console](Addingnewmigrationsusingthepackagemanagerconsole)
    * 1.1. [SIOProjectionDbContext](#SIOProjectionDbContext)
2. [Adding new migrations using the command line](#Addingnewmigrationsusingthecommandline)
    * 2.1. [SIOProjectionDbContext](#SIOProjectionDbContext-1)

##  1. <a name='Addingnewmigrationsusingthepackagemanagerconsole'></a>Adding new migrations using the package manager console

####  1.1. <a name='SIOProjectionDbContext'></a>SIOProjectionDbContext

```
Add-Migration <MigrationName> -c SIOProjectionDbContext -p SIO.Migrations -o Migrations/SIO/Projection
```

##  2. <a name='Addingnewmigrationsusingthecommandline'></a>Adding new migrations using the command line

####  2.1. <a name='SIOProjectionDbContext-1'></a>SIOProjectionDbContext

```
dotnet ef migrations add <MigrationName> -c SIOProjectionDbContext -p SIO.Migrations -o Migrations/SIO/Projection
```