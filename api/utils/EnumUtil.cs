public static class EnumUtil {
    public static string EnumsToString<T>() => string.Join(", ", Enum.GetNames(typeof(T)));
    
}