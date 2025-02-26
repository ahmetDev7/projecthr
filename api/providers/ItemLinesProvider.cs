using DTO.ItemLine;
using FluentValidation;

public class ItemLinesProvider : BaseProvider<ItemLine>
{
    private IValidator<ItemLine> _itemLineValidator;

    public ItemLinesProvider(AppDbContext db, IValidator<ItemLine> validator) : base(db)
    {
        _itemLineValidator = validator;
    }


    public override List<ItemLine>? GetAll() => _db.ItemLines.ToList();

    public override ItemLine? GetById(Guid id) => _db.ItemLines.FirstOrDefault(il => il.Id == id);

    public override ItemLine? Create(BaseDTO createValues)
    {
        ItemLineRequest? req = createValues as ItemLineRequest;
        if (req == null) throw new ApiFlowException("Could not process create item line request. Save new item line failed.");

        ItemLine newItemLine = new ItemLine(newInstance: true)
        {
            Name = req.Name,
            Description = req.Description
        };

        ValidateModel(newItemLine);
        _db.ItemLines.Add(newItemLine);
        SaveToDBOrFail();
        return newItemLine;
    }

    public List<Item> GetRelatedItemsById(Guid itemLineId) => _db.Items.Where(i => i.ItemLineId == itemLineId).ToList();

    public override ItemLine? Update(Guid id, BaseDTO updatedValues)
    {
        ItemLineRequest? req = updatedValues as ItemLineRequest;
        if (req == null) throw new ApiFlowException("Could not process update item line request. Update new item group failed.");

        ItemLine? foundItemLine = _db.ItemLines.FirstOrDefault(il => il.Id == id);
        if (foundItemLine == null) return null;

        foundItemLine.Name = req.Name;
        foundItemLine.Description = req.Description;
        foundItemLine.SetUpdatedAt();

        ValidateModel(foundItemLine);
        _db.ItemLines.Update(foundItemLine);
        SaveToDBOrFail();

        return foundItemLine;
    }

    public override ItemLine? Delete(Guid id)
    {
        ItemLine? foundItemLine = _db.ItemLines.FirstOrDefault(il => il.Id == id);
        if (foundItemLine == null) return null;

        if (_db.Items.Any(i => i.ItemLineId == id)) throw new ApiFlowException("The item line has associated items. Please remove these associations before deleting the item line.");

        _db.ItemLines.Remove(foundItemLine);
        SaveToDBOrFail();
        return foundItemLine;
    }

    protected override void ValidateModel(ItemLine model) => _itemLineValidator.ValidateAndThrow(model);
}