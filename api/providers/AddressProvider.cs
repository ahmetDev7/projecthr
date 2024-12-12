using DTO.Address;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class AddressProvider : BaseProvider<Address>
{
    private readonly IValidator<Address> _addressValidator;

    public AddressProvider(AppDbContext db, IValidator<Address> validator) : base(db)
    {
        _addressValidator = validator;
    }

    public override List<Address> GetAll() => _db.Addresses.ToList();

    public override Address? GetById(Guid id) =>
        _db.Addresses.FirstOrDefault(a => a.Id == id);

    public override Address? Create(BaseDTO createValues)
    {
        AddressRequest? req = createValues as AddressRequest;
        if (req == null) throw new ApiFlowException("Invalid address request. Could not create address.");

        Address newAddress = new Address(newInstance: true)
        {
            Street = req.Street,
            HouseNumber = req.HouseNumber,
            HouseNumberExtension = req.HouseNumberExtension,
            HouseNumberExtensionExtra = req.HouseNumberExtensionExtra,
            ZipCode = req.ZipCode,
            City = req.City,
            Province = req.Province,
            CountryCode = req.CountryCode,
        };

        ValidateModel(newAddress);

        _db.Addresses.Add(newAddress);
        SaveToDBOrFail();

        return newAddress;
    }

    public override Address? Delete(Guid id)
    {
        Address? foundAddress = _db.Addresses.FirstOrDefault(a => a.Id == id);
        if (foundAddress == null) return null;

        _db.Addresses.Remove(foundAddress);
        SaveToDBOrFail();

        return foundAddress;
    }

    public override Address? Update(Guid id, BaseDTO updateValues)
    {
        AddressRequest? req = updateValues as AddressRequest;
        if (req == null) throw new ApiFlowException("Invalid address request. Could not update address.");

        Address? existingAddress = _db.Addresses.FirstOrDefault(a => a.Id == id);
        if (existingAddress == null) throw new ApiFlowException($"Address not found for id '{id}'");

        existingAddress.Street = req.Street;
        existingAddress.HouseNumber = req.HouseNumber;
        existingAddress.HouseNumberExtension = req.HouseNumberExtension;
        existingAddress.HouseNumberExtensionExtra = req.HouseNumberExtensionExtra;
        existingAddress.ZipCode = req.ZipCode;
        existingAddress.City = req.City;
        existingAddress.Province = req.Province;
        existingAddress.CountryCode = req.CountryCode;
        existingAddress.SetUpdatedAt();

        ValidateModel(existingAddress);

        _db.Addresses.Update(existingAddress);
        SaveToDBOrFail();

        return existingAddress;
    }

    protected override void ValidateModel(Address model) => _addressValidator.ValidateAndThrow(model);

    public Address? GetOrCreateAddress(AddressRequest? address = null, Guid? addressId = null)
    {
        if (address == null && addressId == null) return null;

        if (addressId != null) return GetById(addressId.Value);

        if (address != null) return Create(address);

        return null;
    }
}
