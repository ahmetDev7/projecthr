using DTO.Item;
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
    public IActionResult Create([FromBody] TransferRequestCreate req)
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
                Items = newTransfer.TransferItems?.Select(ti => new TransferItemDTO()
                {
                    ItemId = ti.ItemId,
                    Amount = ti.Amount,
                }).ToList(),
                CreatedAt = newTransfer.CreatedAt,
                UpdatedAt = newTransfer.UpdatedAt,
            }
        });
    }

    [HttpPut("{transferId}")]
    public IActionResult CommitTransfer(Guid transferId)
    {
        Transfer? commitedTransfer = _transferProvider.CommitTransfer(transferId);
        if (commitedTransfer == null) throw new ApiFlowException("Commting transfer failed.");

        return Ok(new
        {
            message = "Transfer commited!",
            created_transfer = new TransferResponse
            {
                Id = commitedTransfer.Id,
                Reference = commitedTransfer.Reference,
                TransferFromId = commitedTransfer.TransferFromId,
                TransferToId = commitedTransfer.TransferToId,
                TransferStatus = commitedTransfer.TransferStatus.ToString(),
                Items = commitedTransfer.TransferItems?.Select(ti => new TransferItemDTO()
                {
                    ItemId = ti.ItemId,
                    Amount = ti.Amount,
                }).ToList(),
                CreatedAt = commitedTransfer.CreatedAt,
                UpdatedAt = commitedTransfer.UpdatedAt,
            }
        });
    }

    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        Transfer? foundTransfer = _transferProvider.GetById(id);

        return (foundTransfer == null)
            ? NotFound(new { message = $"Transfer not found for id '{id}'" })
            : Ok(new TransferResponse
            {
                Id = foundTransfer.Id,
                Reference = foundTransfer.Reference,
                TransferFromId = foundTransfer.TransferFromId,
                TransferToId = foundTransfer.TransferToId,
                TransferStatus = foundTransfer.TransferStatus.ToString(),
                Items = foundTransfer.TransferItems?.Select(ti => new TransferItemDTO()
                {
                    ItemId = ti.ItemId,
                    Amount = ti.Amount,
                }).ToList(),
                CreatedAt = foundTransfer.CreatedAt,
                UpdatedAt = foundTransfer.UpdatedAt,
            });
    }

      [HttpGet()]
      public IActionResult ShowAll() => Ok(_transferProvider.GetAll().Select(t => new TransferResponse
      {
          Id = t.Id,
          Reference = t.Reference,
          TransferFromId = t.TransferFromId,
          TransferToId = t.TransferToId,
          TransferStatus = t.TransferStatus.ToString(),
          Items = t.TransferItems?.Select(ti => new TransferItemDTO()
          {
              ItemId = ti.ItemId,
              Amount = ti.Amount,
          }).ToList(),
          CreatedAt = t.CreatedAt,
          UpdatedAt = t.UpdatedAt,
      }));

      [HttpGet("{transferId}/items")]
      public IActionResult GetItemsFromTransfer(Guid transferId)
      {
          Transfer? foundTransfer = _transferProvider.GetById(transferId);
          if (foundTransfer == null) return NotFound(new { message = $"Transfer not found for id '{transferId}'" });

          Item? item = _transferProvider.GetItemsFromTransfer(foundTransfer);
          return Ok(new ItemResponse
          {
              Id = item.Id,
              Code = item.Code,
              Description = item.Description,
              ShortDescription = item.ShortDescription,
              UpcCode = item.UpcCode,
              ModelNumber = item.ModelNumber,
              CommodityCode = item.CommodityCode,
              UnitPurchaseQuantity = item.UnitPurchaseQuantity,
              UnitOrderQuantity = item.UnitOrderQuantity,
              PackOrderQuantity = item.PackOrderQuantity,
              SupplierReferenceCode = item.SupplierReferenceCode,
              SupplierPartNumber = item.SupplierPartNumber,
              ItemGroupId = item.ItemGroupId,
              ItemLineId = item.ItemLineId,
              ItemTypeId = item.ItemTypeId,
              SupplierId = item.SupplierId,
          });
      }
}