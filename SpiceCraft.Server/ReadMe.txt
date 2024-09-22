# Scaffold all the tables from the database
  dotnet ef dbcontext scaffold "Name=ConnectionStrings:DefaultConnection" Microsoft.EntityFrameworkCore.SqlServer --context-dir Context --output-dir Models
  or to force it 
  dotnet ef dbcontext scaffold "Name=ConnectionStrings:DefaultConnection" Microsoft.EntityFrameworkCore.SqlServer --context-dir Context --output-dir Models --force

  dotnet ef dbcontext scaffold "Server=localhost,1455;Database=SpiceCraft;User Id=sa;Password=Admin@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --context-dir Context --output-dir Models --force


# Scaffold specific tables from the database
  dotnet ef dbcontext scaffold "Name=ConnectionStrings:DefaultConnection" --table Artist --table Album

# adding migration
  dotnet ef migrations add InitialCreate

# Updating the database
  dotnet ef database update (apply all the pending migrations which are not applied)
  dotnet ef database update <migration name> (apply specific migrations)

# The following updates your database to a given migration:
  dotnet ef database update AddNewTables

# to revert all migrations and bring the database back to its initial state (empty schema)
  dotnet ef database update 0

# To remove latest migration and go back to previous one
   dotnet ef database update PreviousMigrationName



# terraform

  terraform apply -auto-approve
  terraform plan
  terraform destroy -auto-approve
