namespace Tests.Classes.Source;

public class SourceObject
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    
    public byte Size { get; set; }
    
    public DateTime CreationDate { get; set; }

    public IEnumerable<SourceItem> SourceItems { get; set; } = null!;
}