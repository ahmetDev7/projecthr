public static class DBUtil
{
    public static bool IsSaved(int? rowsAffected = 0)
    {
        return rowsAffected > 0;
    }

    public static void SaveChanges(AppDbContext db, string? errorMessage = null)
    {
        if (errorMessage == null) errorMessage = "Value not stored";
        if (!IsSaved(db.SaveChanges())) throw new ApiFlowException(errorMessage);
    }
}