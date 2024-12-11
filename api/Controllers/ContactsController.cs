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
}
