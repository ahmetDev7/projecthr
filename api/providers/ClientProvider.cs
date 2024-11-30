using DTO.Client;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class ClientProvider : BaseProvider<Client>
{
    private readonly IValidator<Client> _clientValidator;
    private readonly ContactProvider _contactProvider;
    private readonly AddressProvider _addressProvider;

    public ClientProvider(AppDbContext db, IValidator<Client> clientValidator, ContactProvider contactProvider, AddressProvider addressProvider)
        : base(db)
    {
        _clientValidator = clientValidator;
        _contactProvider = contactProvider;
        _addressProvider = addressProvider;
    }


    public override Client? Create(BaseDTO createValues)
    {
        ClientRequest request = createValues as ClientRequest ?? throw new ApiFlowException("Could not process create client request. Save new client failed.");
        
        if (request.ContactId == null && request.Contact == null) throw new ApiFlowException("Either contact_id or contact fields must be filled.");
        if (request.AddressId == null && request.Address == null) throw new ApiFlowException("Either address_id or address fields must be filled.");

        Contact? relatedContact = GetOrCreateContact(request);
        Address? relatedAddress = GetOrCreateAddress(request);
        
        if (relatedContact == null || relatedAddress == null) throw new ApiFlowException("Failed to process contact or address.");
        
        Client newClient = new Client(newInstance: true)
        {
            Name = request.Name,
            ContactId = relatedContact.Id,
            AddressId = relatedAddress?.Id
        };

        ValidateModel(newClient);
        _db.Clients.Add(newClient);
        SaveToDBOrFail();

        return newClient;

    }
    private Contact? GetOrCreateContact(ClientRequest request)
    {
        if (request.ContactId != null)
        {
            Contact? existingContact = _contactProvider.GetById(request.ContactId.Value);
            if (existingContact != null) return existingContact ?? throw new ApiFlowException("contact_id does not exist.");
        }
        if (request.Contact != null) return _contactProvider.Create(request.Contact);

        throw new ApiFlowException("No valid contact or contact_id provided.");
    }
    
    private Address? GetOrCreateAddress(ClientRequest request)
    {
        if (request.AddressId != null){
            Address? existingAddress = _addressProvider.GetById(request.AddressId.Value);
            if (existingAddress != null) return existingAddress ?? throw new ApiFlowException("address_id does not exist.");   
        }
        if (request.Address != null) return _addressProvider.Create(request.Address);

        throw new ApiFlowException("No valid address or address_id provided.");
    }


    protected override void ValidateModel(Client model) => _clientValidator.ValidateAndThrow(model);
}
