public interface ICRUD<T> {
    
    public T? GetById(Guid id);
    public List<T>? GetAll();
    public T? Create<IDTO>(IDTO dto);
    public T? Update<IDTO>(Guid id,IDTO dto);
    public T? Delete(Guid id);
}