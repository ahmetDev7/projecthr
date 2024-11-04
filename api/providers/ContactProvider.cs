using CargoHub.DTOs;

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

        if(_db.SaveChanges() < 1){
            throw new Exception("An error occurred while saving the address");
        }

        return newContact; 
    }

    public Contact Update(Guid id)
    {
        throw new NotImplementedException();
    }

    public Contact Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Contact? GetById(Guid id)
    {
        return _db.Contacts.FirstOrDefault(c => c.Id == id);
    }
}
