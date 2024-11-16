namespace Utils.Number;

public static class NumberUtil{
    public static int EnsureNonNegative(int number) => number < 0 ? 0 : number;

    public static int MinimumInt(int number, int minimumInt) => number < minimumInt ? minimumInt : number;
}