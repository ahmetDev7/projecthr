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

    protected override void ValidateModel(ItemLine model) => _itemLineValidator.ValidateAndThrow(model);
}