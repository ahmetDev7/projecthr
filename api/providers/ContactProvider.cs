using System.Data;
using DTO.Contact;
using FluentValidation;


public class ContactProvider : BaseProvider<Contact>
{
    private IValidator<Contact> _contactValidator;

    public ContactProvider(AppDbContext db, IValidator<Contact> validator) : base(db)
    {
        _contactValidator = validator;
    }

    public override List<Contact> GetAll() => _db.Contacts.ToList();

    public override Contact? GetById(Guid id) =>
        _db.Contacts.FirstOrDefault(c => c.Id == id);

    public override Contact? Create(BaseDTO createValues)
    {
        ContactRequest? req = createValues as ContactRequest;
        if (req == null) throw new ApiFlowException("Invalid contact request. Could not create contact.");

        Contact newContact = new Contact(newInstance: true)
        {
            Name = req.Name,
            Phone = req.Phone,
            Email = req.Email
        };

        ValidateModel(newContact);

        _db.Contacts.Add(newContact);
        SaveToDBOrFail();

        return newContact;
    }

    public override Contact? Delete(Guid id)
    {
        Contact? foundContact = _db.Contacts.FirstOrDefault(c => c.Id == id);
        if (foundContact == null) return null;


        if (_db.Warehouses.Any(w => w.ContactId == id) ||
            _db.Clients.Any(c => c.ContactId == id) ||
            _db.Suppliers.Any(s => s.ContactId == id))
        {
            throw new ApiFlowException($"{id} The provided contact_id is in use and cannot be modified or deleted.");
        }
        _db.Contacts.Remove(foundContact);
        SaveToDBOrFail();

        return foundContact;
    }

    public override Contact? Update(Guid id, BaseDTO updateValues)
    {
        ContactRequest? req = updateValues as ContactRequest;
        if (req == null) throw new ApiFlowException("Invalid contact request. Could not update contact.");

        Contact? existingContact = _db.Contacts.FirstOrDefault(c => c.Id == id);
        if (existingContact == null) throw new ApiFlowException($"Contact not found for id '{id}'");

        existingContact.Name = req.Name;
        existingContact.Phone = req.Phone;
        existingContact.Email = req.Email;
        existingContact.SetUpdatedAt();

        ValidateModel(existingContact);

        _db.Contacts.Update(existingContact);
        SaveToDBOrFail();

        return existingContact;
    }

    protected override void ValidateModel(Contact model) => _contactValidator.ValidateAndThrow(model);

    // Andere methoden zoals GetOrCreateContact blijven zoals ze zijn
    public Contact? GetOrCreateContact(ContactRequest? contact = null, Guid? contactId = null)
    {
        if (contact == null && contactId == null) return null;

        if (contactId != null) return GetById(contactId.Value);

        if (contact != null) return Create(contact);

        return null;
    }
}
