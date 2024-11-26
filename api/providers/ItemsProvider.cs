using DTO.Item;
using FluentValidation;

public class ItemsProvider : BaseProvider<Item>
{
    private readonly IValidator<Item> _itemValidator;

    public ItemsProvider(AppDbContext db, IValidator<Item> validator) : base(db)
    {
        _itemValidator = validator;
    }

    public override Item? GetById(Guid id) => _db.Items.FirstOrDefault(i => i.Id == id);

    public override List<Item>? GetAll() => _db.Items.ToList();

    public override Item? Create(BaseDTO createValues)
    {
        ItemRequest? req = createValues as ItemRequest;
        if (req == null) throw new ApiFlowException("Could not process create item request. Save new item failed.");

        Item newItem = new(newInstance:true)
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
            ItemLineId = req.ItemLineId
        };
        
        ValidateModel(newItem);
        _db.Items.Add(newItem);
        SaveToDBOrFail();
        return newItem;
    }

    protected override void ValidateModel(Item model) => _itemValidator.ValidateAndThrow(model);
}