using AutoFixture;
using FluentExpressions;
using FluentExpressions.Extensions;
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
        // Arrange
        var queryNotebooks = _fixture.CreateMany<Notebook>(10000).AsQueryable();
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

    /// <summary>
    /// LessThan extension method test.
    /// </summary>
    [Fact]
    public void LessThanTest()
    {
        // Arrange
        const int MIN_CORES_FOR_INTEL = 20; 
        const int MIN_CORES_FOR_AMD = 12; 
        
        var queryNotebooks = _fixture.CreateMany<Notebook>(10000).AsQueryable();
        var arrayNotebooks = queryNotebooks.ToArray();
        
        var expectedCount = arrayNotebooks.Count(n => 
            (n.Processor == Processor.Intel 
                ? MIN_CORES_FOR_INTEL : MIN_CORES_FOR_AMD) < n.Cores);
        
        // Act
        var amountOfCores = ExpressionFor<Notebook>
            .If(n => n.Processor == Processor.Intel, MIN_CORES_FOR_INTEL)
            .Else(MIN_CORES_FOR_AMD);
        
        var result = queryNotebooks.Count(amountOfCores.LessThan(n => n.Cores));
        
        // Assert
        Assert.Equal(expectedCount, result);
    }
    
    /// <summary>
    /// GreaterThan extension method test.
    /// </summary>
    [Fact]
    public void GreaterThanTest()
    {
        // Arrange
        const int MAX_PRICE_FOR_ASUS = 5000; 
        const int MAX_PRICE_FOR_OTHER = 3000; 
        
        var queryNotebooks = _fixture.CreateMany<Notebook>(10000).AsQueryable();
        var arrayNotebooks = queryNotebooks.ToArray();
        
        var expectedCount = arrayNotebooks.Count(n => 
            (n.Brand == NotebookBrand.ASUS ? MAX_PRICE_FOR_ASUS : MAX_PRICE_FOR_OTHER) > n.Price);
        
        // Act
        var maxCost = ExpressionFor<Notebook>
            .If(n => n.Brand == NotebookBrand.ASUS, MAX_PRICE_FOR_ASUS)
            .Else(MAX_PRICE_FOR_OTHER);
        
        var result = queryNotebooks.Count(maxCost.GreaterThan(n => (int)n.Price));
        
        // Assert
        Assert.Equal(expectedCount, result);
    }
}