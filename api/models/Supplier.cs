// Table suppliers {
//   id uuid [pk]
//   code varchar
//   name varchar
//   reference varchar
//   contact_id uuid [ref: > contacts.id]
//   created_at datetime
//   updated_at datetime
// }

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

public class Supplier : IDTO
{
    public Supplier()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid? id {get;set;}
    public string? code {get;set;}
    public string? name {get;set;}
    public string? reference {get;set;}
    public Contact? contact_id{get;set;}
    public Address? address_id{get;set;}
    public DateTime CreatedAt {get;}
    public DateTime UpdatedAt{get;} 
    
}