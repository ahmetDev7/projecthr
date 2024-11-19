using DTO.Shipment;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ShipmentController : ControllerBase
{
    private ShipmentProvider _shipmentProvider;

    public ShipmentController(ShipmentProvider shipmentProvider)
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
}