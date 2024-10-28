public interface ICRUD<T> {
    
    public T GetById();
    public List<T> GetAll();
    public T Create();
    public T Update();
    public T Delete();
}