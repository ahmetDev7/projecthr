using DTO.Supplier;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class SupplierProvider : BaseProvider<Supplier>
{
    private ContactProvider _contactProvider;
    private AddressProvider _addressProvider;
    private IValidator<Supplier> _supplierValidator;

    public SupplierProvider(AppDbContext db, IValidator<Supplier> validator, ContactProvider contactProvider, AddressProvider addressProvider) : base(db)
    {
        _supplierValidator = validator;
        _addressProvider = addressProvider;
        _contactProvider = contactProvider;
    }

    public override Supplier? Update(Guid id, BaseDTO updatedValues)
    {
        SupplierRequest? req = updatedValues as SupplierRequest;
        if (req == null) throw new ApiFlowException("Could not process update supplier request. Update new supplier failed.");

        Supplier? foundSupplier = GetById(id);
        if (foundSupplier == null) throw new ApiFlowException("No Supplier found");

        if(req.Name != null) foundSupplier.Name = req.Name;
        if(req.Code != null) foundSupplier.Code = req.Code;
        if(req.Reference != null) foundSupplier.Reference = req.Reference;
        if(req.Contact != null)
        {
            foundSupplier.Contact.Name = req.Contact.Name;
            foundSupplier.Contact.Email = req.Contact.Email;
            foundSupplier.Contact.Phone = req.Contact.Phone;
        }
        if(req.Address != null)
        {
            foundSupplier.Address.Street = req.Address.Street;
            foundSupplier.Address.HouseNumber = req.Address.HouseNumber;
            foundSupplier.Address.HouseNumberExtension = req.Address.HouseNumberExtension;
            foundSupplier.Address.HouseNumberExtensionExtra = req.Address.HouseNumberExtensionExtra;
            foundSupplier.Address.ZipCode = req.Address.ZipCode;
            foundSupplier.Address.City = req.Address.City;
            foundSupplier.Address.Province = req.Address.Province;
            foundSupplier.Address.CountryCode = req.Address.CountryCode;
        }
        foundSupplier.CreatedAt = foundSupplier.CreatedAt;
        foundSupplier.SetUpdatedAt();
        
        SaveToDBOrFail();

        return foundSupplier;
    }

    public override Supplier? GetById(Guid id) => _db.Suppliers.Include(c => c.Contact).Include(a => a.Address).FirstOrDefault(i => i.Id == id);
    protected override void ValidateModel(Supplier model) => _supplierValidator.ValidateAndThrow(model);
}