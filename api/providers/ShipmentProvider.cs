using DTO.Shipment;
using FluentValidation;
using Model;

public class ShipmentProvider : BaseProvider<Shipment>
{
    private IValidator<Shipment> _shipmentValidator;

    public ShipmentProvider(AppDbContext db, IValidator<Shipment> validator) : base(db)
    {
        _shipmentValidator = validator;
    }

    public override Shipment? GetById(Guid id) => _db.Shipments.FirstOrDefault(s => s.Id == id);

    public override List<Shipment>? GetAll() => _db.Shipments.ToList();

    public override Shipment? Create(BaseDTO createValues)
    {
        ShipmentRequest? req = createValues as ShipmentRequest;
        if (req == null) throw new ApiFlowException("Could not process create shipment request. Save new shipment failed.");

        Shipment newShipment = new Shipment(newInstance: true)
        {
            OrderDate = req.OrderDate,
            RequestDate = req.RequestDate,
            ShipmentDate = req.ShipmentDate,
            ShipmentType = req.ShipmentType,
            Notes = req.Notes,
            CarrierCode = req.CarrierCode,
            CarrierDescription = req.CarrierDescription,
            ServiceCode = req.ServiceCode,
            PaymentType = req.PaymentType,
            TransferMode = req.TransferMode,
            TotalPackageCount = req.TotalPackageCount,
            TotalPackageWeight = req.TotalPackageWeight
        };

        ValidateModel(newShipment);
        _db.Shipments.Add(newShipment);
        SaveToDBOrFail();
        return newShipment;
    }

    protected override void ValidateModel(Shipment model) => _shipmentValidator.ValidateAndThrow(model);
}