namespace Model;

public class ItemGroup : BaseModel
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    // Navigation property for ItemGroups
    public ICollection<Item>? ItemGroups { get; set; }
}