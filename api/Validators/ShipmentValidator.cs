using FluentValidation;
using Model;

public class ShipmentValidator : AbstractValidator<Shipment>
{
    public ShipmentValidator(AppDbContext db)
    {
    }
}