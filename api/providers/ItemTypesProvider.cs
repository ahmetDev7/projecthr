using DTO.ItemType;
using FluentValidation;

public class ItemTypesProvider : BaseProvider<ItemType>
{
    private IValidator<ItemType> _itemTypeValidator;

    public ItemTypesProvider(AppDbContext db, IValidator<ItemType> validator) : base(db)
    {
        _itemTypeValidator = validator;
    }

    public override ItemType? Create(BaseDTO createValues)
    {
        ItemTypeRequest? req = createValues as ItemTypeRequest;
        if (req == null) throw new ApiFlowException("Could not process create item type request. Save new item type failed.");

        ItemType newItemType = new ItemType(newInstance: true)
        {
            Name = req.Name,
            Description = req.Description
        };

        ValidateModel(newItemType);
        _db.ItemTypes.Add(newItemType);
        SaveToDBOrFail();
        return newItemType;
    }

    protected override void ValidateModel(ItemType model) => _itemTypeValidator.ValidateAndThrow(model);
}