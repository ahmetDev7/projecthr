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

    public override Client? GetById(Guid id)=>
    _db.Clients.Include(c => c.Contact).Include(c => c.Address).FirstOrDefault(c => c.Id == id);

    public override List<Client>? GetAll() => _db.Clients.Include(c => c.Contact).Include(c => c.Address).ToList();
    
    public override Client? Create(BaseDTO createValues)
    {
        ClientRequest request = createValues as ClientRequest ?? throw new ApiFlowException("Could not process create client request. Save new client failed.");

        Client newClient = new Client(newInstance: true)
        {
            Name = request.Name,
            AddressId = request.AddressId,
            ContactId = request.ContactId
        };

        ValidateModel(newClient);

        _db.Clients.Add(newClient);
        SaveToDBOrFail();

        return GetById(newClient.Id);
    }

    public override Client? Update(Guid id, BaseDTO updateValues)
    {
        ClientRequest request = updateValues as ClientRequest 
            ?? throw new ApiFlowException("Could not process update client request. Update failed.");

        Client? existingClient = _db.Clients.FirstOrDefault(c => c.Id == id);

        existingClient.Name = request.Name;
        existingClient.ContactId = request.ContactId;
        existingClient.AddressId =  request.AddressId;

        ValidateModel(existingClient);
        existingClient.SetUpdatedAt();

        _db.Clients.Update(existingClient);
        SaveToDBOrFail();

        return GetById(existingClient.Id);
    }
    public List<Order> GetRelatedOrdersById(Guid clientId) => _db.Orders.Where(o => o.BillToClientId == clientId).ToList();

    protected override void ValidateModel(Client model) => _clientValidator.ValidateAndThrow(model);
}
