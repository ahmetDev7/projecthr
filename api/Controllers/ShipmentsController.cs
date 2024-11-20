using DTO.Shipment;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ShipmentsController : ControllerBase
{
    private ShipmentProvider _shipmentProvider;

    public ShipmentsController(ShipmentProvider shipmentProvider)
    {
        _shipmentProvider = shipmentProvider;
    }

    [HttpPost()]
    public IActionResult Create([FromBody] ShipmentRequest req)
    {
        Shipment? newShipment = _shipmentProvider.Create(req);
        if (newShipment == null) throw new ApiFlowException("Saving new Shipment failed.");


        return Ok(new ShipmentResponse
        {
            Id = newShipment.Id,
            OrderDate = newShipment.OrderDate,
            RequestDate = newShipment.RequestDate,
            ShipmentDate = newShipment.ShipmentDate,
            ShipmentType = newShipment.ShipmentType,
            Notes = newShipment.Notes,
            CarrierCode = newShipment.CarrierCode,
            CarrierDescription = newShipment.CarrierDescription,
            ServiceCode = newShipment.ServiceCode,
            PaymentType = newShipment.PaymentType,
            TransferMode = newShipment.TransferMode,
            TotalPackageCount = newShipment.TotalPackageCount,
            TotalPackageWeight = newShipment.TotalPackageWeight,
            CreatedAt = newShipment.CreatedAt,
            UpdatedAt = newShipment.UpdatedAt,
            Items = newShipment.ShipmentItems?.Select(si => new ShipmentItemRR
            {
                ItemId = si.ItemId,
                Amount = si.Amount
            }).ToList()
        });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        Shipment? deletedShipment = _shipmentProvider.Delete(id);
        if (deletedShipment == null) throw new ApiFlowException($"Shipment not found for id '{id}'");

        return Ok(new {deleted_shipment = new ShipmentResponse{
            Id = deletedShipment.Id,
            OrderDate = deletedShipment.OrderDate,
            RequestDate = deletedShipment.RequestDate,
            ShipmentDate = deletedShipment.ShipmentDate,
            ShipmentType = deletedShipment.ShipmentType,
            Notes = deletedShipment.Notes,
            CarrierCode = deletedShipment.CarrierCode,
            CarrierDescription = deletedShipment.CarrierDescription,
            ServiceCode = deletedShipment.ServiceCode,
            PaymentType = deletedShipment.PaymentType,
            TransferMode = deletedShipment.TransferMode,
            TotalPackageCount = deletedShipment.TotalPackageCount,
            TotalPackageWeight = deletedShipment.TotalPackageWeight,
            CreatedAt = deletedShipment.CreatedAt,
            UpdatedAt = deletedShipment.UpdatedAt,
            Items = deletedShipment.ShipmentItems?.Select(si => new ShipmentItemRR
            {
                ItemId = si.ItemId,
                Amount = si.Amount
            }).ToList()
        }});

    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        Shipment? foundShipment = _shipmentProvider.GetById(id);

        return (foundShipment == null)
            ? NotFound(new { message = $"Shipment not found for id '{id}'" })
            : Ok(new ShipmentResponse
            {        
                Id = foundShipment.Id,
                OrderDate = foundShipment.OrderDate,
                RequestDate = foundShipment.RequestDate,
                ShipmentDate = foundShipment.ShipmentDate,
                ShipmentType = foundShipment.ShipmentType,
                Notes = foundShipment.Notes,
                CarrierCode = foundShipment.CarrierCode,
                CarrierDescription = foundShipment.CarrierDescription,
                ServiceCode = foundShipment.ServiceCode,
                PaymentType = foundShipment.PaymentType,
                TransferMode = foundShipment.TransferMode,
                TotalPackageCount = foundShipment.TotalPackageCount,
                TotalPackageWeight = foundShipment.TotalPackageWeight,
                CreatedAt = foundShipment.CreatedAt,
                UpdatedAt = foundShipment.UpdatedAt,
                Items = foundShipment.ShipmentItems?.Select(item => new ShipmentItemRR
                {
                    ItemId = item.ItemId,
                    Amount = item.Amount
                }).ToList()
        });
    }
}