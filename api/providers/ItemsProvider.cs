public class ItemsProvider : ICRUD<Item>
{
    readonly JsonProvider _jsonProvider;

    public ItemsProvider()
    {
        _jsonProvider = new JsonProvider();
    }

    public Item Create()
    {
        throw new NotImplementedException();
    }

    public Item Delete()
    {
        throw new NotImplementedException();
    }

    public List<Item>? GetAll()
    {
        return _jsonProvider.Decode<Item>(targetFile:"items.json");
    }

    public Item GetById()
    {
        throw new NotImplementedException();
    }

    public Item Update()
    {
        throw new NotImplementedException();
    }
}