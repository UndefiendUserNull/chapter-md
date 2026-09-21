namespace ChapterMD.Core;

public static class Utils
{
    public static int RevertNumberTypeToDecimal(string num)
    {
        string[] romans = ["M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"];
        string[] boom = ["٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩"];

        if (romans.Any(num.Contains)) return FromRoman(num);
        else if (boom.Any(num.Contains)) return FromArabicIndic(num);

        if (int.TryParse(num, out int parsed))
            return parsed;
        else
            throw new Exception($"Couldn't find numbering type for {num} to convert to an integer.");
    }

    public static string ConvertDecimalToNumberType(int num, NumberingType type)
    {

        switch (type)
        {
            case NumberingType.Romanian:
                return ToRoman(num);
            case NumberingType.Arabic:
                return ToArabicIndic(num);
        }

        return num.ToString();
    }

    public static string ToArabicIndic(int number)
    {
        char[] map = ['٠', '١', '٢', '٣', '٤', '٥', '٦', '٧', '٨', '٩'];

        return string.Concat(
            Math.Abs(number).ToString().Select(c => map[c - '0'])
        );
    }

    // Copied from (https://www.reddit.com/user/Bio2hazard/)
    public static string ToRoman(int num) => num switch
    {
        >= 1000 => "M" + ToRoman(num - 1000),
        >= 900 => "CM" + ToRoman(num - 900),
        >= 500 => "D" + ToRoman(num - 500),
        >= 400 => "CD" + ToRoman(num - 400),
        >= 100 => "C" + ToRoman(num - 100),
        >= 90 => "XC" + ToRoman(num - 90),
        >= 50 => "L" + ToRoman(num - 50),
        >= 40 => "XL" + ToRoman(num - 40),
        >= 10 => "X" + ToRoman(num - 10),
        >= 9 => "IX" + ToRoman(num - 9),
        >= 5 => "V" + ToRoman(num - 5),
        >= 4 => "IV" + ToRoman(num - 4),
        >= 1 => "I" + ToRoman(num - 1),
        _ => string.Empty
    };

    public static int FromArabicIndic(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Input is empty.", nameof(text));

        int start = 0;
        int sign = 1;

        if (text[0] == '-') { sign = -1; start = 1; }
        else if (text[0] == '+') { start = 1; }

        if (start == text.Length)
            throw new FormatException("No digits found.");

        long result = 0;
        for (int i = start; i < text.Length; i++)
        {
            int digit = text[i] - '\u0660'; // '٠'
            if ((uint)digit > 9)
                throw new FormatException($"Invalid Arabic-Indic digit: '{text[i]}'.");
            result = result * 10 + digit;
        }

        return checked((int)(sign * result));
    }

    public static int FromRoman(string text)
    {
        if (string.IsNullOrEmpty(text))
            return 0; // mirrors ToRoman(0) == ""

        int result = 0;
        int prev = 0;

        for (int i = text.Length - 1; i >= 0; i--)
        {
            int value = RomanValue(text[i]);
            if (value < prev)
                result -= value;
            else
            {
                result += value;
                prev = value;
            }
        }

        return result;
    }

    private static int RomanValue(char c) => char.ToUpperInvariant(c) switch
    {
        'I' => 1,
        'V' => 5,
        'X' => 10,
        'L' => 50,
        'C' => 100,
        'D' => 500,
        'M' => 1000,
        _ => throw new FormatException($"Invalid Roman numeral character: '{c}'.")
    };
}
