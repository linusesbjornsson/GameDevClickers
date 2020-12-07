using System.Numerics;

public class NumberFormatter : IFormatter
{
    private BigInteger _number;
    private static string[] SUFFIXES = { "K", "M", "B", "T", "q", "Q", "s", "S", "O", "N", "D", "U", "D", "T", "qu", "QU", "se", "SE", "OC", "NO", "V", "C" };
    private const int THRESHOLD = 5;

    public NumberFormatter(BigRational number) : this(number.WholePart)
    {
    }
    public NumberFormatter(BigInteger number)
    {
        _number = number;
    }
    public string Format()
    {
        return FormatWithSuffix(_number);
    }

    public static string FormatScientific(BigInteger number)
    {
        return FormatNumberScientificString(number.ToString());
    }

    public static string FormatWithSuffix(BigInteger number)
    {
        return FormatNumberWithSuffixString(number.ToString());
    }

    private static string FormatNumberScientificString(string numberString)
    {
        if (numberString.Length < THRESHOLD)
        {
            return numberString;
        }
        var exponent = numberString.Length - 1;
        var leadingDigit = numberString.Substring(0, 1);
        var decimals = numberString.Substring(1, 3);
        return $"{leadingDigit}.{decimals}e{exponent}";
    }

    private static string FormatNumberWithSuffixString(string numberAsString)
    {
        if (numberAsString.Length < THRESHOLD)
        {
            return numberAsString;
        }
        var exponentIndex = numberAsString.Length - 1;
        var leadingDigit = "";
        var decimals = "";
        switch (exponentIndex % 3)
        {
            case 0:
                leadingDigit = numberAsString.Substring(0, 1);
                decimals = numberAsString.Substring(1, 3);
                break;

            case 1:
                leadingDigit = numberAsString.Substring(0, 2);
                decimals = numberAsString.Substring(2, 3);
                break;

            case 2:
                leadingDigit = numberAsString.Substring(0, 3);
                decimals = numberAsString.Substring(3, 3);
                break;
        }
        var numberWithoutSuffix = $"{leadingDigit}.{decimals}";
        numberWithoutSuffix = numberWithoutSuffix.TrimEnd('0').TrimEnd('.');

        var suffix = GetSuffixForNumber((exponentIndex / 3) - 1);

        return $"{numberWithoutSuffix}{suffix}";
    }

    private static string GetSuffixForNumber(int suffixIndex)
    {
        return SUFFIXES[suffixIndex];
    }
}
