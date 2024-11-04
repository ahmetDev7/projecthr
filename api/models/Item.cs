using Utils.Number;

public class Item
{
    public Item()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Description { get; set; }
    public required string ShortDescription { get; set; }
    public required string UpcCode { get; set; }
    public required string ModelNumber { get; set; }
    public required string CommodityCode { get; set; }
    private int _unitPurchaseQuantity = 0;
    private int _unitOrderQuantity = 0;
    private int _packOrderQuantity = 0;
    public required int UnitPurchaseQuantity
    {
        get => _unitPurchaseQuantity;
        set => _unitPurchaseQuantity = NumberUtil.EnsureNonNegative(value);
    }
    public required int UnitOrderQuantity
     { 
        get => _unitOrderQuantity; 
        set => _unitOrderQuantity = NumberUtil.EnsureNonNegative(value); 
    }
    public required int PackOrderQuantity { 
        get => _packOrderQuantity; 
        set => _packOrderQuantity = NumberUtil.EnsureNonNegative(value); 
    }

    public required string SupplierReferenceCode { get; set; }
    public required string SupplierPartNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    /*
    TODO: 
        item_line uuid [ref: > item_lines.id]
        item_group uuid [ref: > item_groups.id]
        item_type uuid [ref: > item_types.id]
        supplier_id uuid [ref: > suppliers.id] 
    */

}
