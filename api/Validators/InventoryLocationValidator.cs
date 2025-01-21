using FluentValidation;

public class InventoryRequestValidator : AbstractValidator<InventoryRequest>
{
    public InventoryRequestValidator(AppDbContext db)
    {
        RuleFor(inventoryRequest => inventoryRequest.Locations)
           .Custom((inventoryLocations, context) =>
           {
               if (inventoryLocations != null && CollectionUtil.ContainsDuplicateId(inventoryLocations.Select(l => l.LocationId).ToList()))
               {
                   context.AddFailure("locations must have unique location IDs. Duplicate location IDs are not allowed.");
               }

               foreach (InventoryLocationRR? row in inventoryLocations)
               {
                   if (db.Locations.Any(l => l.Id == row.LocationId) == false)
                   {
                       context.AddFailure($"Location ID {row.LocationId} not found.");
                   }
               }
           });
    }
}

