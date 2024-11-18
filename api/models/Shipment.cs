using System.ComponentModel.DataAnnotations;
using Utils.Number;

namespace Model;

public class Shipment : BaseModel
{
    public Shipment(){}
    public Shipment(bool newInstance = false, bool isUpdate = false) : base(newInstance, isUpdate){}
    public DateTime? OrderDate { get; set; }
    public DateTime? RequestDate { get; set; }
    public DateTime? ShipmentDate { get; set; }
    
    [Required]
    public string? ShipmentType { get; set; }
    public string? ShipmentStatus { get; set; }
    public string? Notes { get; set; }
    
    [Required]
    public string? CarrierCode { get; set; }
    public string? CarrierDescription { get; set; }
    [Required]
    public string? ServiceCode { get; set; }
    [Required]
    public string? PaymentType { get; set; }
    [Required]
    public string? TransferMode { get; set; }
    public string? TotalPackageAmount { get; set; }

    // private int _totalPackageCount = 0;
    // public int? TotalPackageCount
    // {
    //     get => _totalPackageCount;
    //     set => _totalPackageCount = NumberUtil.EnsureNonNegative((int)value);
    // }

    // private decimal _totalPackageWeight = 0;

    // public decimal? TotalPackageWeight
    // {
    //     get => _totalPackageWeight;
    //     set => _totalPackageWeight = NumberUtil.EnsureNonNegativeWithFourDecimals((decimal)value);
    // }

    // /*
    // TODO: 
    //     order_id: uuid required
    // */
}
