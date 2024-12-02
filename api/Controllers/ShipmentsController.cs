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
            OrderIds = newShipment.OrderIds,
            OrderDate = newShipment.OrderDate,
            RequestDate = newShipment.RequestDate,
            ShipmentDate = newShipment.ShipmentDate,
            ShipmentType = newShipment.ShipmentType.ToString(),
            ShipmentStatus = newShipment.ShipmentStatus.ToString(),
            Notes = newShipment.Notes,
            CarrierCode = newShipment.CarrierCode,
            CarrierDescription = newShipment.CarrierDescription,
            ServiceCode = newShipment.ServiceCode,
            PaymentType = newShipment.PaymentType.ToString(),
            TransferMode = newShipment.TransferMode.ToString(),
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

        return Ok(new {
            message = "Shipment deleted!",
            deleted_shipment = new ShipmentResponse{
            Id = deletedShipment.Id,
            OrderIds = deletedShipment.OrderIds,
            OrderDate = deletedShipment.OrderDate,
            RequestDate = deletedShipment.RequestDate,
            ShipmentDate = deletedShipment.ShipmentDate,
            ShipmentType = deletedShipment.ShipmentType.ToString(),
            ShipmentStatus = deletedShipment.ShipmentStatus.ToString(),
            Notes = deletedShipment.Notes,
            CarrierCode = deletedShipment.CarrierCode,
            CarrierDescription = deletedShipment.CarrierDescription,
            ServiceCode = deletedShipment.ServiceCode,
            PaymentType = deletedShipment.PaymentType.ToString(),
            TransferMode = deletedShipment.TransferMode.ToString(),
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
    }

    [HttpGet()]
    public IActionResult ShowAll() => Ok(_shipmentProvider.GetAll()?.Select(s => new ShipmentResponse
    {
        Id = s.Id,
        OrderIds = s.OrderIds,
        OrderDate = s.OrderDate,
        RequestDate = s.RequestDate,
        ShipmentDate = s.ShipmentDate,
        ShipmentType = s.ShipmentType.ToString(),
        ShipmentStatus = s.ShipmentStatus.ToString(),
        Notes = s.Notes,
        CarrierCode = s.CarrierCode,
        CarrierDescription = s.CarrierDescription,
        ServiceCode = s.ServiceCode,
        PaymentType = s.PaymentType.ToString(),
        TransferMode = s.TransferMode.ToString(),
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

    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        Shipment? foundShipment = _shipmentProvider.GetById(id);

        return (foundShipment == null)
            ? NotFound(new { message = $"Shipment not found for id '{id}'" })
            : Ok(new ShipmentResponse
            {        
                Id = foundShipment.Id,
                OrderIds = foundShipment.OrderIds,
                OrderDate = foundShipment.OrderDate,
                RequestDate = foundShipment.RequestDate,
                ShipmentDate = foundShipment.ShipmentDate,
                ShipmentType = foundShipment.ShipmentType.ToString(),
                ShipmentStatus = foundShipment.ShipmentStatus.ToString(),
                Notes = foundShipment.Notes,
                CarrierCode = foundShipment.CarrierCode,
                CarrierDescription = foundShipment.CarrierDescription,
                ServiceCode = foundShipment.ServiceCode,
                PaymentType = foundShipment.PaymentType.ToString(),
                TransferMode = foundShipment.TransferMode.ToString(),
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

    [HttpGet("{id}/orders")]
    public IActionResult ShowOrders(Guid id)
    {
        return null;
    }
}