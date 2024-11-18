# README

## DB Connection
1. Create an new `.env` file inside the `api` folder
2. From `.env.development` copy the contents
3. Paste the `.env.development`contents into the `.env` file
4. You may need to change the `Username` or `Password` based on your local configuration 


## Creating a new feature

### 1 Create an model implementing the `abstract class` BaseModel

### 2 Add the new model to the `AppDbContext.cs`

### 3 Create and execute the migrations

1. Create an migration via the `CLI`\
```sh
dotnet ef migrations add short_description_of_your_migration
``` 
2. Execute the migration via the `CLI`
```sh
dotnet ef database update
```

### 4 Create an request and response DTO implementing the `abstract class` DTO

### 5 Create controller

**NOTE: if any of the following method names are the actions you want to make. Use the following**
- Create
- Update
- UpdatePartial
- Delete
- ShowSingle
- ShowAll

### 6 Create the validator for the model
- Documentation for validation: https://docs.fluentvalidation.net/en/latest/