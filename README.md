# PgHintPlan

## Getting Started

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
var items = await ctx.Items.EnableHashJoin(false).ToListAsync();
```

Force nested loop for the joins on the tables

```csharp
var DATA = await ctx.Products
                .Join(ctx.Items,
                (p) => p.Id,
                (i) => i.ProductId,
                (p, i) => new
                {
                    ProductId = p.Id,
                    Id        = i.Id
                })
                .NestLoop(ctx.Items.EntityType, ctx.Products.EntityType)
                .ToListAsync();
```

Force index scan on the table

```csharp
var index = ctx.Items.EntityType.GetIndexes().FirstOrDefault();
var items = await ctx.Items.IndexScan(ctx.Items.EntityType,index).ToListAsync();
```


