using DTO.ItemGroup;
using FluentValidation;
using Model;

public class ItemGroupProvider : BaseProvider<ItemGroup>
{
    private IValidator<ItemGroup> _itemGroupValidator;

    public ItemGroupProvider(AppDbContext db, IValidator<ItemGroup> validator) : base(db) {
        _itemGroupValidator = validator;
     }

    public override ItemGroup? GetById(Guid id) => _db.ItemGroups.FirstOrDefault(ig => ig.Id == id);

    public override List<ItemGroup>? GetAll() => _db.ItemGroups.ToList();

    public override ItemGroup? Create(BaseDTO createValues)
    {
        ItemGroupRequest? req = createValues as ItemGroupRequest;
        if (req == null) throw new ApiFlowException("Could not process create item group request. Save new item group failed.");

        ItemGroup newItemGroup = new ItemGroup(newInstance:true)
        {
            Name = req.Name,
            Description = req.Description
        };
        
        ValidateModel(newItemGroup);
        _db.ItemGroups.Add(newItemGroup);
        SaveToDBOrFail();
        return newItemGroup;
    }

    protected override void ValidateModel(ItemGroup model) => _itemGroupValidator.ValidateAndThrow(model);
}