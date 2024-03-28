using AutoFixture;
using FluentExpressions;
using Tests.Classes;

namespace Tests;

public class BaseTests
{
    private readonly Fixture _fixture = new ();
    
    /// <summary>
    /// Filtration test.
    /// </summary>
    [Fact]
    public void FiltrationTest()
    {
        _fixture.Customizations.Add(new RandomDateTimeSequenceGenerator());
        
        // Arrange
        var queryNotebooks = _fixture.CreateMany<Notebook>(1000).AsQueryable();
        var arrayNotebooks = queryNotebooks.ToArray();
        
        var expectedCount = arrayNotebooks.Count(n => 
            n.Brand == NotebookBrand.HP && n.DateManufacture.Year == DateTime.Now.Year - 1);

        // Act
        var isHp = ExpressionFor<Notebook>
            .Where(n => n.Brand == NotebookBrand.HP);
        
        var createdInPastYear = ExpressionFor<Notebook>
            .Where(n => n.DateManufacture.Year == DateTime.Now.Year - 1);
        
        var result = queryNotebooks.Count(isHp.And(createdInPastYear));
        
        // Assert
        Assert.Equal(expectedCount, result);
    }

}