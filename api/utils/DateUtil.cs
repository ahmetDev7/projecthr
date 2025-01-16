namespace api.Utils.Date;
public static class DateUtil
{
    public static DateTime? ToUtcOrNull(DateTime? dateTime) =>
        (!dateTime.HasValue)
        ? null
        : DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc);

}