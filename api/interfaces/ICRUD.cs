public interface ICRUD<T> {
    
    public T GetById(int id);
    public List<T> GetAll();
    public bool Create();
    public T Update(int id);
    public bool Delete(int id);
}