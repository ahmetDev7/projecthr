public interface ICRUD<T> {
    
    public T? GetById(Guid id);
    public IDTO? GetByIdAsDTO(Guid id);
    public List<IDTO>? GetAll();
    public T? Create<IDTO>(IDTO dto);
    public T? Update<IDTO>(Guid id,IDTO dto);
    public T? Delete(Guid id);
}