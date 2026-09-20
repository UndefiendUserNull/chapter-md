namespace ChapterMD.Core;

public static class Utils
{
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
}
