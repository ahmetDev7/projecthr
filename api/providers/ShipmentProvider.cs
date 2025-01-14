using DTO.Shipment;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Utils.Date;

public class ShipmentProvider : BaseProvider<Shipment>
{
    private IValidator<Shipment> _shipmentValidator;
    private IValidator<ShipmentRequest> _shipmentRequestValidator;
    private IValidator<UpdateShipmentItemDTO> _shipmentItemRequestValidator;

    public ShipmentProvider(AppDbContext db, IValidator<Shipment> validator, IValidator<ShipmentRequest> shipmentRequestValidator, IValidator<UpdateShipmentItemDTO> shipmentItemRequestValidator) : base(db)
    {
        _shipmentValidator = validator;
        _shipmentRequestValidator = shipmentRequestValidator;
        _shipmentItemRequestValidator = shipmentItemRequestValidator;
    }

    public override Shipment? GetById(Guid id) =>
        _db.Shipments.Include(s => s.ShipmentItems).Include(s => s.OrderShipments).FirstOrDefault(s => s.Id == id);

    public override List<Shipment>? GetAll() => _db.Shipments.Include(s => s.ShipmentItems).Include(s => s.OrderShipments).ToList();

    public override Shipment? Create(BaseDTO createValues)
    {
        ShipmentRequest? req = createValues as ShipmentRequest;
        if (req == null) throw new ApiFlowException("Could not process create shipment request. Save new shipment failed.");

        _shipmentRequestValidator.ValidateAndThrow(req);

        using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        try
        {
            Shipment newShipment = new Shipment(newInstance: true)
            {
                ShipmentDate = DateUtil.ToUtcOrNull(req.ShipmentDate),
                OrderDate = DateUtil.ToUtcOrNull(req.OrderDate),
                RequestDate = DateUtil.ToUtcOrNull(req.RequestDate),
                Notes = req.Notes,
                CarrierCode = req.CarrierCode,
                CarrierDescription = req.CarrierDescription,
                ServiceCode = req.ServiceCode,
                TotalPackageCount = req.TotalPackageCount,
                TotalPackageWeight = req.TotalPackageWeight,
                ShipmentStatus = req.ShipmentStatus,
                ShipmentType = req.ShipmentType,
                TransferMode = req.TransferMode,
                PaymentType = req.PaymentType,
                ShipmentItems = req.Items?.Select(item => new ShipmentItem(newInstance: true)
                {
                    ItemId = item.ItemId,
                    Amount = item.Amount
                }).ToList(),
                OrderShipments = req.Orders?.Select(o => new OrderShipment(newInstance: true)
                {
                    OrderId = o
                }).ToList(),

            };

            if (req.OrderDate == null)
            {
                newShipment.OrderDate = DateUtil.ToUtcOrNull(newShipment.CreatedAt);
            }

            if (req.RequestDate == null)
            {
                newShipment.RequestDate = DateUtil.ToUtcOrNull(newShipment.OrderDate);
            }

            if (req.ShipmentStatus == null)
            {
                newShipment.ShipmentStatus = ShipmentStatus.Plan;
            }

            ValidateModel(newShipment);
            _db.Shipments.Add(newShipment);
            SaveToDBOrFail();

            transaction.Commit();
            return newShipment;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }

    }

    public override Shipment? Update(Guid id, BaseDTO updateValues)
    {
        ShipmentRequest? req = updateValues as ShipmentRequest;
        if (req == null) throw new ApiFlowException("Could not process update shipment request. Update failed.");

        _shipmentRequestValidator.ValidateAndThrow(req);

        Shipment? existingShipment = GetById(id);
        if (existingShipment == null) return null;

        using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        try
        {
            existingShipment.OrderDate = DateUtil.ToUtcOrNull(req.OrderDate);
            existingShipment.RequestDate = DateUtil.ToUtcOrNull(req.RequestDate);
            existingShipment.ShipmentDate = DateUtil.ToUtcOrNull(req.ShipmentDate);
            existingShipment.Notes = req.Notes;
            existingShipment.CarrierCode = req.CarrierCode;
            existingShipment.CarrierDescription = req.CarrierDescription;
            existingShipment.ServiceCode = req.ServiceCode;
            existingShipment.TotalPackageCount = req.TotalPackageCount;
            existingShipment.TotalPackageWeight = req.TotalPackageWeight;
            existingShipment.ShipmentStatus = req.ShipmentStatus;
            existingShipment.ShipmentType = req.ShipmentType;
            existingShipment.TransferMode = req.TransferMode;
            existingShipment.PaymentType = req.PaymentType;
            existingShipment.SetUpdatedAt();


            _db.ShipmentItems.RemoveRange(existingShipment.ShipmentItems);
            if (req.Items != null)
            {
                existingShipment.ShipmentItems = req.Items.Select(si => new ShipmentItem(newInstance: true)
                {
                    ItemId = si.ItemId,
                    Amount = si.Amount
                }).ToList();
            }


            _db.OrderShipments.RemoveRange(existingShipment.OrderShipments);
            if (req.Orders != null)
            {
                existingShipment.OrderShipments = req.Orders?.Select(o => new OrderShipment(newInstance: true)
                {
                    OrderId = o
                }).ToList();
            }

            if (req.OrderDate == null)
            {
                existingShipment.OrderDate = DateUtil.ToUtcOrNull(existingShipment.CreatedAt);
            }

            if (req.RequestDate == null)
            {
                existingShipment.RequestDate = DateUtil.ToUtcOrNull(existingShipment.OrderDate);
            }

            if (req.ShipmentStatus == null)
            {
                existingShipment.ShipmentStatus = ShipmentStatus.Plan;
            }

            ValidateModel(existingShipment);
            _db.Shipments.Update(existingShipment);
            SaveToDBOrFail();
            transaction.Commit();
            return existingShipment;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    public override Shipment? Delete(Guid id)
    {
        Shipment? foundShipment = GetById(id);
        if (foundShipment == null) return null;

        _db.Shipments.Remove(foundShipment);
        SaveToDBOrFail();
        return foundShipment;
    }

    public Shipment? CommitShipment(Shipment shipment)
    {
        shipment.ShipmentStatus = ShipmentStatus.Delivered;
        _db.Shipments.Update(shipment);
        SaveToDBOrFail();
        return shipment;
    }

    public List<Order?>? GetOrdersByShipment(Guid shipmentId)
    {
        return _db.OrderShipments
          .Where(os => os.ShipmentId == shipmentId)
          .Include(os => os.Order)
          .ThenInclude(o => o.OrderItems)
          .Select(os => os.Order)
          .ToList();
    }

    public List<Item> GetShipmentItems(Guid shipmentId)
    {
        return _db.Shipments
                  .Where(s => s.Id == shipmentId)
                  .Include(s => s.ShipmentItems)
                  .ThenInclude(si => si.Item)
                  .SelectMany(s => s.ShipmentItems.Select(si => si.Item))
                  .ToList();
    }

    public Shipment? UpdateShipmentItems(Shipment shipment, List<ShipmentItemRR> shipmentItems)
    {
        var reqDTO = new UpdateShipmentItemDTO()
        {
            Items = shipmentItems,
            Orders = shipment?.OrderShipments?.Select(os => os.OrderId).ToList()
        };

        _shipmentItemRequestValidator.ValidateAndThrow(reqDTO);

        using IDbContextTransaction transaction = _db.Database.BeginTransaction();
        try
        {
            _db.ShipmentItems.RemoveRange(shipment.ShipmentItems);
            if (shipmentItems.Count() > 0)
            {
                shipment.ShipmentItems = shipmentItems.Select(si => new ShipmentItem(newInstance: true)
                {
                    ItemId = si.ItemId,
                    Amount = si.Amount
                }).ToList();
            }


            ValidateModel(shipment);
            _db.Shipments.Update(shipment);
            SaveToDBOrFail();
            transaction.Commit();
            return shipment;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    protected override void ValidateModel(Shipment model) => _shipmentValidator.ValidateAndThrow(model);
}