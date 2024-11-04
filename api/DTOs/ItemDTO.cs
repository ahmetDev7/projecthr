using Utils.Number;

namespace DTOs;
public class ItemDTO : IDTO
{
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
    public required int PackOrderQuantity
    {
        get => _packOrderQuantity;
        set => _packOrderQuantity = NumberUtil.EnsureNonNegative(value);
    }

    public required string SupplierReferenceCode { get; set; }
    public required string SupplierPartNumber { get; set; }
}