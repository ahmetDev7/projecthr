using DTO.Shipment;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Utils.Date;

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

    public override Shipment? Update(Guid id, BaseDTO updateValues)
    {
        ShipmentRequest? req = updateValues as ShipmentRequest;
        if (req == null) throw new ApiFlowException("Could not process update shipment request. Update failed.");

        Shipment? existingShipment = _db.Shipments.Include(o => o.ShipmentItems).FirstOrDefault(o => o.Id == id);
        if (existingShipment == null) throw new ApiFlowException($"Shipment not found for id '{id}'");

        existingShipment.OrderId = req.OrderId;
        existingShipment.OrderDate = DateUtil.ToUtcOrNull(req.OrderDate);
        existingShipment.RequestDate = DateUtil.ToUtcOrNull(req.RequestDate);
        existingShipment.ShipmentDate = DateUtil.ToUtcOrNull(req.ShipmentDate);
        existingShipment.SetShipmentType(req.ShipmentType);
        existingShipment.SetShipmentStatus(req.ShipmentStatus);
        existingShipment.Notes = req.Notes;
        existingShipment.CarrierCode = req.CarrierCode;
        existingShipment.CarrierDescription = req.CarrierDescription;
        existingShipment.ServiceCode = req.ServiceCode;
        existingShipment.SetPaymentType(req.PaymentType);
        existingShipment.SetTransferMode(req.TransferMode);
        existingShipment.TotalPackageCount = req.TotalPackageCount;
        existingShipment.TotalPackageWeight = req.TotalPackageWeight;
        existingShipment.SetUpdatedAt();
        if (req.Items != null)
        {
            _db.ShipmentItems.RemoveRange(existingShipment.ShipmentItems);

            existingShipment.ShipmentItems = req.Items.Select(si => new ShipmentItem(newInstance: true)
            {
                ItemId = si.ItemId,
                Amount = si.Amount
            }).ToList();
        }

        ValidateModel(existingShipment);

        _db.Shipments.Update(existingShipment);
        SaveToDBOrFail();

        return existingShipment;
    }

    public override Shipment? Delete(Guid id)
    {
        Shipment? foundShipment = GetById(id);
        if (foundShipment == null) return null;

        _db.Shipments.Remove(foundShipment);
        SaveToDBOrFail();
        return foundShipment;
    }

    protected override void ValidateModel(Shipment model) => _shipmentValidator.ValidateAndThrow(model);
}