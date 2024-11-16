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
        [JsonPropertyName("ship_to_client")]
        public string? ShipToClient  { get; set; }
        [JsonPropertyName("picking_notes")]
        public string? PickingNotes  { get; set; }
        [JsonPropertyName("total_amount")]
        public float? TotalAmount  { get; set; }
        [JsonPropertyName("total_discount")]
        public float? TotalDiscount  { get; set; }
        [JsonPropertyName("total_tax")]
        public float? TotalTax  { get; set; }
        [JsonPropertyName("total_surcharge")]
        public float? TotalSurcharge  { get; set; }
        [JsonPropertyName("warehouse_id")]
        public Guid? WarehouseId  { get; set; }
        
        
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
        [JsonPropertyName("orderdate")]
        public DateTime? OrderDate { get; set; }
        [JsonPropertyName("requestdate")]
        public DateTime? RequestDate { get; set; }
        [JsonPropertyName("reference")]
        public string? Reference{ get; set; }
        [JsonPropertyName("referenceextra")]
        public string? ReferenceExtra { get; set; }
        [JsonPropertyName("orderstatus")]
        public string? OrderStatus  { get; set; }
        [JsonPropertyName("note")]
        public string? Notes  { get; set; }
        [JsonPropertyName("shiptoclient")]
        public string? ShipToClient  { get; set; }
        [JsonPropertyName("pickingnotes")]
        public string? PickingNotes  { get; set; }
        //TODO..
        //  ship_to_client uuid [ref: > clients.id]
        //[Required]
        //bill_to_client uuid [ref: > clients.id]
        [JsonPropertyName("totalamount")]
        public float? TotalAmount  { get; set; }
        [JsonPropertyName("totaldiscount")]
        public float? TotalDiscount  { get; set; }
        [JsonPropertyName("totaltax")]
        public float? TotalTax  { get; set; }
        [JsonPropertyName("totalsurcharge")]
        public float? TotalSurcharge  { get; set; }
        [JsonPropertyName("warehouseid")]
        public Guid? WarehouseId  { get; set; }
        //TODO:
        //  shipment_id uuid [ref: > shipments.id]
    }
}