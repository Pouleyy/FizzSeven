# FizzSeven

FizzSeven is a small ASP.NET Core API for the configurable FizzBuzz exercise.

You send `int1`, `int2`, `limit`, `str1`, and `str2`, and the API returns the generated sequence.  
It also keeps track of the most frequent request and exposes it through a statistics endpoint.

The API defaults to a maximum `limit` of `100`. This value is arbitrary and can be changed in `src/FizzSeven.Api/appsettings.json` with `FizzBuzz:MaxLimit`.

## Why Aspire

Because it keeps local dev simple: **one run starts the API and Redis together** and gives you a dashboard to see what is running.

## Run locally

You need:

- .NET 10 SDK
- Docker running

Then start the AppHost:

```bash
dotnet run --project src/FizzSeven.AppHost
```

If you prefer an IDE, just run the `FizzSeven.AppHost` project.

Once it starts, Aspire prints a dashboard URL in the terminal. Open it, then open the API resource from there.  
In development, the API docs are available with Scalar at `/scalar/v1`.

## Configuration

- `FizzBuzz:MaxLimit`: maximum accepted `limit` value, default `100`
- `Statistics:UseInMemoryStore`: when `false` the API stores statistics in Redis, when `true` it keeps them in memory for the process lifetime

With the default settings, the AppHost starts Redis automatically. If you switch to in-memory statistics, Redis is not required.

## What the API does

- `GET /api/v1/fizzbuzz` returns the generated sequence
- `GET /api/v1/fizzbuzz/statistics` returns the most used request and its hit count

### `GET /api/v1/fizzbuzz`

Query parameters:

- `int1`: integer greater than `0`
- `int2`: integer greater than `0`
- `limit`: integer greater than `0` and less than or equal to `100` by default
- `str1`: non-empty string
- `str2`: non-empty string

Example request:

```text
/api/v1/fizzbuzz?int1=3&int2=5&limit=15&str1=fizz&str2=buzz
```

Successful response (`200 OK`):

```json
["1", "2", "fizz", "4", "buzz", "fizz", "7", "8", "fizz", "buzz", "11", "fizz", "13", "14", "fizzbuzz"]
```

Validation error response (`400 Bad Request`):

```json
{
  "errors": {
    "Limit": ["limit must be less than or equal to 100."],
    "Str1": ["str1 must not be empty."]
  }
}
```

### `GET /api/v1/fizzbuzz/statistics`

Successful response (`200 OK`):

```json
{
  "int1": 3,
  "int2": 5,
  "limit": 15,
  "str1": "fizz",
  "str2": "buzz",
  "hits": 7
}
```

If no successful FizzBuzz request has been recorded yet, the endpoint returns `404 Not Found`.

## Tests

```bash
dotnet test FizzSeven.slnx
```
