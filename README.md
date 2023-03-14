This project implements a `.NET Core Web API` (including a Swagger documentation) using `SQLite`, which allows users to manage address data (meaning: street, number, zipcode, etc.). 

The following are required to be installed (and can be downloaded via NuGet in VisualStudio): 
- `NeutrinoAPI 3.6.0`
- `Microsoft.EntityFrameworkCore.Sqlite 7.0.3` 
- `Swashbuckle.AspNetCore 6.2.3` 

The inlcuded SQLite database, contains 3 example addresses, inside [db_1.db](db_1.db), that can be used for testing purposes. The database path is set [here](appsettings.json#L3).
