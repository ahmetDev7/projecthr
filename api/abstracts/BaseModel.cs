using System.ComponentModel.DataAnnotations;

namespace Model;

public abstract class BaseModel
{
    public BaseModel (){}
    public BaseModel(bool newInstance=false, bool isUpdate = false)
    {
        if(newInstance) SetTimeStamps();
        if(isUpdate) SetUpdatedAt();
    }

    [Required]
    public Guid Id { get; set; }

    [Required]
    public virtual DateTime CreatedAt { get; set; }
    [Required]
    public virtual DateTime UpdatedAt { get; set; }

    public virtual void SetTimeStamps()
    {
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public virtual void SetUpdatedAt()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}