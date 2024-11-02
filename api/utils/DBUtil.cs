public static class DBUtil{
    public static bool IsSaved(int? rowsAffected = 0){
        return rowsAffected > 0;
    }
}