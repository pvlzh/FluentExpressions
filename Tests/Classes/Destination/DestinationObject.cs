using System.Collections;

namespace Tests.Classes.Destination;

public class DestinationObject
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    
    public DateTime CreationDate { get; set; }

    public DestinationItem DestinationItem { get; set; }
    public IEnumerable<DestinationItem> DestinationItems { get; set; } = null!;
}