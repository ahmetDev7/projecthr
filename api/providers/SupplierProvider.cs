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

    public override Supplier? Update(Guid id, BaseDTO updatedValues)
{
    SupplierReQuestDTO? req = updatedValues as SupplierReQuestDTO;
    if (req == null) throw new ApiFlowException("If you want to Update, you need to change something");

    Supplier? foundSupplier = GetById(id);
    if (foundSupplier == null) throw new ApiFlowException("No Supplier found");

    // Update the found supplier with the new values
    foundSupplier.Code = req.Code;
    foundSupplier.Name = req.Name;
    foundSupplier.Reference = req.Reference;

    // Update the Contact and Address entities
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

    // Validate the updated supplier
    ValidateModel(foundSupplier);

    // Save changes to the DB
    SaveToDBOrFail();

    return foundSupplier;
}

    public override Supplier? GetById(Guid id) => _db.Suppliers.Include(c => c.Contact).Include(a => a.Address).FirstOrDefault(i => i.Id == id);
    protected override void ValidateModel(Supplier model) => _supplierValidator.ValidateAndThrow(model);

}