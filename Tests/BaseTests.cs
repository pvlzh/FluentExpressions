using AutoFixture;
using ExpressionBuilder;
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
        var count = sources.Count(s => s.CreationDate.Year == currentYear && s.Size > sizeLimit);
        
        // Act
        var filter = ExpressionFor<SourceObject>
            .Where(s => s.CreationDate.Year == currentYear)
            .And(s => s.Size > sizeLimit);

        var result = sources.AsQueryable().Where(filter).ToArray();
        
        // Assert
        Assert.Equal(count, result.Length);
    }
    
    /// <summary>
    /// Тест проекции.
    /// </summary>
    [Fact]
    public void ProjectionTest()
    {
        
    }

}