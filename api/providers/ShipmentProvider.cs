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
            OrderId = req.OrderId,
            OrderDate = req.OrderDate,
            RequestDate = req.RequestDate,
            ShipmentDate = req.ShipmentDate,
            Notes = req.Notes,
            CarrierCode = req.CarrierCode,
            CarrierDescription = req.CarrierDescription,
            ServiceCode = req.ServiceCode,
            TransferMode = req.TransferMode,
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

    protected override void ValidateModel(Shipment model) => _shipmentValidator.ValidateAndThrow(model);
}