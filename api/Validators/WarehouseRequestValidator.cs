using FluentValidation;

public class WarehouseRequestValidator : AbstractValidator<WarehouseRequest>
{
    public WarehouseRequestValidator(AppDbContext db)
    {
        RuleFor(warehouseRequest => warehouseRequest.ContactIds)
           .Custom((contactIds, context) =>
           {
               if (contactIds != null && CollectionUtil.ContainsDuplicateId(contactIds.Select(l => l).ToList()))
               {
                   context.AddFailure("Contacts must have unique contact IDs. Duplicate contacts are not allowed.");
               }
           });

        RuleFor(request => request.ContactIds)
            .NotEmpty().WithMessage("A warehouse requires at least one contact.");

        RuleForEach(warehouseRequest => warehouseRequest.ContactIds).ChildRules(contactId =>
        {
            contactId.RuleFor(contactId => contactId)
                .NotNull().WithMessage("The warehouse_id field is required.")
                .NotEmpty().WithMessage("The warehouse_id field cannot be empty.")
                .Custom((contactId, context) =>
                {
                    if (contactId != null && !db.Contacts.Any(i => i.Id == contactId))
                    {
                        context.AddFailure("contact_ids", $"The provided contact_id {contactId} does not exist.");
                    }
                });
        });
    }
}