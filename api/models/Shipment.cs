using System.ComponentModel.DataAnnotations;
using Utils.Number;

public class Shipment : BaseModel
{
    public Shipment() { }
    public Shipment(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }

    [Required]
    public Guid? OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? RequestDate { get; set; }
    public DateTime? ShipmentDate { get; set; }

    [Required]
    public ShipmentType? ShipmentType { get; set; } // I or O
    public ShipmentStatus? ShipmentStatus { get; set; } // Pending, Deliverd, Transit
    public string? Notes { get; set; }

    [Required]
    public string? CarrierCode { get; set; }
    public string? CarrierDescription { get; set; }
    [Required]
    public string? ServiceCode { get; set; }
    [Required]
    public string? PaymentType { get; set; } // Automatic, Manual
    [Required]
    public string? TransferMode { get; set; } // Air, Sea, Ground

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

    public ICollection<ShipmentItem>? ShipmentItems { get; set; }

    public void SetShipmentType(string? strShipmentType)
    {
        if (strShipmentType == null) return;

        Enum.TryParse(typeof(ShipmentType), strShipmentType, true, out var result);
        if (result is not ShipmentType shipmentType) return;
        
        this.ShipmentType = shipmentType;
    }

    public void SetShipmentStatus(string? strShipmentStatus)
    {
        if (strShipmentStatus == null){
            this.ShipmentStatus = global::ShipmentStatus.Pending;
            return;
        }

        Enum.TryParse(typeof(ShipmentStatus), strShipmentStatus, true, out var result);
        if (result is not ShipmentStatus shipmentStatus) return;
        
        this.ShipmentStatus = shipmentStatus;
    }
}
