public abstract class BaseProvider<T>
{
    private string ProviderName { get; set; }

    protected AppDbContext _db;

    public BaseProvider(AppDbContext db)
    {
        ProviderName = GetType().Name;
        _db = db;
    }

    public virtual T? GetById(Guid id) { return default(T); }
    public virtual List<T>? GetAll() => default(List<T>);
    public virtual T? Create(BaseDTO createValues) => default(T);
    public virtual T? Update(Guid id, BaseDTO updatedValues) => default(T);
    public virtual T? Delete(Guid id) => default(T);

    protected virtual void SaveToDBOrFail(string? errorMessage = null)
    {
        string message = errorMessage ?? $"Failed to save {ProviderName}.";
        DBUtil.SaveChanges(_db, message);
    }

    protected abstract void ValidateModel(T model);
}
