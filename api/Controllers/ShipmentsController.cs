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
            // OrderId = newShipment.OrderId,
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

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] ShipmentRequest req)
    {
        Shipment? updatedShipment = _shipmentProvider.Update(id, req);

        return updatedShipment == null
            ? NotFound(new { message = $"Shipment not found for id '{id}'" })
            : Ok(new
            {
                message = "Shipment updated!",
                updated_shipment = new ShipmentResponse
                {
                    Id = updatedShipment.Id,
                    // OrderId = updatedShipment.OrderId,
                    OrderDate = updatedShipment.OrderDate,
                    RequestDate = updatedShipment.RequestDate,
                    ShipmentDate = updatedShipment.ShipmentDate,
                    ShipmentType = updatedShipment.ShipmentType.ToString(),
                    ShipmentStatus = updatedShipment.ShipmentStatus.ToString(),
                    Notes = updatedShipment.Notes,
                    CarrierCode = updatedShipment.CarrierCode,
                    CarrierDescription = updatedShipment.CarrierDescription,
                    ServiceCode = updatedShipment.ServiceCode,
                    PaymentType = updatedShipment.PaymentType.ToString(),
                    TransferMode = updatedShipment.TransferMode.ToString(),
                    TotalPackageCount = updatedShipment.TotalPackageCount,
                    TotalPackageWeight = updatedShipment.TotalPackageWeight,
                    CreatedAt = updatedShipment.CreatedAt,
                    UpdatedAt = updatedShipment.UpdatedAt,
                    Items = updatedShipment.ShipmentItems?.Select(item => new ShipmentItemRR
                    {
                        ItemId = item.ItemId,
                        Amount = item.Amount
                    }).ToList()
                }
            });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        Shipment? deletedShipment = _shipmentProvider.Delete(id);
        if (deletedShipment == null) throw new ApiFlowException($"Shipment not found for id '{id}'");

        return Ok(new
        {
            message = "Shipment deleted!",
            deleted_shipment = new ShipmentResponse
            {
                Id = deletedShipment.Id,
                // OrderId = deletedShipment.OrderId,
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
            }
        });
    }

    [HttpGet()]
    public IActionResult ShowAll() => Ok(_shipmentProvider.GetAll()?.Select(s => new ShipmentResponse
    {
        Id = s.Id,
        // OrderId = s.OrderId,
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
                // OrderId = foundShipment.OrderId,
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
}