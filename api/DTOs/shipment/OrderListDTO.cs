using DTO.Order;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DTO.OrderListRequest
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class OrderListRequest : BaseDTO
    {
        [JsonPropertyName("order_ids")]
        public List<Guid>? OrderIds { get; set; }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public class OrderListResponse : BaseDTO
    {
        [JsonPropertyName("orders")]
        public List<OrderResponse>? Orders { get; set; }
    }

}