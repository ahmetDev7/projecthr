using FluentValidation;

public class InventoryRequestValidator : AbstractValidator<InventoryRequest>
{
    public InventoryRequestValidator()
    {
        RuleFor(inventoryRequest => inventoryRequest.Locations)
           .Custom((inventoryLocations, context) =>
           {
               if (inventoryLocations != null && CollectionUtil.ContainsDuplicateId(inventoryLocations.Select(l => l.LocationId).ToList()))
               {
                   context.AddFailure("locations must have unique location IDs. Duplicate location IDs are not allowed.");
               }
           });
    }
}

