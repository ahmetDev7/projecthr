
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
        if(req == null) throw new ApiFlowException("Could not process create item request. Save new item failed.");

        Item newItem = new(){
            Code=req.Code,
            Description=req.Description,
            ShortDescription=req.ShortDescription,
            UpcCode=req.UpcCode,
            ModelNumber=req.ModelNumber,
            CommodityCode=req.CommodityCode,
            UnitPurchaseQuantity=req.UnitPurchaseQuantity,
            UnitOrderQuantity=req.UnitOrderQuantity,
            PackOrderQuantity=req.PackOrderQuantity,
            SupplierReferenceCode=req.SupplierReferenceCode,
            SupplierPartNumber=req.SupplierPartNumber,
        };

        _db.Items.Add(newItem);
        
        if(!DBUtil.IsSaved(_db.SaveChanges())) throw new ApiFlowException("Item not stored.");

        return newItem;
    }

    public Item? Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<Item>? GetAll()
    {
        throw new NotImplementedException();
    }

    public Item? GetById(Guid id) => _db.Items.FirstOrDefault(i => i.Id == id);

    public Item? Update(Guid id)
    {
        throw new NotImplementedException();
    }
}