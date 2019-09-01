# Gobln.OrderBy

Gobln.Orderby allow you to order IEnumerable or IQueryable by dynamic properties.
Also you can define IComparer to the dynamic property.

You can also define an array of dynamic properties by which to order.

This libery also contains a natural sort comparer

## Frameworks

* .Net 4.0 and higher
* .Net Core 1.0 and higher
* .Net Core 2.0 and higher
* .Net Standard 2.0 and higher

## OrderBy

### How to use

Install Gobln.OrderBy, trough [Nuget](https://nuget.org/) or other means.

### Examples

```csharp

TestData.MoonWalkers.OrderBy(" firstname       Desc; LunarEvaDate asc ; ").ToList();


TestData.MoonWalkers.OrderBy("lunarevadate", OrderDirectionEnum.Descending).ToArray()


TestData.MoonWalkers.OrderBy("FirstName", new NaturalSortComparer()).ThenBy("GuiId", OrderDirectionEnum.Ascending, new NaturalSortComparer()).ToList()


var orderItems = new List<OrderItem>{
                    new OrderItem(){ SortColum = "LunarEvaDate", OrderDirection = OrderDirectionEnum.Descending },
                    new OrderItem(){ SortColum = "Remark" }
                };

TestData.MoonWalkers.OrderBy(orderItems).ToArray();

```

For more examples, check the test project

## Installing Gobln.OrderBy

The project is on [Nuget](https://www.nuget.org/packages/Gobln.OrderBy/). Install via the NuGet Package Manager.

PM > Install-Package Gobln.OrderBy

## License

[Apache License, Version 2.0](http://opensource.org/licenses/Apache-2.0).

## Documentation and Readme file

I'm going to provide an documentation file, but haven't started on one yet.
As for the Readme file, if there are any inconsitencies or grammatical errors feel free to let me know by an pull request. This also counts for problems in de code.

## Issues and Contributions

* If something is broken and you know how to fix it, send a pull request.
* If you have no idea what is wrong, create an issue.

## Any feedback and contributions are welcome

If you have something you'd like to improve do not hesitate to send a Pull Request