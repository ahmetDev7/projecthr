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

    public override ItemType? Update(Guid id, BaseDTO updatedValues)
    {
        ItemTypeRequest? req = updatedValues as ItemTypeRequest;
        if (req == null) throw new ApiFlowException("Could not process update item type request. Update new item group failed.");

        ItemType? foundItemType = _db.ItemTypes.FirstOrDefault(it => it.Id == id);
        if (foundItemType == null) return null;

        foundItemType.Name = req.Name;
        foundItemType.Description = req.Description;
        foundItemType.SetUpdatedAt();

        ValidateModel(foundItemType);
        _db.ItemTypes.Update(foundItemType);
        SaveToDBOrFail();

        return foundItemType;
    }

    protected override void ValidateModel(ItemType model) => _itemTypeValidator.ValidateAndThrow(model);
}