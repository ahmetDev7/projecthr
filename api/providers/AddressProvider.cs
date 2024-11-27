public class AddressProvider : ICRUD<Address>
{
    private readonly AppDbContext _db;

    public AddressProvider(AppDbContext db)
    {
        _db = db;
    }

    public Address? Create<IDTO>(IDTO newElement)
    {
        AddressDTO? request = newElement as AddressDTO;
        if(request == null) throw new Exception("Request invalid");

        Address newAddress = new(){
            Street=request.Street,
            HouseNumber=request.HouseNumber,
            HouseNumberExtension=request.HouseNumberExtension,
            HouseNumberExtensionExtra=request.HouseNumberExtensionExtra,
            ZipCode=request.ZipCode,
            City=request.City,
            Province=request.Province,
            CountryCode=request.CountryCode,
            CreatedAt=DateTime.UtcNow
        };

        _db.Addresses.Add(newAddress);

        DBUtil.SaveChanges(_db, "Address not stored");


        return newAddress;

    }

    public Address Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<Address> GetAll()
    {
        throw new NotImplementedException();
    }
    

    public Address? GetById(Guid id)
    {
        return _db.Addresses.FirstOrDefault(a => a.Id == id);
    }

    public Address? Update<IDTO>(Guid id, IDTO dto)
    {
        throw new NotImplementedException();
    }
    
    public Address? GetOrCreateAddress(AddressDTO? addressDTO = null, Guid? addressId = null)  
    {  
        if (addressDTO == null && addressId == null) return null;  

        if (addressId != null) return GetById(addressId.Value);  

        if(addressDTO != null) return Create(addressDTO);  

        return null;  
    } 
}