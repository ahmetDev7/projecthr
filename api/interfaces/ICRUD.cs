public interface ICRUD<T> {
    
    public T? GetById(Guid id);
    public List<T>? GetAll();
    public T? Create<IDTO>(IDTO dto);
    public T? Update(Guid id);
    public T? Delete(Guid id);
}