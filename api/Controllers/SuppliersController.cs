using Microsoft.AspNetCore.Mvc;
using DTO.Supplier;
using DTO.Item;
using DTO.Address;
using DTO.Contact;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly SupplierProvider _supplierProvider;

    public SuppliersController(SupplierProvider supplierProvider)
    {
        _supplierProvider = supplierProvider;
    }
    [HttpGet("{id}/items")]
    public IActionResult GetItemsSupplier(Guid id)
    {
        List<Item> items = _supplierProvider.GetItemsBySupplierId(id);
        if (_supplierProvider.GetById(id) == null) throw new ApiFlowException("Supplier not found");

        List<ItemResponse> itemsSupplier = items.Select(item => new ItemResponse
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
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        }).ToList();

        return Ok(itemsSupplier);
    }

    [HttpPost]
    public IActionResult Create(SupplierRequest request)
    {
        Supplier? supplier = _supplierProvider.Create(request);
        if (supplier == null) throw new ApiFlowException("Saving new Supplier failed.");


        return Ok(new
        {
            message = "Supplier created successfully.",
            created_supplier = new SupplierResponse
            {
                Id = supplier.Id,
                Code = supplier.Code,
                Name = supplier.Name,
                Reference = supplier.Reference,
                Contact = new ContactResponse
                {
                    Id = supplier.ContactId,
                    Name = supplier.Contact?.Name,
                    Function = supplier.Contact?.Function,
                    Phone = supplier.Contact?.Phone,
                    Email = supplier.Contact?.Email,
                    CreatedAt = supplier.Contact?.CreatedAt,
                    UpdatedAt = supplier.Contact?.UpdatedAt

                },
                Address = new AddressResponse
                {
                    Id = supplier.AddressId,
                    Street = supplier.Address?.Street,
                    HouseNumber = supplier.Address?.HouseNumber,
                    HouseNumberExtension = supplier.Address?.HouseNumberExtension,
                    HouseNumberExtensionExtra = supplier.Address?.HouseNumberExtensionExtra,
                    ZipCode = supplier.Address?.ZipCode,
                    City = supplier.Address?.City,
                    Province = supplier.Address?.Province,
                    CountryCode = supplier.Address?.CountryCode,
                    CreatedAt = supplier.Address?.CreatedAt,
                    UpdatedAt = supplier.Address?.UpdatedAt
                },
                CreatedAt = supplier.CreatedAt,
                UpdatedAt = supplier.UpdatedAt
            }
        });
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, SupplierRequest request)
    {
        Supplier? updatedSupplier = _supplierProvider.Update(id, request);
        if (updatedSupplier == null) return NotFound(new { message = $"Supplier not found for id '{id}'" });

        return Ok(new
        {
            message = "Supplier updated successfully.",
            updated_supplier = new SupplierResponse
            {
                Id = updatedSupplier.Id,
                Code = updatedSupplier.Code,
                Name = updatedSupplier.Name,
                Reference = updatedSupplier.Reference,
                Contact = new ContactResponse
                {
                    Id = updatedSupplier.ContactId,
                    Name = updatedSupplier.Contact?.Name,
                    Function = updatedSupplier.Contact?.Function,
                    Phone = updatedSupplier.Contact?.Phone,
                    Email = updatedSupplier.Contact?.Email,
                    CreatedAt = updatedSupplier.Contact?.CreatedAt,
                    UpdatedAt = updatedSupplier.Contact?.UpdatedAt
                },
                Address = new AddressResponse
                {
                    Id = updatedSupplier.AddressId,
                    Street = updatedSupplier.Address?.Street,
                    HouseNumber = updatedSupplier.Address?.HouseNumber,
                    HouseNumberExtension = updatedSupplier.Address?.HouseNumberExtension,
                    HouseNumberExtensionExtra = updatedSupplier.Address?.HouseNumberExtensionExtra,
                    ZipCode = updatedSupplier.Address?.ZipCode,
                    City = updatedSupplier.Address?.City,
                    Province = updatedSupplier.Address?.Province,
                    CountryCode = updatedSupplier.Address?.CountryCode,
                    CreatedAt = updatedSupplier.Address?.CreatedAt,
                    UpdatedAt = updatedSupplier.Address?.UpdatedAt
                },
                CreatedAt = updatedSupplier.CreatedAt,
                UpdatedAt = updatedSupplier.UpdatedAt
            }
        });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        Supplier? deletedSupplier = _supplierProvider.Delete(id);
        if (deletedSupplier == null) throw new ApiFlowException($"Supplier not found for id '{id}'");

        return Ok(new
        {
            Message = "Supplier succesfully deleted",
            deleted_supplier = new SupplierResponse
            {
                Id = deletedSupplier.Id,
                Code = deletedSupplier.Code,
                Name = deletedSupplier.Name,
                Reference = deletedSupplier.Reference,
                Contact = new ContactResponse
                {
                    Name = deletedSupplier.Contact?.Name,
                    Function = deletedSupplier.Contact?.Function,
                    Email = deletedSupplier.Contact?.Email,
                    Phone = deletedSupplier.Contact?.Phone,
                    CreatedAt = deletedSupplier.Contact?.CreatedAt,
                    UpdatedAt = deletedSupplier.Contact?.UpdatedAt
                },
                Address = new AddressResponse
                {
                    Street = deletedSupplier.Address?.Street,
                    HouseNumber = deletedSupplier.Address?.HouseNumber,
                    HouseNumberExtension = deletedSupplier.Address?.HouseNumberExtension,
                    HouseNumberExtensionExtra = deletedSupplier.Address?.HouseNumberExtensionExtra,
                    ZipCode = deletedSupplier.Address?.ZipCode,
                    City = deletedSupplier.Address?.City,
                    Province = deletedSupplier.Address?.Province,
                    CountryCode = deletedSupplier.Address?.CountryCode,
                    CreatedAt = deletedSupplier.Address?.CreatedAt,
                    UpdatedAt = deletedSupplier.Address?.UpdatedAt
                },
                CreatedAt = deletedSupplier.CreatedAt,
                UpdatedAt = deletedSupplier.UpdatedAt
            }
        });
    }

    [HttpGet("{id}")]
    public IActionResult ShowSingle(Guid id)
    {
        Supplier? supplier = _supplierProvider.GetById(id);

        if (supplier == null) return NotFound(new { message = $"Supplier not found for id '{id}'" });

        return Ok(new SupplierResponse
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Code = supplier.Code,
            Reference = supplier.Reference,
            Contact = new ContactResponse
            {
                Id = supplier.ContactId,
                Name = supplier.Contact?.Name,
                Function = supplier.Contact?.Function,
                Phone = supplier.Contact?.Phone,
                Email = supplier.Contact?.Email,
                CreatedAt = supplier.Contact?.CreatedAt,
                UpdatedAt = supplier.Contact?.UpdatedAt
            },
            Address = new AddressResponse
            {
                Id = supplier.AddressId,
                Street = supplier.Address?.Street,
                HouseNumber = supplier.Address?.HouseNumber,
                HouseNumberExtension = supplier.Address?.HouseNumberExtension,
                HouseNumberExtensionExtra = supplier.Address?.HouseNumberExtensionExtra,
                ZipCode = supplier.Address?.ZipCode,
                City = supplier.Address?.City,
                Province = supplier.Address?.Province,
                CountryCode = supplier.Address?.CountryCode,
                CreatedAt = supplier.Address?.CreatedAt,
                UpdatedAt = supplier.Address?.UpdatedAt
            },
            CreatedAt = supplier.CreatedAt,
            UpdatedAt = supplier.UpdatedAt
        });
    }
    [HttpGet]
    public IActionResult ShowAll() => Ok(_supplierProvider.GetAll().Select(ig => new SupplierResponse
    {
        Id = ig.Id,
        Code = ig.Code,
        Name = ig.Name,
        Reference = ig.Reference,
        Contact = new ContactResponse
        {
            Id = ig.ContactId,
            Name = ig.Contact?.Name,
            Function = ig.Contact?.Function,
            Phone = ig.Contact?.Phone,
            Email = ig.Contact?.Email,
            CreatedAt = ig.Contact?.CreatedAt,
            UpdatedAt = ig.Contact?.UpdatedAt
        },
        Address = new AddressResponse
        {
            Id = ig.AddressId,
            Street = ig.Address?.Street,
            HouseNumber = ig.Address?.HouseNumber,
            HouseNumberExtension = ig.Address?.HouseNumberExtension,
            HouseNumberExtensionExtra = ig.Address?.HouseNumberExtensionExtra,
            ZipCode = ig.Address?.ZipCode,
            City = ig.Address?.City,
            Province = ig.Address?.Province,
            CountryCode = ig.Address?.CountryCode,
            CreatedAt = ig.Address?.CreatedAt,
            UpdatedAt = ig.Address?.UpdatedAt
        },
        CreatedAt = ig.CreatedAt,
        UpdatedAt = ig.UpdatedAt

    }).ToList());
}