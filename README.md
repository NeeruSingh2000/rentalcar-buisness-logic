# RentalCar

A small .NET solution for managing rental pickup/return registration and price calculation.

## Solution Structure

- `src/RentalCar` — main domain/library project
- `tests/RentalCar.Tests` — NUnit test project

## Prerequisites

- .NET SDK 10.0+

Check your SDK:

```bash
dotnet --info
```

## Build

From the repository root:

```bash
dotnet build RentalCar.sln
```

## Run Tests

```bash
dotnet test RentalCar.sln
```
## Notes

- In-memory storage is used via `ConcurrentDictionary` in `RentalService`.
- Data is not persisted across application restarts unless persistence is added.
