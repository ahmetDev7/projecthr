using DTO.Item;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class ItemsProvider : BaseProvider<Item>
{
    private readonly IValidator<Item> _itemValidator;

    public ItemsProvider(AppDbContext db, IValidator<Item> validator) : base(db)
    {
        _itemValidator = validator;
    }

    private IQueryable<Item> GetItemByIdQuery(bool includeInventory = false)
    {
        IQueryable<Item> query = _db.Items.AsQueryable();

        if (includeInventory) query = query.Include(i => i.Inventory);

        return query;
    }

    public override Item? GetById(Guid id) => GetItemByIdQuery().FirstOrDefault(i => i.Id == id);

    public Item? GetById(Guid id, bool includeInventory = false) => GetItemByIdQuery(includeInventory).FirstOrDefault(i => i.Id == id);



    public override List<Item>? GetAll() => _db.Items.ToList();

    public override Item? Create(BaseDTO createValues)
    {
        ItemRequest? req = createValues as ItemRequest;
        if (req == null) throw new ApiFlowException("Could not process create item request. Save new item failed.");

        Item newItem = new(newInstance: true)
        {
            Code = req.Code,
            Description = req.Description,
            ShortDescription = req.ShortDescription,
            UpcCode = req.UpcCode,
            ModelNumber = req.ModelNumber,
            CommodityCode = req.CommodityCode,
            UnitPurchaseQuantity = req.UnitPurchaseQuantity,
            UnitOrderQuantity = req.UnitOrderQuantity,
            PackOrderQuantity = req.PackOrderQuantity,
            SupplierReferenceCode = req.SupplierReferenceCode,
            SupplierPartNumber = req.SupplierPartNumber,
            ItemGroupId = req.ItemGroupId,
            ItemLineId = req.ItemLineId,
            ItemTypeId = req.ItemTypeId,
            SupplierId = req.SupplierId,
            CreatedBy = req.CreatedBy
        };

        ValidateModel(newItem);
        _db.Items.Add(newItem);
        SaveToDBOrFail();
        return newItem;
    }

    public override Item? Update(Guid id, BaseDTO updateValues)
    {
        ItemRequest? req = updateValues as ItemRequest;
        if (req == null) throw new ApiFlowException("Could not process update item request. Update failed.");

        Item? existingItem = _db.Items.FirstOrDefault(i => i.Id == id);
        if (existingItem == null) throw new ApiFlowException($"Item not found for id '{id}'");

        existingItem.Code = req.Code;
        existingItem.Description = req.Description;
        existingItem.ShortDescription = req.ShortDescription;
        existingItem.UpcCode = req.UpcCode;
        existingItem.ModelNumber = req.ModelNumber;
        existingItem.CommodityCode = req.CommodityCode;
        existingItem.UnitPurchaseQuantity = req.UnitPurchaseQuantity;
        existingItem.UnitOrderQuantity = req.UnitOrderQuantity;
        existingItem.PackOrderQuantity = req.PackOrderQuantity;
        existingItem.SupplierReferenceCode = req.SupplierReferenceCode;
        existingItem.SupplierPartNumber = req.SupplierPartNumber;
        existingItem.ItemGroupId = req.ItemGroupId;
        existingItem.ItemLineId = req.ItemLineId;
        existingItem.ItemTypeId = req.ItemTypeId;
        existingItem.SupplierId = req.SupplierId;
        existingItem.SetUpdatedAt();

        ValidateModel(existingItem);

        _db.Items.Update(existingItem);
        SaveToDBOrFail();

        return existingItem;
    }

    public override Item? Delete(Guid id)
    {
        if (
            _db.OrderItems.Any(row => row.ItemId == id)
            || _db.ShipmentItems.Any(row => row.ItemId == id)
            || _db.DockItems.Any(row => row.ItemId == id)
            || _db.TransferItems.Any(row => row.ItemId == id)
            || _db.Inventories.Any(row => row.ItemId == id)
        )
        {
            throw new ApiFlowException("This item is related to one of the following relations: Order, Shipment, Dock, Transfer, or Inventory. Please unlink any relation before deleting the item.", StatusCodes.Status409Conflict);
        }

        Item? foundItem = GetById(id);
        if (foundItem == null) throw new ApiFlowException("Item not found", StatusCodes.Status404NotFound);


        _db.Items.Remove(foundItem);
        SaveToDBOrFail();
        return foundItem;
    }


    public Inventory? GetInventory(Guid itemId) => _db.Inventories.FirstOrDefault(i => i.ItemId == itemId);

    protected override void ValidateModel(Item model) => _itemValidator.ValidateAndThrow(model);
}