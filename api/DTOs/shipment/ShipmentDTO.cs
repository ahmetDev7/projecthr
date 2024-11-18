using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Utils.Number;

namespace DTO.Shipment
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ShipmentRequest : BaseDTO
    {
        [JsonPropertyName("order_date")]
        public DateTime? OrderDate { get; set; }

        [JsonPropertyName("request_date")]
        public DateTime? RequestDate { get; set; }
        
        [JsonPropertyName("shipment_date")]
        public DateTime? ShipmentDate { get; set; }
        
        [JsonPropertyName("shipment_status")]
        public string? ShipmentType { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }
        
        [JsonPropertyName("carrier_code")]
        public string? CarrierCode { get; set; }

        [JsonPropertyName("carrier_description")]
        public string? CarrierDescription { get; set; }

        [JsonPropertyName("service_code")]
        public string? ServiceCode { get; set; }

        [JsonPropertyName("payment_type")]
        public string? PaymentType { get; set; }

        [JsonPropertyName("transfer_mode")]
        public string? TransferMode { get; set; }

        private int _totalPackageCount = 0;
        
        [JsonPropertyName("total_package_count")]
        public int? TotalPackageCount
        {
            get => _totalPackageCount;
            set => _totalPackageCount = NumberUtil.EnsureNonNegative((int)value);
        }

        private decimal _totalPackageWeight = 0;

        [JsonPropertyName("total_package_weight")]
        public decimal? TotalPackageWeight
        {
            get => _totalPackageWeight;
            set => _totalPackageWeight = NumberUtil.EnsureNonNegativeWithFourDecimals((decimal)value);
        }

        // TODO: Add Order ID (UUID)
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ShipmentResponse : BaseDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("order_date")]
        public DateTime? OrderDate { get; set; }

        [JsonPropertyName("request_date")]
        public DateTime? RequestDate { get; set; }
        
        [JsonPropertyName("shipment_date")]
        public DateTime? ShipmentDate { get; set; }
        
        [JsonPropertyName("shipment_status")]
        public string? ShipmentType { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }
        
        [JsonPropertyName("carrier_code")]
        public string? CarrierCode { get; set; }

        [JsonPropertyName("carrier_description")]
        public string? CarrierDescription { get; set; }

        [JsonPropertyName("service_code")]
        public string? ServiceCode { get; set; }

        [JsonPropertyName("payment_type")]
        public string? PaymentType { get; set; }

        [JsonPropertyName("transfer_mode")]
        public string? TransferMode { get; set; }

        [JsonPropertyName("total_package_count")]
        public int? TotalPackageCount { get; set; }

        [JsonPropertyName("total_package_weight")]
        public decimal? TotalPackageWeight { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
        
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("items")]
        public List<ShipmentItemResponse>? Items { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ShipmentItemResponse : BaseDTO
    {
        [JsonPropertyName("item_id")]
        public Guid? ItemId { get; set; }

        [JsonPropertyName("amount")]
        public int? Amount { get; set; }
    }
}