using System.ComponentModel.DataAnnotations;

public class Order : BaseModel
{
    public Order() { }
    public Order(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public DateTime? OrderDate { get; set; }
    [Required]
    public DateTime? RequestDate { get; set; }
    public string? Reference { get; set; }
    public string? ReferenceExtra { get; set; }
    [Required]
    public OrderStatus? OrderStatus { get; set; } // default: order_status.pending
    public string? Notes { get; set; }
    public string? PickingNotes { get; set; }
    private decimal? _totalAmount;
    public decimal? TotalAmount
    {
        get => _totalAmount;
        set => _totalAmount = value.HasValue ? NumberUtil.EnsureNonNegativeWithTwoDecimals(value.Value) : (decimal?)null;
    }
    private decimal? _totalDiscount;
    public decimal? TotalDiscount
    {
        get => _totalDiscount;
        set => _totalDiscount = value.HasValue ? NumberUtil.EnsureNonNegativeWithTwoDecimals(value.Value) : (decimal?)null;
    }
    private decimal? _totalTax;
    public decimal? TotalTax
    {
        get => _totalTax;
        set => _totalTax = value.HasValue ? NumberUtil.EnsureNonNegativeWithTwoDecimals(value.Value) : (decimal?)null;
    }
    private decimal? _totalSurcharge;
    public decimal? TotalSurcharge
    {
        get => _totalSurcharge;
        set => _totalSurcharge = value.HasValue ? NumberUtil.EnsureNonNegativeWithTwoDecimals(value.Value) : (decimal?)null;
    }
    [Required]
    public Guid? WarehouseId { get; set; }

    public Warehouse? Warehouse { get; set; }
    [Required]
    public ICollection<OrderItem>? OrderItems { get; set; }
    public Guid? ShipToClientId { get; set; }
    public Client? ShipToClient { get; set; }
    [Required]
    public Guid? BillToClientId { get; set; }
    public Client? BillToClient { get; set; }

    public ICollection<OrderShipment>? OrderShipments { get; set; }
    public string? CreatedBy { get; set; }
}
