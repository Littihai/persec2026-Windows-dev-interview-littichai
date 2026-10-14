namespace Persec.Interview.Tests;

public class Q4_RomanNumeralTests
{
    [Theory]
    // ตัวอย่างจากโจทย์
    [InlineData(1989, "MCMLXXXIX")]
    [InlineData(2000, "MM")]
    [InlineData(68, "LXVIII")]
    [InlineData(109, "CIX")]
    // กรณีลบและขอบเขต
    [InlineData(1, "I")]
    [InlineData(4, "IV")]
    [InlineData(9, "IX")]
    [InlineData(40, "XL")]
    [InlineData(90, "XC")]
    [InlineData(400, "CD")]
    [InlineData(900, "CM")]
    [InlineData(3999, "MMMCMXCIX")]
    public void ToRoman_And_FromRoman(int number, string roman)
    {
        Assert.Equal(roman, RomanNumeral.ToRoman(number));
        Assert.Equal(number, RomanNumeral.FromRoman(roman));
    }

    [Fact]
    public void RoundTrip_AllValuesInRange()
    {
        for (int n = RomanNumeral.MinValue; n <= RomanNumeral.MaxValue; n++)
        {
            Assert.Equal(n, RomanNumeral.FromRoman(RomanNumeral.ToRoman(n)));
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(4000)]
    public void ToRoman_OutOfRange_Throws(int number)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RomanNumeral.ToRoman(number));
    }

    [Fact]
    public void FromRoman_AcceptsLowercase()
    {
        Assert.Equal(1989, RomanNumeral.FromRoman("mcmlxxxix"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("IIII")]   // ต้องเขียน IV
    [InlineData("VV")]     // ต้องเขียน X
    [InlineData("IC")]     // ลบข้ามหลักไม่ได้ ต้องเขียน XCIX
    [InlineData("XM")]
    [InlineData("MMMM")]   // เกิน 3999
    [InlineData("ABC")]    // ไม่ใช่สัญลักษณ์โรมัน
    public void FromRoman_InvalidInput_Throws(string roman)
    {
        Assert.Throws<FormatException>(() => RomanNumeral.FromRoman(roman));
    }
}
