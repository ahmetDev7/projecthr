
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

        if(_db.SaveChanges() < 1){
            throw new Exception("An error occurred while saving the address");
        }


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

    public Address Update(Guid id)
    {
        throw new NotImplementedException();
    }
}