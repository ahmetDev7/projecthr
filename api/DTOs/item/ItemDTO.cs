using System.Text.Json.Serialization;

namespace DTO.Item
{
    public class ItemRequest : BaseDTO
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("short_description")]
        public string? ShortDescription { get; set; }

        [JsonPropertyName("upc_code")]
        public string? UpcCode { get; set; }

        [JsonPropertyName("model_number")]
        public string? ModelNumber { get; set; }

        [JsonPropertyName("commodity_code")]
        public string? CommodityCode { get; set; }

        private int _unitPurchaseQuantity = 1;
        private int _unitOrderQuantity = 1;
        private int _packOrderQuantity = 1;

        [JsonPropertyName("unit_purchase_quantity")]
        public int UnitPurchaseQuantity
        {
            get => _unitPurchaseQuantity;
            set => _unitPurchaseQuantity = NumberUtil.MinimumInt(value, 1);
        }

        [JsonPropertyName("unit_order_quantity")]
        public int UnitOrderQuantity
        {
            get => _unitOrderQuantity;
            set => _unitOrderQuantity = NumberUtil.MinimumInt(value, 1);
        }

        [JsonPropertyName("pack_order_quantity")]
        public int PackOrderQuantity
        {
            get => _packOrderQuantity;
            set => _packOrderQuantity = NumberUtil.MinimumInt(value, 1);
        }

        [JsonPropertyName("supplier_reference_code")]
        public string? SupplierReferenceCode { get; set; }

        [JsonPropertyName("supplier_part_number")]
        public string? SupplierPartNumber { get; set; }

        [JsonPropertyName("item_group_id")]
        public Guid? ItemGroupId { get; set; }

        [JsonPropertyName("item_line_id")]
        public Guid? ItemLineId { get; set; }

        [JsonPropertyName("item_type_id")]
        public Guid? ItemTypeId { get; set; }

        [JsonPropertyName("supplier_id")]
        public Guid? SupplierId { get; set; }

        [JsonPropertyName("created_by")]
        public string? CreatedBy { get; set; }
    }

    public class ItemResponse : BaseDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("short_description")]
        public string? ShortDescription { get; set; }

        [JsonPropertyName("upc_code")]
        public string? UpcCode { get; set; }

        [JsonPropertyName("model_number")]
        public string? ModelNumber { get; set; }

        [JsonPropertyName("commodity_code")]
        public string? CommodityCode { get; set; }

        [JsonPropertyName("unit_purchase_quantity")]
        public int UnitPurchaseQuantity { get; set; }

        [JsonPropertyName("unit_order_quantity")]
        public int UnitOrderQuantity { get; set; }

        [JsonPropertyName("pack_order_quantity")]
        public int PackOrderQuantity { get; set; }

        [JsonPropertyName("supplier_reference_code")]
        public string? SupplierReferenceCode { get; set; }

        [JsonPropertyName("supplier_part_number")]
        public string? SupplierPartNumber { get; set; }

        [JsonPropertyName("item_group_id")]
        public Guid? ItemGroupId { get; set; }


        [JsonPropertyName("item_line_id")]
        public Guid? ItemLineId { get; set; }

        [JsonPropertyName("item_type_id")]
        public Guid? ItemTypeId { get; set; }

        [JsonPropertyName("supplier_id")]
        public Guid? SupplierId { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [JsonPropertyName("created_by")]
        public string? CreatedBy { get; set; }
    }
}