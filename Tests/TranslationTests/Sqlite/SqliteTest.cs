using System.Linq.Expressions;
using AutoFixture;
using ExpressionBuilder;
using Microsoft.EntityFrameworkCore;
using Tests.Classes.Destination;
using Tests.Classes.Source;

namespace Tests.TranslationTests.Sqlite;

public class SqliteTest
{
    private readonly Fixture _fixture;
    private readonly DataContext _context;

    public SqliteTest(Fixture fixture, DataContext context)
    {
        _fixture = fixture;
        _context = context;
    }
    
    [Fact]
    public async Task SqliteConditionsTest1()
    {
        // Arrange
        await FillDataContext();
        
        var currentYear = DateTime.UtcNow.Year;
        var sizeLimit = 10;
        
        var dbSet = _context.Set<SourceObject>();
        
        var objects = dbSet.ToArray();
        var expectedCount = objects.Count(o => 
            o.CreationDate.Year == currentYear 
            && o.Size > sizeLimit
            && o.SourceItems!.All(i => i.Price > 100));
        
        // Act
        var createdInCurrentYear = ExpressionFor<SourceObject>
            .Where(s => s.CreationDate.Year == currentYear);
        
        var sizeIsGreaterThanSizeLimit = ExpressionFor<SourceObject>
            .Where(s => s.Size > sizeLimit);
        
        var priceIsGreaterThanOneHundred = ExpressionFor<SourceItem>
            .Where(item => item.Price > 100);
        
        var allItemsPriceIsGreaterThanOneHundred = ExpressionFor<SourceObject>
            .Where(s => s.SourceItems!, items => items.All(priceIsGreaterThanOneHundred));
        
        var filter = createdInCurrentYear.And(sizeIsGreaterThanSizeLimit)
            .And(allItemsPriceIsGreaterThanOneHundred);

        var resultCount = await dbSet.CountAsync(filter);
        
        // Assert
        Assert.Equal(expectedCount, resultCount);
    }


    [Fact]
    public async Task SqliteProjectionsTest1()
    {
        // Arrange
        await FillDataContext();
        var dbSet = _context.Set<SourceObject>();
        
        // Act
        var expectedQuery = dbSet.Select(s => new DestinationObject
        {
            Id = s.Id, 
            Name = s.Name, 
            CreationDate = s.CreationDate,
            DestinationItem = new DestinationItem
            {
                Id = s.SourceItem.Id,
                Description = s.SourceItem.Description,
                Price = s.SourceItem.Price,
            },
            DestinationItems = s.SourceItems!.Select(i => new DestinationItem
            {
                Id = i.Id,
                Price = i.Price,
                Description = i.Description,
            })
        });

        
        var itemProjection = ExpressionFor<SourceItem>.Select(i => new DestinationItem
        {
            Id = i.Id,
            Description = i.Description,
            Price = i.Price,
        });
        
        var proj = ExpressionFor<SourceObject>.Select(s => new DestinationObject { 
                Id = s.Id, 
                Name = s.Name, 
                CreationDate = s.CreationDate })
            .With(to: d => d.DestinationItem, from: s => s.SourceItem, itemProjection);
            //.With(to: d => d.DestinationItems, from: s => s.SourceItems, itemProjection);
        
        var resultQuery = dbSet.Select<SourceObject, DestinationObject>(proj);
        
        var expected = await expectedQuery.ToArrayAsync();
        var result = await resultQuery.ToArrayAsync();

        // Assert


    }

    private async Task FillDataContext()
    {
        var objects = _fixture.CreateMany<SourceObject>(100);
        _context.AddRange(objects);
        await _context.SaveChangesAsync();
    }
}