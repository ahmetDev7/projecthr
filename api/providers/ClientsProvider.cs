using DTO.Client;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class ClientsProvider : BaseProvider<Client>
{
    private readonly IValidator<Client> _clientValidator;
    private readonly ContactProvider _contactProvider;
    private readonly AddressProvider _addressProvider;

    public ClientsProvider(AppDbContext db, IValidator<Client> clientValidator, ContactProvider contactProvider, AddressProvider addressProvider)
        : base(db)
    {
        _clientValidator = clientValidator;
        _contactProvider = contactProvider;
        _addressProvider = addressProvider;
    }

    public override Client? Create(BaseDTO createValues)
    {
        ClientRequest request = createValues as ClientRequest ?? throw new ApiFlowException("Could not process create client request. Save new client failed.");

        Contact? relatedContact = _contactProvider.GetOrCreateContact(request.Contact, request.ContactId);
        Address? relatedAddress = _addressProvider.GetOrCreateAddress(request.Address, request.AddressId);

        Client newClient = new Client(newInstance: true)
        {
            Name = request.Name,
        };

        if (relatedContact != null) newClient.ContactId = relatedContact.Id;
        if (relatedAddress != null) newClient.AddressId = relatedAddress.Id;

        ValidateModel(newClient);

        _db.Clients.Add(newClient);
        SaveToDBOrFail();

        return newClient;
    }
    public override Client? Delete(Guid id)
    {
        Client? foundClient = _db.Clients.FirstOrDefault(c => c.Id == id);

        if (foundClient == null) return null;

        _db.Clients.Remove(foundClient);

        SaveToDBOrFail();

        return foundClient;
    }


    protected override void ValidateModel(Client model) => _clientValidator.ValidateAndThrow(model);
}
