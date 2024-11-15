using DTOs;
using Models.Location;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;

public class WarehouseProvider : ICRUD<Warehouse>
{
    private readonly AppDbContext _db;
    private readonly AddressProvider _addressProvider;
    private readonly ContactProvider _contactProvider;

    private readonly IValidator<Warehouse> _warehouseValidator;

    public WarehouseProvider(AppDbContext db, AddressProvider addressProvider, ContactProvider contactProvider, IValidator<Warehouse> WarehouseValidator)
    {
        _db = db;
        _addressProvider = addressProvider;
        _contactProvider = contactProvider;
        _warehouseValidator = WarehouseValidator;
    }

    public Warehouse? Create<IDTO>(IDTO newElement)
    {
        var request = newElement as WarehouseDTO ?? throw new ApiFlowException("Could not process create warehouse request. Save new warehouse failed.");

        if (request.ContactId == null && request.Contact == null)
            throw new ApiFlowException("Either contact_id or contact fields must be filled");

        if (request.AddressId == null && request.Address == null)
            throw new ApiFlowException("Either address_id or address fields must be filled");

        var relatedContact = GetOrCreateContact(request);
        var relatedAddress = GetOrCreateAddress(request);

        if (relatedContact == null || relatedAddress == null)
            throw new ApiFlowException("Failed to process address or contact");

        // Create the new Warehouse entry
        var newWarehouse = new Warehouse
        {
            Code = request.Code,
            Name = request.Name,
            ContactId = relatedContact.Id,
            AddressId = relatedAddress.Id
        };

        _db.Warehouses.Add(newWarehouse);
        DBUtil.SaveChanges(_db, "Location not stored");

        return newWarehouse;
    }

    private Contact? GetOrCreateContact(WarehouseDTO request)
    {
        if (request.ContactId != null)
            return _contactProvider.GetById(request.ContactId.Value)
                   ?? throw new ApiFlowException("contact_id does not exist");

        return request.Contact != null
               ? _contactProvider.Create<ContactDTO>(request.Contact)
               : throw new ApiFlowException("An error occurred while saving the warehouse contact");
    }

    private Address? GetOrCreateAddress(WarehouseDTO request)
    {
        if (request.AddressId != null)
            return _addressProvider.GetById(request.AddressId.Value)
                   ?? throw new ApiFlowException("address_id does not exist");

        return request.Address != null
               ? _addressProvider.Create<AddressDTO>(request.Address)
               : throw new ApiFlowException("An error occurred while saving the warehouse address");
    }


    public Warehouse? Delete(Guid id)
    {
        Warehouse? foundWarehouse = GetById(id);
        if (foundWarehouse == null) return null;

        _db.Warehouses.Remove(foundWarehouse);

        DBUtil.SaveChanges(_db, "Warehouse not deleted");

        return foundWarehouse;
    }

    public List<IDTO>? GetAll()
    {
        return _db.Warehouses.Include(w => w.Contact).Include(w => w.Address).Select(w => (IDTO)new WarehouseResultDTO
        {
            Id = w.Id,
            Code = w.Code,
            Name = w.Name,
            Contact = new ContactDTO { Name = w.Contact.Name, Phone = w.Contact.Phone, Email = w.Contact.Email },
            Address = new AddressDTO { Street = w.Address.Street, HouseNumber = w.Address.HouseNumber, HouseNumberExtension = w.Address.HouseNumberExtension, HouseNumberExtensionExtra = w.Address.HouseNumberExtensionExtra, ZipCode = w.Address.ZipCode, City = w.Address.City, Province = w.Address.Province, CountryCode = w.Address.CountryCode }

        }).ToList();
    }

    public Warehouse? GetById(Guid id) => _db.Warehouses.FirstOrDefault(l => l.Id == id);

    public Warehouse? Update<IDTO>(Guid id, IDTO dto)
    {
        var request = dto as WarehouseDTO ?? throw new ApiFlowException("Could not process update warehouse request. Update warehouse failed.");

        Warehouse? foundWarehouseWithContactAndAddress = _db.Warehouses.Include(w => w.Contact).Include(w => w.Address).FirstOrDefault(w => w.Id == id);
        if (foundWarehouseWithContactAndAddress == null) return null;

        Contact? contact = foundWarehouseWithContactAndAddress.Contact;
        Address? address = foundWarehouseWithContactAndAddress.Address;

        if(!string.IsNullOrEmpty(request.Code)) foundWarehouseWithContactAndAddress.Code = request.Code;
        if(!string.IsNullOrEmpty(request.Name)) foundWarehouseWithContactAndAddress.Name = request.Name;
        if(request.Address != null && address != null)
        {
            if(!string.IsNullOrEmpty(request.Address.Street)) address.Street = request.Address.Street;
            if(!string.IsNullOrEmpty(request.Address.HouseNumber)) address.HouseNumber = request.Address.HouseNumber;
            if(!string.IsNullOrEmpty(request.Address.HouseNumberExtension)) address.HouseNumberExtension = request.Address.HouseNumberExtension;
            if(!string.IsNullOrEmpty(request.Address.HouseNumberExtensionExtra)) address.HouseNumberExtensionExtra = request.Address.HouseNumberExtensionExtra;
            if(!string.IsNullOrEmpty(request.Address.ZipCode)) address.ZipCode = request.Address.ZipCode;
            if(!string.IsNullOrEmpty(request.Address.City)) address.City = request.Address.City;
            if(!string.IsNullOrEmpty(request.Address.Province)) address.Province = request.Address.Province;
            if(!string.IsNullOrEmpty(request.Address.CountryCode)) address.CountryCode = request.Address.CountryCode;
        }

        if(request.Contact != null && contact != null){
            if(!string.IsNullOrEmpty(request.Contact.Name)) contact.Name = request.Contact.Name;
            if(!string.IsNullOrEmpty(request.Contact.Phone)) contact.Phone = request.Contact.Phone;
            if(!string.IsNullOrEmpty(request.Contact.Email)) contact.Email = request.Contact.Email;
        }



        validateWarehouse(foundWarehouseWithContactAndAddress);

        DBUtil.SaveChanges(_db, "Warehouse not updated");

        return foundWarehouseWithContactAndAddress;
    }

    public List<Location> GetLocationsByWarehouseId(Guid warehouseId)
    {
        var locationsOfspecificWarehouse = _db.Locations.Where(l => l.WarehouseId == warehouseId).ToList();
        if (locationsOfspecificWarehouse.Count == 0)
            throw new ApiFlowException("No locations found for this warehouse");

        return locationsOfspecificWarehouse;
    }
    private void validateWarehouse(Warehouse warehouse)
    {

        var validationResult = _warehouseValidator.Validate(warehouse);
        if (validationResult.IsValid == false)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }

    public IDTO? GetByIdAsDTO(Guid id)
    {
        Warehouse? warehouse = _db.Warehouses.Include(w => w.Contact).Include(w => w.Address).FirstOrDefault(l => l.Id == id);
        if(warehouse == null) return null;

        return new WarehouseResultDTO{
            Id = warehouse.Id,
            Code = warehouse.Code,
            Name = warehouse.Name,
            Contact = new ContactDTO { Name = warehouse.Contact.Name, Phone = warehouse.Contact.Phone, Email = warehouse.Contact.Email },
            Address = new AddressDTO { Street = warehouse.Address.Street, HouseNumber = warehouse.Address.HouseNumber, HouseNumberExtension = warehouse.Address.HouseNumberExtension, HouseNumberExtensionExtra = warehouse.Address.HouseNumberExtensionExtra, ZipCode = warehouse.Address.ZipCode, City = warehouse.Address.City, Province = warehouse.Address.Province, CountryCode = warehouse.Address.CountryCode }

        };
    }
}


public class WarehouseValidator : AbstractValidator<Warehouse>
{
    private readonly AppDbContext? _db;
    public WarehouseValidator(AppDbContext db)
    {
        _db = db;

        // juiste regels toepassen in de validator
        RuleFor(Warehouse => Warehouse.Name)
             .NotNull().WithMessage("warehouse name is required.")
             .NotEmpty().WithMessage("Warehouse name cannot be empty");

        RuleFor(Warehouse => Warehouse.Code)
            .NotNull().WithMessage("warehouse code is required.")
            .NotEmpty().WithMessage("Warehouse Code cannot be empty");

        RuleFor(Warehouse => Warehouse.Contact)
            .NotNull().WithMessage("warehouse contact is required.")
            .NotEmpty().WithMessage("Warehouse contact cannot be empty.")
            .Must(contact => _db.Contacts.Any(x => x.Id == contact.Id));

        RuleFor(Warehouse => Warehouse.Address)
            .NotNull().WithMessage("warehouse address is required.")
            .NotEmpty().WithMessage("Warehouse address cannot be empty.")
            .Must(address => _db.Addresses.Any(x => x.Id == address.Id));

    }
}
