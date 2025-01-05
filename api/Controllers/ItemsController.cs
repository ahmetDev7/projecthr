using Microsoft.AspNetCore.Mvc;
using DTO.Item;

[Route("api/[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly ItemsProvider _itemsProvider;
    private readonly InventoriesProvider _inventoriesProvider;

    public ItemsController(ItemsProvider itemsProvider, InventoriesProvider inventoriesProvider, LocationsProvider locationsProvider)
    {
        _itemsProvider = itemsProvider;
        _inventoriesProvider = inventoriesProvider;
    }

    /*
        Create
        Update
        UpdatePartial
        Delete
        ShowSingle
        ShowAll
    */

    [HttpPost()]
    public IActionResult Create([FromBody] ItemRequest req)
    {
        Item? newItem = _itemsProvider.Create(req);
        if (newItem == null) throw new ApiFlowException("Saving new Item failed.");

        return Ok(new
        {
            message = "Item created!",
            new_item = new ItemResponse
            {
                Id = newItem.Id,
                Code = newItem.Code,
                Description = newItem.Description,
                ShortDescription = newItem.ShortDescription,
                UpcCode = newItem.UpcCode,
                ModelNumber = newItem.ModelNumber,
                CommodityCode = newItem.CommodityCode,
                UnitPurchaseQuantity = newItem.UnitPurchaseQuantity,
                UnitOrderQuantity = newItem.UnitOrderQuantity,
                PackOrderQuantity = newItem.PackOrderQuantity,
                SupplierReferenceCode = newItem.SupplierReferenceCode,
                SupplierPartNumber = newItem.SupplierPartNumber,
                ItemGroupId = newItem.ItemGroupId,
                ItemLineId = newItem.ItemLineId,
                ItemTypeId = newItem.ItemTypeId,
                SupplierId = newItem.SupplierId,
                CreatedAt = newItem.CreatedAt,
                UpdatedAt = newItem.UpdatedAt
            }
        });
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] ItemRequest req)
    {
        Item? updatedItem = _itemsProvider.Update(id, req);

        return updatedItem == null
            ? NotFound(new { message = $"Item not found for id '{id}'" })
            : Ok(new
            {
                message = "Item updated!",
                updated_item = new ItemResponse
                {
                    Id = updatedItem.Id,
                    Code = updatedItem.Code,
                    Description = updatedItem.Description,
                    ShortDescription = updatedItem.ShortDescription,
                    UpcCode = updatedItem.UpcCode,
                    ModelNumber = updatedItem.ModelNumber,
                    CommodityCode = updatedItem.CommodityCode,
                    UnitPurchaseQuantity = updatedItem.UnitPurchaseQuantity,
                    UnitOrderQuantity = updatedItem.UnitOrderQuantity,
                    PackOrderQuantity = updatedItem.PackOrderQuantity,
                    SupplierReferenceCode = updatedItem.SupplierReferenceCode,
                    SupplierPartNumber = updatedItem.SupplierPartNumber,
                    ItemGroupId = updatedItem.ItemGroupId,
                    ItemLineId = updatedItem.ItemLineId,
                    ItemTypeId = updatedItem.ItemTypeId,
                    SupplierId = updatedItem.SupplierId,
                    CreatedAt = updatedItem.CreatedAt,
                    UpdatedAt = updatedItem.UpdatedAt
                }
            });
    }

    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        Item? foundItem = _itemsProvider.GetById(id);

        return (foundItem == null)
            ? NotFound(new { message = $"Item not found for id '{id}'" })
            : Ok(new ItemResponse
            {
                Id = foundItem.Id,
                Code = foundItem.Code,
                Description = foundItem.Description,
                ShortDescription = foundItem.ShortDescription,
                UpcCode = foundItem.UpcCode,
                ModelNumber = foundItem.ModelNumber,
                CommodityCode = foundItem.CommodityCode,
                UnitPurchaseQuantity = foundItem.UnitPurchaseQuantity,
                UnitOrderQuantity = foundItem.UnitOrderQuantity,
                PackOrderQuantity = foundItem.PackOrderQuantity,
                SupplierReferenceCode = foundItem.SupplierReferenceCode,
                SupplierPartNumber = foundItem.SupplierPartNumber,
                ItemGroupId = foundItem.ItemGroupId,
                ItemLineId = foundItem.ItemLineId,
                ItemTypeId = foundItem.ItemTypeId,
                SupplierId = foundItem.SupplierId,
                CreatedAt = foundItem.CreatedAt,
                UpdatedAt = foundItem.UpdatedAt
            });
    }

    [HttpGet()]
    public IActionResult ShowAll() => Ok(_itemsProvider.GetAll()?.Select(i => new ItemResponse
    {
        Id = i.Id,
        Code = i.Code,
        Description = i.Description,
        ShortDescription = i.ShortDescription,
        UpcCode = i.UpcCode,
        ModelNumber = i.ModelNumber,
        CommodityCode = i.CommodityCode,
        UnitPurchaseQuantity = i.UnitPurchaseQuantity,
        UnitOrderQuantity = i.UnitOrderQuantity,
        PackOrderQuantity = i.PackOrderQuantity,
        SupplierReferenceCode = i.SupplierReferenceCode,
        SupplierPartNumber = i.SupplierPartNumber,
        ItemGroupId = i.ItemGroupId,
        ItemLineId = i.ItemLineId,
        ItemTypeId = i.ItemTypeId,
        SupplierId = i.SupplierId,
        CreatedAt = i.CreatedAt,
        UpdatedAt = i.UpdatedAt
    }).ToList());

    [HttpGet("{id}/inventories")]
    public IActionResult GetInventories(Guid id)
    {
        Inventory? foundInventory = _itemsProvider.GetInventory(id);
        if (foundInventory == null)
        {
            return NotFound(new { message = "The specified item is not currently associated with any inventory." });
        }

        return Ok(new InventoryResponse()
        {
            Id = foundInventory.Id,
            Description = foundInventory.Description,
            ItemReference = foundInventory.ItemReference,
            ItemId = foundInventory.ItemId,
            Locations = _inventoriesProvider.GetInventoryLocations(foundInventory.Id).Select(i => new InventoryLocationResponse
            {
                WarehouseId = i.Location.WarehouseId,
                LocationId = i.LocationId,
                OnHand = i.OnHandAmount
            }).ToList(),
            TotalOnHand = foundInventory.TotalOnHand,
            TotalExpected = foundInventory.TotalExpected,
            TotalOrdered = foundInventory.TotalOrderd,
            TotalAllocated = foundInventory.TotalAllocated,
            TotalAvailable = foundInventory.TotalAvailable,
            CreatedAt = foundInventory.CreatedAt,
            UpdatedAt = foundInventory.UpdatedAt,
        });
    }

    [HttpGet("{id}/inventories/totals")]
    public IActionResult GetInventoriesTotals(Guid id)
    {
        Inventory? foundInventory = _itemsProvider.GetInventory(id);
        if (foundInventory == null)
        {
            return NotFound(new { message = "The specified item is not currently associated with any inventory." });
        }

        return Ok(
            new
            {
                inventory_id = foundInventory.Id,
                total_on_hand = foundInventory.TotalOnHand,
                total_expected = foundInventory.TotalExpected,
                total_orderd = foundInventory.TotalOrderd,
                total_allocated = foundInventory.TotalAllocated,
                total_available = foundInventory.TotalAvailable,
            }
        );
    }
}