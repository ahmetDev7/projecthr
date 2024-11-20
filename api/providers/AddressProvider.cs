using DTO.Supplier;

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

    public Address? GetOrCreateAddress(SupplierRequest request)
    {

        if (request.Address_id != null)
        {
            Address? existingAddress = GetById(request.Address_id.Value);
            if (existingAddress == null) throw new ApiFlowException("address_id does not exist");
            return existingAddress;
        }
        Address address = Create<AddressDTO>(request.Address);

        if (request.Address != null)
        {
            return address;
        }

        throw new ApiFlowException("Both address_id and address data are missing. Unable to create supplier address.");
    }
}