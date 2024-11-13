
using DTOs;

public class ItemsProvider : ICRUD<Item>
{
    private readonly AppDbContext _db;
    public ItemsProvider(AppDbContext db)
    {
        _db = db;
    }

    public Item? Create<IDTO>(IDTO dto)
    {
        ItemDTO? req = dto as ItemDTO;
        if (req == null) throw new ApiFlowException("Could not process create item request. Save new item failed.");

        Item newItem = new()
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
        };

        _db.Items.Add(newItem);


        DBUtil.SaveChanges(_db, "Item not stored");        

        return newItem;
    }

    public Item? Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<Item>? GetAll() => _db.Items.ToList();


    public Item? GetById(Guid id) => _db.Items.FirstOrDefault(i => i.Id == id);

    public IDTO? GetByIdAsDTO(Guid id)
    {
        throw new NotImplementedException();
    }

    public Item? Update<IDTO>(Guid id, IDTO dto)
    {
        throw new NotImplementedException();
    }

    List<IDTO>? ICRUD<Item>.GetAll()
    {
        throw new NotImplementedException();
    }
}