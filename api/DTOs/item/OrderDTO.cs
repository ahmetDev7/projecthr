using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DTO.Order
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class OrderRequest : BaseDTO
    {
        [JsonPropertyName("order_date")]
        public DateTime? OrderDate { get; set; }
        [JsonPropertyName("request_date")]
        public DateTime? RequestDate { get; set; }
        [JsonPropertyName("reference")]
        public string? Reference{ get; set; }
        [JsonPropertyName("reference_extra")]
        public string? ReferenceExtra { get; set; }
        [JsonPropertyName("order_status")]
        public string? OrderStatus  { get; set; }
        [JsonPropertyName("note")]
        public string? Notes  { get; set; }
        [JsonPropertyName("picking_notes")]
        public string? PickingNotes  { get; set; }
        [JsonPropertyName("total_amount")]
        public decimal? TotalAmount { get; set;}

        [JsonPropertyName("total_discount")]
        public decimal? TotalDiscount { get; set;}
        [JsonPropertyName("total_tax")]
        public decimal? TotalTax { get; set;}

        [JsonPropertyName("total_surcharge")]
        public decimal? TotalSurcharge { get; set;}
        [JsonPropertyName("warehouse_id")]
        public Guid? WarehouseId  { get; set; }
        [JsonPropertyName("order_items")]
        public List<OrderItemRequest>? OrderItems { get; set; }
        
        
        //TODO:
        //  shipment_id uuid [ref: > shipments.id]
        //TODO..
        //  ship_to_client uuid [ref: > clients.id]
        //[Required]
        //bill_to_client uuid [ref: > clients.id]
    }
    
    [ApiExplorerSettings(IgnoreApi = true)]
    public class OrderResponse  : BaseDTO
    {
        [JsonPropertyName("id")]
        public Guid Id {get; set;}
        [JsonPropertyName("order_date")]
        public DateTime? OrderDate { get; set; }
        [JsonPropertyName("request_date")]
        public DateTime? RequestDate { get; set; }
        [JsonPropertyName("reference")]
        public string? Reference{ get; set; }
        [JsonPropertyName("reference_extra")]
        public string? ReferenceExtra { get; set; }
        [JsonPropertyName("order_status")]
        public string? OrderStatus  { get; set; }
        [JsonPropertyName("note")]
        public string? Notes  { get; set; }
        [JsonPropertyName("picking_notes")]
        public string? PickingNotes  { get; set; }

        [JsonPropertyName("total_amount")]
        public decimal? TotalAmount { get; set;}

        [JsonPropertyName("total_discount")]
        public decimal? TotalDiscount { get; set;}
        [JsonPropertyName("total_tax")]
        public decimal? TotalTax { get; set;}

        [JsonPropertyName("total_surcharge")]
        public decimal? TotalSurcharge { get; set;}
        [JsonPropertyName("warehouseid")]
        public Guid? WarehouseId  { get; set; }
        [JsonPropertyName("order_items")]
        public List<OrderItemRequest>? Items { get; set; }
        //TODO..
        //  ship_to_client uuid [ref: > clients.id]
        //[Required]
        //bill_to_client uuid [ref: > clients.id]}
  
        //TODO:
        //  shipment_id uuid [ref: > shipments.id]
    }
    public class OrderItemRequest  : BaseDTO
    {
        [JsonPropertyName("item_id")]
        public Guid? ItemId { get; set; }
        [JsonPropertyName("amount")]
        public int? Amount { get; set; } // 10 fietsen besteld


    }
}