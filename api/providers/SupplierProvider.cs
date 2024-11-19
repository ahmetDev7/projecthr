using System.Security.Permissions;
using DTO.Supplier;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Model;

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


    public override Supplier? GetById(Guid id) => _db.Suppliers.Include(c => c.Contact).Include(a => a.Address).FirstOrDefault(i => i.Id == id);

    public override List<Supplier> GetAll() => _db.Suppliers.Include(c => c.Contact).Include(a => a.Address).ToList();

    public override Supplier? Create(BaseDTO createValues)
    {
        SupplierReQuestDTO? req = createValues as SupplierReQuestDTO;

        if (req == null) throw new ApiFlowException("Could not process create supplier request. Save new supplier failed.");

        Contact? relatedContact = _contactProvider.GetOrCreateContact(req);
        Address? relatedAddress = _addressProvider.GetOrCreateAddress(req);

        Supplier newSupplier = new Supplier(newInstance: true)
        {
            Code = req.Code,
            Name = req.Name,
            Reference = req.Reference,
            ContactId = relatedContact.Id,
            AddressId = relatedAddress.Id,
        };

        // Validate the new supplier model (uncomment if needed)
        ValidateModel(newSupplier);

        // Add to the database and save
        _db.Suppliers.Add(newSupplier);
        SaveToDBOrFail();

        newSupplier.Contact =relatedContact;
        newSupplier.Address =relatedAddress;

        return newSupplier;
    }

    public override Supplier? Delete(Guid id)
    {
        Supplier? supplier = GetById(id);
        if (supplier == null) return null;

        _db.Suppliers.Remove(supplier);
        SaveToDBOrFail("Failed to delete supplier");
        return supplier;
    }
    protected override void ValidateModel(Supplier model) => _supplierValidator.ValidateAndThrow(model);

}