using System.ComponentModel.DataAnnotations;

public abstract class BaseModel
{
    public BaseModel() { }
    public BaseModel(bool newInstance = false, bool isUpdate = false)
    {
        if (newInstance) SetTimeStamps();
        if (isUpdate) SetUpdatedAt();
    }

    [Required]
    public Guid Id { get; set; }

    [Required]
    public virtual DateTime CreatedAt { get; set; }
    [Required]
    public virtual DateTime UpdatedAt { get; set; }

    public virtual void SetTimeStamps()
    {
        DateTime now = DateTime.UtcNow;
        CreatedAt = now;
        UpdatedAt = now;
    }

    public virtual void SetUpdatedAt()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}