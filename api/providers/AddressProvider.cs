
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
        if (request == null) throw new Exception("Request invalid");

        Address newAddress = new()
        {
            Street = request.Street,
            HouseNumber = request.HouseNumber,
            HouseNumberExtension = request.HouseNumberExtension,
            HouseNumberExtensionExtra = request.HouseNumberExtensionExtra,
            ZipCode = request.ZipCode,
            City = request.City,
            Province = request.Province,
            CountryCode = request.CountryCode,
        };

        _db.Addresses.Add(newAddress);

        DBUtil.SaveChanges(_db, "Address not stored");


        return newAddress;

    }

    public Address Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<IDTO>? GetAll()
    {
        throw new NotImplementedException();
    }


    public Address? GetById(Guid id)
    {
        return _db.Addresses.FirstOrDefault(a => a.Id == id);
    }

    public IDTO? GetByIdAsDTO(Guid id)
    {
        throw new NotImplementedException();
    }

    public Address? Update<IDTO>(Guid id, IDTO dto)
    {
        throw new NotImplementedException();
    }

    public virtual Address? GetOrCreateAddress(SupplierReQuest request)
    {
        if (request == null)
        {
            throw new ApiFlowException("Request object is null.");
        }

        if (request.AddressId != null)
        {
            Console.WriteLine($"ContactId: {request.AddressId}");
            Address? existingContact = GetById(request.AddressId.Value);
            if (existingContact == null) throw new ApiFlowException("contact_id does not exist");
            return existingContact;
        }
        Address address = Create<AddressDTO>(request.Address);

        if (request.Address != null)
        {
            Console.WriteLine($"Creating new contact with data: {request.Address.Street}");
            return address;
        }

        throw new ApiFlowException("Both contact_id and contact data are missing. Unable to create supplier contact.");
    }
}