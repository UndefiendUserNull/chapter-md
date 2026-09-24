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

    public static string ConvertDecimalToNumberType(int num, NumberingStyle type)
    {

        switch (type)
        {
            case NumberingStyle.Romanian:
                return ToRoman(num);
            case NumberingStyle.Arabic:
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

    public static string ToRoman(int num)
    {
        if (num == 0) return "0";
        else
            return ConvertToRoman(num);
    }

    // Copied from (https://www.reddit.com/user/Bio2hazard/)
    private static string ConvertToRoman(int num) => num switch
    {
        >= 1000 => "M" + ConvertToRoman(num - 1000),
        >= 900 => "CM" + ConvertToRoman(num - 900),
        >= 500 => "D" + ConvertToRoman(num - 500),
        >= 400 => "CD" + ConvertToRoman(num - 400),
        >= 100 => "C" + ConvertToRoman(num - 100),
        >= 90 => "XC" + ConvertToRoman(num - 90),
        >= 50 => "L" + ConvertToRoman(num - 50),
        >= 40 => "XL" + ConvertToRoman(num - 40),
        >= 10 => "X" + ConvertToRoman(num - 10),
        >= 9 => "IX" + ConvertToRoman(num - 9),
        >= 5 => "V" + ConvertToRoman(num - 5),
        >= 4 => "IV" + ConvertToRoman(num - 4),
        >= 1 => "I" + ConvertToRoman(num - 1),
        _ => string.Empty,
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
            result = (result * 10) + digit;
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

    public static (int Min, int Max) ParseRange(string text)
    {
        var block = text.Replace(" ", string.Empty).Trim();

        if (block.Length < 5 || block[0] != '[')
            throw new FormatException($"Invalid range format '{text}'");

        block = block[1..(block.IndexOf(']'))]; // Without brackets

        int commaPos = block.IndexOf(',');
        if (commaPos < 0) throw new FormatException($"Missing comma '{text}'");

        var min = int.Parse(block[..commaPos].Trim());
        var max = int.Parse(block[(commaPos + 1)..].Trim());

        return (min, max);
    }

}
