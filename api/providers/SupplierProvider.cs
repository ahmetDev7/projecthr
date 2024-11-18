using System.Security.Permissions;
using DTO.Supplier;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
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

    public override Supplier? Create(BaseDTO createValues)
    {
        SupplierReQuestDTO? req = createValues as SupplierReQuestDTO;

        if (req == null)
            throw new ApiFlowException("Could not process create supplier request. Save new supplier failed.");
        if (req.Name == null || req.Code == null || req.Reference == null)
            throw new ApiFlowException("Name,Code and Reference are required fields.");

        var relatedContact = GetOrCreateContact(req);
        var relatedAddress = GetOrCreateAddress(req);

        Supplier newSupplier = new(newInstance:true)
        {
            Code = req.Code,
            Name = req.Name,
            Reference = req.Reference,
            Contact = relatedContact,
            Address = relatedAddress

        };

        // Validate the new supplier model (uncomment if needed)
        ValidateModel(newSupplier);

        // Add to the database and save
        _db.Suppliers.Add(newSupplier);
        SaveToDBOrFail();

        return newSupplier;
    }


    public override Supplier? GetById(Guid id) => _db.Suppliers.FirstOrDefault(i => i.Id == id);
    protected override void ValidateModel(Supplier model) => _supplierValidator.ValidateAndThrow(model);

    private Contact? GetOrCreateContact(SupplierReQuestDTO request)
    {
        // Check if Contact_id is not Guid.Empty and fetch the contact if it exists
        if (request.Contact_id != Guid.Empty)
        {
            var existingContact = _contactProvider.GetById(request.Contact_id);
            if (existingContact != null)
                return existingContact;

            throw new ApiFlowException("contact_id does not exist");
        }

        // If Contact_id is not provided, check if a new Contact DTO is available
        if (request.Contact != null)
        {
            return _contactProvider.Create<ContactDTO>(request.Contact);
        }

        throw new ApiFlowException("Both contact_id and contact data are missing. Unable to create supplier contact.");
    }

    private Address? GetOrCreateAddress(SupplierReQuestDTO request)
    {
        // Check if Address_id is not Guid.Empty and fetch the address if it exists
        if (request.Address_id != Guid.Empty)
        {
            var existingAddress = _addressProvider.GetById(request.Address_id);
            if (existingAddress != null)
                return existingAddress;

            throw new ApiFlowException("address_id does not exist");
        }

        // If Address_id is not provided, check if a new Address DTO is available
        if (request.Address != null)
        {
            return _addressProvider.Create<AddressDTO>(request.Address);
        }

        throw new ApiFlowException("Both address_id and address data are missing. Unable to create supplier address.");
    }


}