using Utils.Number;

public class Shipment
{
    public Shipment()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; set; }

    public DateTime? OrderDate { get; set; }
    public DateTime? RequestDate { get; set; }
    public DateTime? ShipmentDate { get; set; }

    public required string ShipmentType { get; set; }

    public string? ShipmentStatus { get; set; }

    public string? Notes { get; set; }
    public required string CarrierCode { get; set; }
    public string? CarrierDescription { get; set; }
    public required string ServiceCode { get; set; }
    public required string PaymentType { get; set; }
    public required string TransferMode { get; set; }
    public string? TotalPackageAmount { get; set; }

    private int _totalPackageCount = 0;
    public int? TotalPackageCount
    {
        get => _totalPackageCount;
        set => _totalPackageCount = NumberUtil.EnsureNonNegative((int)value);
    }

    private decimal _totalPackageWeight = 0;

    public decimal? TotalPackageWeight
    {
        get => _totalPackageWeight;
        set => _totalPackageWeight = NumberUtil.EnsureNonNegativeWithFourDecimals((decimal)value);
    }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    /*
    TODO: 
        order_id: uuid required
    */

}
