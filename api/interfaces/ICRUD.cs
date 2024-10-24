public interface ICRUD<T> {
    
    public T GetById(int id);
    public List<T> GetAll();
    public T Create();
    public T Update(int id);
    public T Delete(int id);
}