This project implements a `.NET Core Web API` (that also supports Swagger UI) using `SQLite`, which allows users to handle address data (meaning: street, number, zipcode, etc.), and also search and calculate the distance between two addresses. The distance is calculated using great-circle-distance between the `lat,lon` coordinates of the two addresses (obtained by geocoding via NeutrinoAPI).

The following are required to be installed (and can be downloaded via NuGet in VisualStudio): 
- `NeutrinoAPI 3.6.0`
- `Microsoft.EntityFrameworkCore.Sqlite 7.0.3` 
- `Swashbuckle.AspNetCore 6.2.3` 

The inlcuded SQLite database, contains 3 example addresses, inside [db_1.db](db_1.db), that can be used for testing purposes. The database path is set [here](appsettings.json#L3).

Examples:
- https://localhost:7235/api/addresses/query?query=Delft&sortby=houseNumber&asc=true
- https://localhost:7235/api/addresses/calcDist?id=1&id2=3


