public static class CollectionUtil
{
    
    public static bool ContainsDuplicateId(List<Guid?> ids) // why not LINQ see: https://www.milanjovanovic.tech/blog/5-ways-to-check-for-duplicates-in-collections#benchmark-results
    {
        HashSet<Guid> hashSet = new();
        foreach (Guid? id in ids) {
            if(!id.HasValue) return true;

           if (!hashSet.Add(id.Value)){
                return true;
            }
        }
      
        return false;
    }
}