using System.ComponentModel.DataAnnotations;
using Utils.Number;

public class Item : BaseModel
{
    public Item() { }
    public Item(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    [Required]
    public string? UpcCode { get; set; }
    [Required]
    public string? ModelNumber { get; set; }
    public string? CommodityCode { get; set; }
    private int _unitPurchaseQuantity = 1;
    private int _unitOrderQuantity = 1;
    private int _packOrderQuantity = 1;
    [Required]
    public int UnitPurchaseQuantity
    {
        get => _unitPurchaseQuantity;
        set => _unitPurchaseQuantity = NumberUtil.MinimumInt(value, 1);
    }
    [Required]
    public int UnitOrderQuantity
    {
        get => _unitOrderQuantity;
        set => _unitOrderQuantity = NumberUtil.MinimumInt(value, 1);
    }
    [Required]
    public int PackOrderQuantity
    {
        get => _packOrderQuantity;
        set => _packOrderQuantity = NumberUtil.MinimumInt(value, 1);
    }

    [Required]
    public string? SupplierReferenceCode { get; set; } // original name was supplier_part_number
    public string? SupplierPartNumber { get; set; }

    // Foreign Key Relationship
    public Guid? ItemGroupId { get; set; }
    public ItemGroup? ItemGroup { get; set; }
    public Guid? SupplierId { get; set; }

    public ICollection<OrderItem>? OrderItems { get; set; }

    /*
    TODO: 
        item_line uuid [ref: > item_lines.id]
        item_type uuid [ref: > item_types.id]
        supplier_id uuid [ref: > suppliers.id] [Required]
    */
    
    // shipment_items table connection
    public ICollection<ShipmentItem>? ShipmentItems { get; set; }
}
