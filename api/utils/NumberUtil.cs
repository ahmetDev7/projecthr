namespace Utils.Number;

public static class NumberUtil
{
    public static int EnsureNonNegative(int number) => number < 0 ? 0 : number;
    public static decimal EnsureNonNegativeWithFourDecimals(decimal number) => number < 0 ? 0 : Math.Round(number, 4);
    public static int MinimumInt(int number, int minimumInt) => number < minimumInt ? minimumInt : number;

    public static decimal EnsureNonNegativeWithTwoDecimals(decimal number) => number < 0 ? 0 : Math.Round(number, 2);
}