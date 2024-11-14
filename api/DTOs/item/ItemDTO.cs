using Utils.Number;

namespace DTO.Item
{
    public class Base : BaseDTO
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public string? UpcCode { get; set; }
        public string? ModelNumber { get; set; }
        public string? CommodityCode { get; set; }
        private int _unitPurchaseQuantity = 0;
        private int _unitOrderQuantity = 0;
        private int _packOrderQuantity = 0;
        public int UnitPurchaseQuantity
        {
            get => _unitPurchaseQuantity;
            set => _unitPurchaseQuantity = NumberUtil.EnsureNonNegative(value);
        }
        public int UnitOrderQuantity
        {
            get => _unitOrderQuantity;
            set => _unitOrderQuantity = NumberUtil.EnsureNonNegative(value);
        }
        public int PackOrderQuantity
        {
            get => _packOrderQuantity;
            set => _packOrderQuantity = NumberUtil.EnsureNonNegative(value);
        }

        public string? SupplierReferenceCode { get; set; }
        public string? SupplierPartNumber { get; set; }
    }

    public class Result : BaseDTO
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public string? UpcCode { get; set; }
        public string? ModelNumber { get; set; }
        public string? CommodityCode { get; set; }
        private int _unitPurchaseQuantity = 0;
        private int _unitOrderQuantity = 0;
        private int _packOrderQuantity = 0;
        public int UnitPurchaseQuantity
        {
            get => _unitPurchaseQuantity;
            set => _unitPurchaseQuantity = NumberUtil.EnsureNonNegative(value);
        }
        public int UnitOrderQuantity
        {
            get => _unitOrderQuantity;
            set => _unitOrderQuantity = NumberUtil.EnsureNonNegative(value);
        }
        public int PackOrderQuantity
        {
            get => _packOrderQuantity;
            set => _packOrderQuantity = NumberUtil.EnsureNonNegative(value);
        }

        public string? SupplierReferenceCode { get; set; }
        public string? SupplierPartNumber { get; set; }
    }
}