using DTO.Supplier;
using FluentValidation;

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

    public override Supplier? Create(BaseDTO createValues)
    {
        SupplierRequest? req = createValues as SupplierRequest;

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
        

        ValidateModel(newSupplier);
        _db.Suppliers.Add(newSupplier);
        SaveToDBOrFail();

        newSupplier.Contact =relatedContact;
        newSupplier.Address =relatedAddress;

        return newSupplier;
    }
    protected override void ValidateModel(Supplier model) => _supplierValidator.ValidateAndThrow(model);

}