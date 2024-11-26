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

        foundSupplier.Name = req.Name;
        foundSupplier.Code = req.Code;
        foundSupplier.Reference = req.Reference;
        
        foundSupplier.Contact.Name = req.Contact.Name;
        foundSupplier.Contact.Email = req.Contact.Email;
        foundSupplier.Contact.Phone = req.Contact.Phone;

        foundSupplier.Address.Street = req.Address.Street;
        foundSupplier.Address.HouseNumber = req.Address.HouseNumber;
        foundSupplier.Address.HouseNumberExtension = req.Address.HouseNumberExtension;
        foundSupplier.Address.HouseNumberExtensionExtra = req.Address.HouseNumberExtensionExtra;
        foundSupplier.Address.ZipCode = req.Address.ZipCode;
        foundSupplier.Address.City = req.Address.City;
        foundSupplier.Address.Province = req.Address.Province;
        foundSupplier.Address.CountryCode = req.Address.CountryCode;    
    
        foundSupplier.CreatedAt = foundSupplier.CreatedAt;
        foundSupplier.SetUpdatedAt();
        
        SaveToDBOrFail();

        return foundSupplier;
    }

    public override Supplier? GetById(Guid id) => _db.Suppliers.Include(c => c.Contact).Include(a => a.Address).FirstOrDefault(i => i.Id == id);
    
    public override Supplier? Create(BaseDTO createValues)
    {
        SupplierRequest? req = createValues as SupplierRequest;
        var allSuppliers = _db.Suppliers.Include(c => c.Contact).Include(a => a.Address).ToList();
        if (req == null) throw new ApiFlowException("Could not process create supplier request. Save new supplier failed.");

        Contact? relatedContact = _contactProvider.GetOrCreateContact(req.Contact, req.ContactId);
        Address? relatedAddress = _addressProvider.GetOrCreateAddress(req.Address, req.AddressId);

        Supplier newSupplier = new Supplier(newInstance: true)
        {
            Code = req.Code,
            Name = req.Name,
            Reference = req.Reference,
        };
        if(relatedContact != null) newSupplier.ContactId = relatedContact.Id;
        if(relatedAddress != null) newSupplier.AddressId = relatedAddress.Id;
        

        ValidateModel(newSupplier);
        _db.Suppliers.Add(newSupplier);
        SaveToDBOrFail();

        newSupplier.Contact =relatedContact;
        newSupplier.Address =relatedAddress;

        return newSupplier;
    }
    protected override void ValidateModel(Supplier model) => _supplierValidator.ValidateAndThrow(model);

}