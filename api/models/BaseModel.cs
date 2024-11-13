using System.ComponentModel.DataAnnotations;

namespace Model;

public abstract class BaseModel
{

    public BaseModel() { }
    public BaseModel(bool newInstance)
    {
        SetTimeStamps();
    }

    [Required]
    public Guid Id { get; set; }

    [Required]
    public virtual DateTime CreatedAt { get; set; }
    [Required]
    public virtual DateTime UpdatedAt { get; set; }

    public virtual void SetTimeStamps()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public virtual void SetUpdatedAt()
    {
        UpdatedAt = DateTime.Now;
    }
}