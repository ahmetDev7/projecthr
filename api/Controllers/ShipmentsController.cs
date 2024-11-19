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

    [HttpGet()]
    public IActionResult ShowAll() => Ok(_shipmentProvider.GetAll()?.Select(s => new ShipmentResponse
    {
        Id = s.Id,
        OrderDate = s.OrderDate,
        RequestDate = s.RequestDate,
        ShipmentDate = s.ShipmentDate,
        ShipmentType = s.ShipmentType,
        Notes = s.Notes,
        CarrierCode = s.CarrierCode,
        CarrierDescription = s.CarrierDescription,
        ServiceCode = s.ServiceCode,
        PaymentType = s.PaymentType,
        TransferMode = s.TransferMode,
        TotalPackageCount = s.TotalPackageCount,
        TotalPackageWeight = s.TotalPackageWeight,
        CreatedAt = s.CreatedAt,
        UpdatedAt = s.UpdatedAt,
        Items = s.ShipmentItems?.Select(item => new ShipmentItemRR
        {
            ItemId = item.ItemId,
            Amount = item.Amount
        }).ToList()
    }).ToList());
}