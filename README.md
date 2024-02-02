# PgHintPlan

[![.NET](https://github.com/MyInStep/PgHintPlan/actions/workflows/publish.yml/badge.svg?branch=master)](https://github.com/MyInStep/PgHintPlan/actions/workflows/publish.yml)
![NuGet Version](https://img.shields.io/nuget/v/PgHintPlan?style=flat&logo=nuget&label=NuGet)

## PgHintPlan
Run

```sh
dotnet add package PgHintPlan
```

Enable the extension

```csharp
// create the data source
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);
await using var dataSource = dataSourceBuilder.Build();

// enable extension using the data source
await dataSource.EnablePgHintPlanAsync();

// alternatively, you can use the connection
var conn = dataSource.OpenConnection();
await conn.EnablePgHintPlanAsync();

```

## PgHintPlan.EntityFrameworkCore

Run

```sh
dotnet add package PgHintPlan.EntityFrameworkCore
```


Import the library

```csharp
using PgHintPlan.EntityFrameworkCore;
```

Enable the extension

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.UsePgHintPlan();
}
```

Configure the connection

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UsePgHintPlan();
    optionsBuilder.UseNpgsql();
}
```


Disable Hash Join

```csharp
var items = await ctx.Items
    .EnableHashJoin(false)
    .ToListAsync();
```

Force nested loop for the joins on the tables

```csharp
var data = await ctx.Products
    .Join(ctx.Items,
        (product) => product.Id,
        (item) => item.ProductId,
        (product, item) => new
        {
            ProductId = product.Id,
            Id        = item.Id
        })
    .NestLoop(ctx.Items.EntityType, ctx.Products.EntityType)
    .ToListAsync();
```


Force index scan on the table

```csharp

var index = ctx.Items.EntityType
    .GetIndexes()
    .Where(i => i.Name == "test_index")
    .Single();

var items = await ctx.Items
    .IndexScan(ctx.Items.EntityType, index)
    .ToListAsync();
```

You can stack as many commands as you want

```csharp

var items = await ctx.Items
    .EnableHashAgg()
    .EnableHashJoin(false)
    .EnableIndexOnlyScan()
    .ToListAsync();
```

## Supported Commands:

Join methods
- `HashJoin`
- `NoHashJoin`
- `NestLoop`
- `NoNestLoop`

Scan methods
- `SeqScan`
- `TidScan`
- `IndexScan`
- `IndexOnlyScan`
- `BitmapScan`
- `IndexScanRegexp`
- `IndexOnlyScanRegexp`
- `BitmapScanRegexp`
- `NoSeqScan`
- `NoTidScan`
- `NoIndexScan`
- `NoIndexOnlyScan`
- `NoBitmapScan`

Join order
- `Leading`

Behavior control on Join
- `Memoize`
- `NoMemoize`

Row number correction
- `SetRows`

Parallel query configuration
- `Parallel`

GUC
- `EnableHashAgg`
- `EnableHashJoin`
- `EnableIndexOnlyScan`
- `EnableIndexScan`
- `EnableMaterial`
- `EnableNestLoop`
- `EnablePartitionPruning`
- `EnablePartitionWiseAggregate`
- `EnablePartitionWiseJoin`
- `EnableSeqScan`
- `EnableSort`
