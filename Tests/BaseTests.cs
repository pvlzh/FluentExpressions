using AutoFixture;
using ExpressionBuilder;
using Tests.Classes.Destination;
using Tests.Classes.Source;

namespace Tests;

public class BaseTests
{
    private readonly Fixture _fixture;

    public BaseTests(Fixture fixture)
    {
        _fixture = fixture;
    }
    
    /// <summary>
    /// Тест фильтрации.
    /// </summary>
    [Fact]
    public void FiltrationTest()
    {
        _fixture.Customizations.Add(new RandomDateTimeSequenceGenerator());
        
        // Arrange
        const int sizeLimit = 10;
        var currentYear = DateTime.UtcNow.Year;
        
        var sources = _fixture.CreateMany<SourceObject>(30).ToArray();
        var count = sources.Count(s => s.CreationDate.Year == currentYear 
                                       && s.Size > sizeLimit 
                                       && s.Name.StartsWith(nameof(SourceObject.Name))
                                       && s.SourceItems!.All(i => i.Price > 100));
        
        // Act
        var createdInCurrentYear = ExpressionFor<SourceObject>
            .Where(s => s.CreationDate.Year == currentYear);
        
        var sizeIsGreaterThanSizeLimit = ExpressionFor<SourceObject>
            .Where(s => s.Size > sizeLimit);
        
        var priceIsGreaterThanOneHundred = ExpressionFor<SourceItem>
            .Where(item => item.Price > 100);
        
        var allItemsPriceIsGreaterThanOneHundred = ExpressionFor<SourceObject>
            .Where(s => s.SourceItems!, items => items.All(priceIsGreaterThanOneHundred));
        
        var filter = createdInCurrentYear
            .And(sizeIsGreaterThanSizeLimit)
            .And(allItemsPriceIsGreaterThanOneHundred);
        
        var result = sources.AsQueryable().Count(filter);
        
        // Assert
        Assert.Equal(count, result);
    }
    
    /// <summary>
    /// Тест проекции.
    /// </summary>
    [Fact]
    public void ProjectionTest()
    {
        // Arrange
        var sources = _fixture.CreateMany<SourceObject>(30).ToArray();
        var query = sources.AsQueryable();
        // Act

        var itemProjection = ExpressionFor<SourceItem>.Select(i => new DestinationItem
            { Id = i.Id, Description = i.Description, Price = i.Price });
        
        var projectionExpression = ExpressionFor<SourceObject>.Select(s => 
                new DestinationObject {
                    Id = s.Id, 
                    Name = s.Name, 
                    CreationDate = s.CreationDate })
            .With(to: d => d.DestinationItem, from: s => s.SourceItem, itemProjection);

        var result = query.Select<SourceObject, DestinationObject>(projectionExpression).ToArray();

        // Assert
        Assert.Equal(sources.Length, result.Length);
    }
}