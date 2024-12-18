public static class EnumUtil
{
    public static string EnumsToString<T>() => string.Join(", ", Enum.GetNames(typeof(T)));

    public static T? ParseOrIgnore<T>(string? strValue) where T : struct
    {
        if (string.IsNullOrEmpty(strValue)) return null;

        Enum.TryParse(typeof(T), strValue, true, out var result);

        if (result is not T enumType) return null;

        return enumType;
    }
}