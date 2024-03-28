using AutoFixture;
using FluentExpressions;
using Microsoft.EntityFrameworkCore;
using Tests.Classes;

namespace Tests.TranslationTests.Sqlite;

/// <summary>
/// Tests with sql translation.
/// </summary>
public class SqliteTest
{
    private readonly Fixture _fixture;
    private readonly DataContext _context;

    public SqliteTest(Fixture fixture, DataContext context) => 
        (_fixture, _context) = (fixture, context);
    
    [Fact]
    public async Task SqliteFiltrationTest1()
    {
        // Arrange
        _fixture.Customizations.Add(new RandomDateTimeSequenceGenerator());
        
        const int amountOfFunds = 5000;
        _context.AddRange(_fixture.CreateMany<Notebook>(1000));
        await _context.SaveChangesAsync();
        
        var queryNotebooks = _context.Set<Notebook>();
        var arrayNotebooks = queryNotebooks.ToArray();
        
        var expectedCount = arrayNotebooks.Count(n => 
            n.Brand == NotebookBrand.ASUS && n.DateManufacture.Year == DateTime.Now.Year && n.Price < amountOfFunds);
        
        // Act
        var isAsus = ExpressionFor<Notebook>
            .Where(n => n.Brand == NotebookBrand.ASUS);
        
        var manufactureInCurrentYear = ExpressionFor<Notebook>
            .Where(n => n.DateManufacture.Year == DateTime.Now.Year);
        
        var fitsIntoBudget = ExpressionFor<Notebook>
            .Where(n => n.Price < amountOfFunds);
        
        var filter = isAsus.And(manufactureInCurrentYear).And(fitsIntoBudget);
        var resultCount = await queryNotebooks.CountAsync(filter);
        
        // Assert
        Assert.Equal(expectedCount, resultCount);
    }
    
}