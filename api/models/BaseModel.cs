namespace Model;

public abstract class BaseModel{
    public Guid Id {get; set;}
    public required virtual DateTime CreatedAt {get; set;}
    public required virtual DateTime UpdatedAt {get; set;}

    public virtual void SetTimeStamps(){
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public virtual void SetUpdatedAt() {
        UpdatedAt = DateTime.Now;
    }
}