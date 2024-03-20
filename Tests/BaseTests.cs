using AutoFixture;
using FluentExpressions;
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

    [Fact]
    public void ConditionTest1()
    {
        // Arrange
        var sources = _fixture.CreateMany<SourceObject>(30).ToArray();
        
        var expected = sources.Select(s => s.Size > 100 ? 1 : 0).Count(s => s == 1);
        var query = sources.AsQueryable();
        
        // Act
        var projectionCondition = ExpressionFor<SourceObject>.If(s => s.Size > 100, 1).Else(0);
        var filter = ExpressionFor<int>.Where(s => s == 1);
        
        var result = query.Select(projectionCondition).Count(filter);

        // Assert
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void ConditionTest2()
    {
        // Arrange
        var sources = _fixture.CreateMany<SourceObject>(30).ToArray();
        
        var expected = sources.Select(
            s => s.Size > 200 ? 2 
                : s.Size > 100 ? 1 
                    : 0);
        var query = sources.AsQueryable();
        
        // Act
        var projectionCondition = ExpressionFor<SourceObject>
            .If(s => s.Size > 200, 2)
            .ElseIf(s => s.Size > 100, 1)
            .Else(0);

        var result = query.Select(projectionCondition);

        // Assert
        Assert.Empty(expected.Except(result));
    }
}