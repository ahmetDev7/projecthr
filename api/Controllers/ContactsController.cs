using DTO.Contact;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly ContactProvider _contactProvider;

    public ContactsController(ContactProvider contactProvider)
    {
        _contactProvider = contactProvider;
    }

    [HttpPost()]
    public IActionResult Create([FromBody] ContactRequest req)
    {
        Contact? newContact = _contactProvider.Create(req);

        if (newContact == null) NotFound(new{message = "Contact creation failed"});

        return Ok(new
        { 
            message = "Contact created!",
            Contact = new ContactResponse
            {
                Id = newContact?.Id,
                Name = newContact?.Name,
                Phone = newContact?.Phone,
                Email = newContact?.Email
            }
        });
    }

    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        Contact? foundContact = _contactProvider.GetById(id);

        if (foundContact == null)
            return NotFound(new { message = $"Contact not found for id {id}" });

        return Ok(new
        {
            message = "Contact found!",
            Contact = new ContactResponse
            {
                Id = foundContact.Id,
                Name = foundContact.Name,
                Phone = foundContact.Phone,
                Email = foundContact.Email
            }
        });
    }
}
