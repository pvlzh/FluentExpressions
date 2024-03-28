namespace Tests.Classes;
#nullable disable

/// <summary> Notebook model.</summary>
public class Notebook
{
    /// <summary> Identifier.</summary>
    public Guid Id { get; set; }
    
    /// <summary> Notebook brand.</summary>
    public NotebookBrand Brand { get; set; }
    
    /// <summary> Notebook series. </summary>
    public string Series { get; set; }
    
    /// <summary> Processor manufacturer. </summary>
    public Processor Processor { get; set; }
    
    /// <summary> Amount of RAM in GB. </summary>
    public uint RAM { get; set; }
    
    /// <summary> Amount of ROM in GB. </summary>
    public uint ROM { get; set; }
    
    /// <summary> Cost. </summary>
    public decimal Price { get; set; }
    
    /// <summary> Date of manufacture.</summary>
    public DateTime DateManufacture { get; set; }
}

/// <summary>
/// Brands.
/// </summary>
public enum NotebookBrand
{
    ASUS = 0,
    MSI = 1,
    HP = 2
}
/// <summary>
/// Processor manufacturer.
/// </summary>
public enum Processor
{
    Intel = 0,
    AMD = 0,
}