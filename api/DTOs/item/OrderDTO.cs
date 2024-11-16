using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DTO.Order
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class OrderRequest : BaseDTO
    {
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
        public Warehouse? WarehouseId  { get; set; }
        //TODO:
        //  shipment_id uuid [ref: > shipments.id]
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
        public Warehouse? WarehouseId  { get; set; }
        //TODO:
        //  shipment_id uuid [ref: > shipments.id]
    }
}