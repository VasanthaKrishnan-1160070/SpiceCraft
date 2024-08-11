# Scaffold all the tables from the database
  dotnet ef dbcontext scaffold "Name=ConnectionStrings:DefaultConnection" Microsoft.EntityFrameworkCore.SqlServer --context-dir Context --output-dir Models

# Scaffold specific tables from the database
  dotnet ef dbcontext scaffold "Name=ConnectionStrings:DefaultConnection" --table Artist --table Album

# adding migration
  dotnet ef migrations add InitialCreate

# Updating the database
  dotnet ef database update

# The following updates your database to a given migration:
  dotnet ef database update AddNewTables


# terraform

  terraform apply -auto-approve
  terraform plan
  terraform destroy -auto-approve
