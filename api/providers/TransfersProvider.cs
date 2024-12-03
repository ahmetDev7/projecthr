

using FluentValidation;

public class TransferProvider : BaseProvider<Transfer>
{
    private IValidator<Transfer> _transferValidator;

    public TransferProvider(AppDbContext db, IValidator<Transfer> validator) : base(db)
    {
        _transferValidator = validator;
    }

    public override Transfer? Create(BaseDTO createValues)
    {
        TransferRequest? req = createValues as TransferRequest;
        if (req == null) throw new ApiFlowException("Could not process create transfer request. Save new transfer failed.");

        Transfer newTransfer = new(newInstance: true)
        {
            TransferFromId = req.TransferFromId,
            TransferToId = req.TransferToId,
            Reference = req.Reference,
            TransferItems = req.Items?.Select(reqItem => new TransferItem(newInstance: true)
            {
                ItemId = reqItem.ItemId,
                Amount = reqItem.Amount
            }).ToList()
        };

        newTransfer.SetTransferStatus(req.TransferStatus);
        ValidateModel(newTransfer);
        _db.Transfers.Add(newTransfer);
        SaveToDBOrFail();
        return newTransfer;
    }

    protected override void ValidateModel(Transfer model) => _transferValidator.ValidateAndThrow(model);
}