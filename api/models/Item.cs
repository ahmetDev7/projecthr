using System.ComponentModel.DataAnnotations;

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

    public Guid? ItemGroupId { get; set; } // Foreign Key Relationship with ItemGroups
    public ItemGroup? ItemGroup { get; set; }
    [Required]
    public Guid? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public Guid? ItemLineId { get; set; } // Foreign Key Relationship with ItemLines
    public ItemLine? ItemLine { get; set; }

    public Guid? ItemTypeId { get; set; } // Foreign Key Relationship with ItemLines
    public ItemType? ItemType { get; set; }

    public ICollection<OrderItem>? OrderItems { get; set; }
    public ICollection<ShipmentItem>? ShipmentItems { get; set; } // shipment_items table connection

    public Inventory? Inventory { get; set; }
    public ICollection<TransferItem>? TransferItems { get; set; } // transfer_items table connection

    public string? CreatedBy { get; set; }
}
