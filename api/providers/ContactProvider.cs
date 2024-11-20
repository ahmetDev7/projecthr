using DTO.Supplier;

public class ContactProvider : ICRUD<Contact>
{
    private readonly AppDbContext _db;

    public ContactProvider(AppDbContext db)
    {
        _db = db;
    }
    public List<Contact> GetAll()
    {
        return _db.Contacts.ToList();
    }

    public Contact? Create<IDTO>(IDTO newElement)
    {
        ContactDTO? request = newElement as ContactDTO;
        if(request == null) throw new Exception("Request invalid");

        Contact newContact = new()
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
        };

        _db.Contacts.Add(newContact);

        DBUtil.SaveChanges(_db, "Contact not stored");

        return newContact; 
    }


    public Contact Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Contact? GetById(Guid id)
    {
        return _db.Contacts.FirstOrDefault(c => c.Id == id);
    }

    public Contact? Update<IDTO>(Guid id, IDTO dto)
    {
        throw new NotImplementedException();
    }

    public Contact? GetOrCreateContact(SupplierRequest request)
    {
        if (request == null)
        {
            throw new ApiFlowException("Request object is null.");
        }

        if (request.Contact_id != null)
        {
            Contact? existingContact = GetById(request.Contact_id.Value);
            if (existingContact == null) throw new ApiFlowException("contact_id does not exist");
            return existingContact;
        }

        if (request.Contact != null)
        {
            return Create<ContactDTO>(request.Contact);
        }

        throw new ApiFlowException("Both contact_id and contact data are missing. Unable to create supplier contact.");
    }

}
