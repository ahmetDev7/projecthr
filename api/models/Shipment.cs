using System.ComponentModel.DataAnnotations;
using Utils.Number;

public class Shipment : BaseModel
{
    public Shipment() { }
    public Shipment(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate) { }
    public List<Guid?> OrderIds { get; set; }
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
    public PaymentType? PaymentType { get; set; } // Automatic, Manual
    [Required]
    public TransferMode? TransferMode { get; set; } // Air, Sea, Ground

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

    public void SetShipmentType(string? strShipmentType) => ShipmentType = EnumUtil.ParseOrIgnore<ShipmentType>(strShipmentType);

    public void SetShipmentStatus(string? strShipmentStatus) => 
        ShipmentStatus = strShipmentStatus == null 
            ? global::ShipmentStatus.Pending  // on strShipmentStatus null set to default (Pending)
            : EnumUtil.ParseOrIgnore<ShipmentStatus>(strShipmentStatus);
    
    public void SetPaymentType(string? strPaymentType) => PaymentType = EnumUtil.ParseOrIgnore<PaymentType>(strPaymentType);
    public void SetTransferMode(string? strTransferMode) => TransferMode = EnumUtil.ParseOrIgnore<TransferMode>(strTransferMode);
    
}
