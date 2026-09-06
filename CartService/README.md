# CartService

ASP.NET Core (.NET 10) Web API for managing a user's shopping cart, backed by SQL Server via EF Core.

## Prerequisites

- .NET 10 SDK
- SQL Server LocalDB (ships with Visual Studio, or install the "SQL Server Express LocalDB" component separately)
- EF Core CLI tools (only needed if you'll run migrations yourself):

  ```
  dotnet tool install --global dotnet-ef
  ```

## Running the app

From the `CartService` project directory:

```
dotnet restore
dotnet run
```

The API listens on:

- `http://localhost:5232`
- `https://localhost:7169`

(see `Properties/launchSettings.json`). Swagger/OpenAPI is available in the `Development` environment at `/openapi/v1.json`.

## Creating the database

The connection string lives in `appsettings.json`:

```
Server=(localdb)\mssqllocaldb;Database=CartServiceDb;Trusted_Connection=True;MultipleActiveResultSets=true
```

Apply the existing migrations to create (and seed) the database:

```
dotnet ef database update
```

This creates the `CartServiceDb` database on your local LocalDB instance, including the seeded users and catalog items defined in `Data/CartDbContext.cs`.

If you change the EF models, generate a new migration before updating the database:

```
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## Getting an access token

All endpoints require a JWT bearer token. In `Development`, a helper endpoint mints one for you using the signing key configured under `Jwt` in `appsettings.json` — no external identity provider needed for local testing:

```
POST /api/dev/token
POST /api/dev/token?userId=<guid>
```

- If `userId` is omitted, a random one is generated.
- To act as one of the seeded users, pass one of their IDs (see `Data/CartDbContext.cs`), e.g. `11111111-1111-1111-1111-111111111101` (flukas).

Response:

```json
{
  "userId": "11111111-1111-1111-1111-111111111101",
  "token": "<jwt>"
}
```

Send the token on subsequent requests:

```
Authorization: Bearer <jwt>
```

The token is valid for 1 hour. This endpoint only exists in `Development` — it is not mapped in other environments.

## Authenticated endpoints

Every endpoint below requires the `Authorization: Bearer <jwt>` header. The cart endpoints act on the cart belonging to the caller — identity is taken from the token's `NameIdentifier`/`sub` claim, never from a request parameter.

### Cart (`/api/cart`)

| Method | Route              | Description                                                        |
|--------|---------------------|---------------------------------------------------------------------|
| GET    | `/api/cart`         | Get the current user's cart.                                        |
| POST   | `/api/cart/items`    | Add an item to the current user's cart. Body: `{ "itemId": "<guid>", "quantity": 1 }` |
| DELETE | `/api/cart/items/{itemId}` | Remove one unit of an item from the current user's cart (removes the line entirely once its quantity reaches 0). |
| DELETE | `/api/cart`          | Delete the current user's cart entirely (including its items).      |

### User (`/api/user`)

| Method | Route        | Description         |
|--------|--------------|----------------------|
| GET    | `/api/user`  | List all users.      |
