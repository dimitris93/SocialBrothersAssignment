This project implements a `.NET Core Web API` using `SQLite`, which allows users to handle address data (meaning: street, number, zipcode, etc.). It also allows users to search and then sort the addresses that are added in the database, and can also calculate the distance between two given addresses. The distance is computed using the great-circle-distance formula between the {latitude,longitude} coordinates of the two addresses, which are obtained via geocoding using `NeutrinoAPI`. The project supports Swagger UI. 

The following are required to be installed (and can be downloaded via NuGet in VisualStudio): 
- `NeutrinoAPI 3.6.0`
- `Microsoft.EntityFrameworkCore.Sqlite 7.0.3` 
- `Swashbuckle.AspNetCore 6.2.3` 

The inlcuded SQLite database ([db_1.db](db_1.db)), contains 3 example addresses that can be used for testing purposes. The database path is set [here](appsettings.json#L3). The `NeutrinoAPI` credentials need to be added [here](Controllers/AddressController.cs#L24).

Examples:
- https://localhost:7235/api/addresses/query?q=Delft&sortby=houseNumber&asc=true
- https://localhost:7235/api/addresses/calcDist?id=1&id2=3


