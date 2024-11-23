using DTO.ItemGroup;
using FluentValidation;

public class ItemGroupProvider : BaseProvider<ItemGroup>
{
    private IValidator<ItemGroup> _itemGroupValidator;

    public ItemGroupProvider(AppDbContext db, IValidator<ItemGroup> validator) : base(db)
    {
        _itemGroupValidator = validator;
    }

    public override ItemGroup? GetById(Guid id) => _db.ItemGroups.FirstOrDefault(ig => ig.Id == id);

    public override List<ItemGroup>? GetAll() => _db.ItemGroups.ToList();

    public override ItemGroup? Create(BaseDTO createValues)
    {
        ItemGroupRequest? req = createValues as ItemGroupRequest;
        if (req == null) throw new ApiFlowException("Could not process create item group request. Save new item group failed.");

        ItemGroup newItemGroup = new ItemGroup(newInstance: true)
        {
            Name = req.Name,
            Description = req.Description
        };

        ValidateModel(newItemGroup);
        _db.ItemGroups.Add(newItemGroup);
        SaveToDBOrFail();
        return newItemGroup;
    }

    public override ItemGroup? Update(Guid id, BaseDTO updatedValues)
    {
        bool hasChanges = false;
        ItemGroupRequest? req = updatedValues as ItemGroupRequest;
        if (req == null) throw new ApiFlowException("Could not process update item group request. Update new item group failed.");

        ItemGroup? foundItemGroup = _db.ItemGroups.FirstOrDefault(ig => ig.Id == id);
        if (foundItemGroup == null) return null;

        if (!string.IsNullOrEmpty(req.Name) && req.Name != foundItemGroup.Name)
        {
            foundItemGroup.Name = req.Name;
            hasChanges = true;
        }

        if (req.Description != foundItemGroup.Description)
        {
            foundItemGroup.Description = req.Description;
            hasChanges = true;
        }

        if (hasChanges) foundItemGroup.SetUpdatedAt();

        SaveToDBOrFail();

        return foundItemGroup;
    }

    public List<Item> GetRelatedItemsById(Guid itemGroupId) => _db.Items.Where(i => i.ItemGroupId == itemGroupId).ToList();

    public override ItemGroup? Delete(Guid id)
    {
        ItemGroup? foundItemGroup = _db.ItemGroups.FirstOrDefault(i => i.Id == id);
        if(foundItemGroup == null) return null;

        if(_db.Items.Any(i => i.ItemGroupId == id)) throw new ApiFlowException("The item group has associated items. Please remove these associations before deleting the group.");
        
        _db.ItemGroups.Remove(foundItemGroup);
        SaveToDBOrFail();
        return foundItemGroup;
    }

    protected override void ValidateModel(ItemGroup model) => _itemGroupValidator.ValidateAndThrow(model);
}