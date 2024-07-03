## FluentExpressions

This library is designed to simplify the construction of lambda expressions. It utilizes the fluent object construction approach to facilitate the creation of lambda expressions in a more readable and maintainable manner.

Simple examples:
```csharp
    var filter = ExpressionFor<Notebook>
        .Where(n => n.Brand == targetBrand)
        .And(n => n.Price >= lowPriceBound)
        .And(n => n.Price <= upPriceBound);
    
    ... = _dataContext.Set<Notebook>().Where(filter);
```
```csharp
    var amountOfCores = ExpressionFor<Notebook>
        .If(n => n.Processor == Processor.Intel, minCoresForIntel)
        .Else(minCoresForAmd);
        
    ... = _dataContext.Set<Notebook>()
        .Count(amountOfCores.LessThan(n => n.Cores));
```
```csharp
   var maxCost = ExpressionFor<Notebook>
        .If(n => n.Brand == NotebookBrand.ASUS, maxPriceForAsus)
        .ElseIf(n => n.Brand == NotebookBrand.HP, maxPriceForHp)
        .Else(maxPriceForOther);
    
   ... = _dataContext.Set<Notebook>()
       .Count(maxCost.GreaterThan(n => n.Price));
```
```csharp
   var correspondsBrands = ExpressionFor<Notebook>.Start();
   foreach (var brand in brands) {
       correspondsBrands = correspondsBrands.Or(n => n.Brand == brand);
   }

   var createdInCurrentYear = ExpressionFor<Notebook>
       .Where(n => n.DateManufacture.Year == DateTime.Now.Year);
   
   var filter = createdInCurrentYear.And(correspondsBrands);
   ... = _dataContext.Set<Notebook>().Where(filter);
```

The following improvements are planned
* Fluent construction of entity projection expressions 
```csharp
var projection = ProjectionFor<ObjectA>
    .Select(a => new ProjectB {
        Name = a.LastName + a.FirstName,
        Age = a.Age})
    .With(b => b.Contacts, a => a.Contacts, 
        ExpressionFor<AContact>.Select(ac => new BContact{
            Type = ac.Type,
            Value = ac.Value
        }));

   ... = _dataContext.Set<Notebook>().Select(projection);
```
