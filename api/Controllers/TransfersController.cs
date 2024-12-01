using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TransfersController : ControllerBase
{
    private TransferProvider _transferProvider;

    public TransfersController(TransferProvider transferProvider)
    {
        _transferProvider = transferProvider;
    }

    [HttpPost()]
    public IActionResult Create([FromBody] TransferRequest req)
    {
        Transfer? newTransfer = _transferProvider.Create(req);
        if (newTransfer == null) throw new ApiFlowException("Saving new transfer failed.");

        return Ok(new
        {
            message = "Transfer created!",
            created_transfer = new TransferResponse
            {
                Id = newTransfer.Id,
                Reference = newTransfer.Reference,
                TransferFromId = newTransfer.TransferFromId,
                TransferToId = newTransfer.TransferToId,
                TransferStatus = newTransfer.TransferStatus.ToString(),
                Items = newTransfer.TransferItems?.Select(ti => new TransferItemDTO(){
                    ItemId = ti.ItemId,
                    Amount = ti.Amount,
                }).ToList(),
                CreatedAt = newTransfer.CreatedAt,
                UpdatedAt = newTransfer.UpdatedAt,
            }
        });
    }


}