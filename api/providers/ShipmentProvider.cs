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

    protected override void ValidateModel(Shipment model) => _shipmentValidator.ValidateAndThrow(model);
}