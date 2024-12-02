using DTO.Shipment;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class ShipmentProvider : BaseProvider<Shipment>
{
    private IValidator<Shipment> _shipmentValidator;

    public ShipmentProvider(AppDbContext db, IValidator<Shipment> validator) : base(db)
    {
        _shipmentValidator = validator;
    }

    public override Shipment? GetById(Guid id) => 
        _db.Shipments.Include(s => s.ShipmentItems).FirstOrDefault(s => s.Id == id);

    public override List<Shipment>? GetAll() => _db.Shipments.Include(s => s.ShipmentItems).ToList();

    public override Shipment? Create(BaseDTO createValues)
    {
        ShipmentRequest? req = createValues as ShipmentRequest;
        if (req == null) throw new ApiFlowException("Could not process create shipment request. Save new shipment failed.");

        Shipment newShipment = new Shipment(newInstance: true)
        {
            OrderIds = req.OrderIds,
            OrderDate = req.OrderDate,
            RequestDate = req.RequestDate,
            ShipmentDate = req.ShipmentDate,
            Notes = req.Notes,
            CarrierCode = req.CarrierCode,
            CarrierDescription = req.CarrierDescription,
            ServiceCode = req.ServiceCode,
            TotalPackageCount = req.TotalPackageCount,
            TotalPackageWeight = req.TotalPackageWeight,
            ShipmentItems = req.Items?.Select(item => new ShipmentItem
            {
                ItemId = item.ItemId ?? throw new ApiFlowException("Item ID cannot be null."),
                Amount = item.Amount ?? throw new ApiFlowException("Amount cannot be null.")
            }).ToList()
        };

        newShipment.SetShipmentType(req.ShipmentType);
        newShipment.SetShipmentStatus(req.ShipmentStatus);
        newShipment.SetPaymentType(req.PaymentType);
        newShipment.SetTransferMode(req.TransferMode);

        ValidateModel(newShipment);
        _db.Shipments.Add(newShipment);
        SaveToDBOrFail();
        return newShipment;
    }

    public override Shipment? Delete(Guid id)
    {
        Shipment? foundShipment = GetById(id);
        if(foundShipment == null) return null;

        _db.Shipments.Remove(foundShipment);
        SaveToDBOrFail();
        return foundShipment;
    }

    public List<Order>? GetOrdersForShipment(Guid shipmentId)
    {
        var shipment = _db.Shipments.FirstOrDefault(s => s.Id == shipmentId);

        if (shipment == null || shipment.OrderIds == null || !shipment.OrderIds.Any())
            return new List<Order>();

        return _db.Orders
            .Include(o => o.OrderItems)
            .Where(o => shipment.OrderIds.Contains(o.Id))
            .ToList();
    }

    protected override void ValidateModel(Shipment model) => _shipmentValidator.ValidateAndThrow(model);
}