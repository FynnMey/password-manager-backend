# Password Manager Backend

> [!WARNING]
> **Project Status: Under Active Development**  
> This project is currently in development. Several APIs and features are still work in progress and yet to be added.

## Database Migrations

The following Entity Framework Core commands are used to manage the database schema and migrations:

### Add a new migration
Generate a new migration when database models have been updated:
```bash
dotnet ef migrations add <migration name>
```

### Apply migrations to database
Update the database schema with any pending migrations:
```bash
dotnet ef database update
```
