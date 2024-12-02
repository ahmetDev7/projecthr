using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class InventoriesProvider : BaseProvider<Inventory>
{
    private readonly LocationsProvider _locationsProvider;
    private IValidator<Inventory> _inventoryValidator;
    private IValidator<InventoryRequest> _inventoryRequestValidator;


    public InventoriesProvider(AppDbContext db, LocationsProvider locationsProvider, IValidator<Inventory> inventoryValidator, IValidator<InventoryRequest> inventoryRequestValidator) : base(db)
    {
        _locationsProvider = locationsProvider;
        _inventoryValidator = inventoryValidator;
        _inventoryRequestValidator = inventoryRequestValidator;
    }

    public override Inventory? GetById(Guid id) => _db.Inventories.Include(i => i.Item).FirstOrDefault(i => i.Id == id);

    public override List<Inventory>? GetAll() => _db.Inventories.Include(i => i.Item).ToList();

    public override Inventory? Create(BaseDTO createValues)
    {
        InventoryRequest? req = createValues as InventoryRequest;
        if (req == null) throw new ApiFlowException("Invalid inventory request. Could not create inventory.");

        _inventoryRequestValidator.ValidateAndThrow(req);

        Inventory newInventory = new(newInstance: true)
        {
            Description = req.Description,
            ItemReference = req.ItemReference,
            ItemId = req.ItemId,
        };

        ValidateModel(newInventory);
        _db.Inventories.Add(newInventory);

        _locationsProvider.FillOnHandAmount(newInventory.Id, req.Locations, false);
        SaveToDBOrFail();

        return newInventory;
    }

    public override Inventory? Delete(Guid id){
        Inventory? foundInventory = GetById(id);
        if (foundInventory == null) return null;

        _db.Inventories.Remove(foundInventory);
        SaveToDBOrFail();
        return foundInventory;
    }

    public Inventory? GetInventoryByItemId(Guid? itemId) => _db.Inventories.Where(i => i.ItemId == itemId).FirstOrDefault();

    /*
        total_on_hand int //calculated value from location_inventory
        total_expected int //calculated value 
        total_ordered int //calculated value op basis van order
        total_allocated int //calculated value
        total_available int //calculated value
    */

    public int CalculateTotalOnHand(Guid inventoryId) => _db.Locations.Where(location => location.InventoryId == inventoryId).Sum(location => location.OnHand ?? 0);

    public int CalculateTotalExpected()
    {
        return 0; // TODO
    }

    public int CalculateTotalOrdered()
    {
        return 0; // TODO
    }

    public int CalculateTotalAllocated()
    {
        return 0; // TODO
    }

    public int CalculateTotalAvailable()
    {
        return 0; // TODO
    }

    public Dictionary<string, int> GetCalculatedValues(Guid inventoryId) => new Dictionary<string, int>()
        {
            {"TotalOnHand", CalculateTotalOnHand(inventoryId)},
            {"TotalExpected", CalculateTotalExpected()},
            {"TotalOrdered", CalculateTotalOrdered()},
            {"TotalAllocated", CalculateTotalAllocated()},
            {"TotalAvailable", CalculateTotalAvailable()},
        };



    protected override void ValidateModel(Inventory model) => _inventoryValidator.ValidateAndThrow(model);
}