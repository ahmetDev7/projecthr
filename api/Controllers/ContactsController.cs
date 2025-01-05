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

        if (newContact == null) NotFound(new { message = "Contact creation failed" });

        return Ok(new
        {
            message = "Contact created!",
            Contact = new ContactResponse
            {
                Id = newContact?.Id,
                Name = newContact?.Name,
                Phone = newContact?.Phone,
                Email = newContact?.Email,
                CreatedAt = newContact?.CreatedAt,
                UpdatedAt = newContact?.UpdatedAt
            }
        });
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] ContactRequest req)
    {
        Contact? updateContact = _contactProvider.Update(id, req);

        if (updateContact == null)
            return NotFound(new { message = $"Contact not found for id {id}" });

        return Ok(new
        {
            message = "Contact updated!",
            Contact = new ContactResponse
            {
                Id = updateContact.Id,
                Name = updateContact.Name,
                Phone = updateContact.Phone,
                Email = updateContact.Email,
                CreatedAt = updateContact.CreatedAt,
                UpdatedAt = updateContact.UpdatedAt
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
                Email = foundContact.Email,
                CreatedAt = foundContact.CreatedAt,
                UpdatedAt = foundContact.UpdatedAt
            }
        });
    }

    [HttpGet()]
    public IActionResult ShowAll() => Ok(_contactProvider.GetAll()?.Select(c => new ContactResponse
    {
        Id = c.Id,
        Name = c.Name,
        Phone = c.Phone,
        Email = c.Email,
        CreatedAt = c.CreatedAt,
        UpdatedAt = c.UpdatedAt
    }).ToList());

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        Contact? foundContact = _contactProvider.Delete(id);

        if (foundContact == null)
            return NotFound(new { message = $"Contact not found for id {id}" });

        return Ok(new
        {
            message = "Contact deleted!",
            Contact = new ContactResponse
            {
                Id = foundContact.Id,
                Name = foundContact.Name,
                Phone = foundContact.Phone,
                Email = foundContact.Email,
                CreatedAt = foundContact.CreatedAt,
                UpdatedAt = foundContact.UpdatedAt
            }
        });
    }
}
