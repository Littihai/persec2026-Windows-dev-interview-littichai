namespace Persec.Interview.Tests;

public class Q5_DigitSorterTests
{
    [Theory]
    // ตัวอย่างจากโจทย์
    [InlineData(3008, 8300L)]
    [InlineData(1989, 9981L)]
    [InlineData(2679, 9762L)]
    [InlineData(9163, 9631L)]
    // กรณีอื่น
    [InlineData(0, 0L)]
    [InlineData(7, 7L)]
    [InlineData(1000, 1000L)]
    [InlineData(1234567890, 9876543210L)]
    public void SortDescending_ReturnsDigitsInDescendingOrder(int input, long expected)
    {
        Assert.Equal(expected, DigitSorter.SortDescending(input));
    }

    [Fact]
    public void SortDescending_ResultLargerThanIntMax_DoesNotOverflow()
    {
        // เหตุผลที่คืนค่าเป็น long: input เป็น int ที่ถูกต้อง แต่ผลลัพธ์เกิน int.MaxValue
        Assert.Equal(9_999_999_991L, DigitSorter.SortDescending(1_999_999_999));
        Assert.Equal(8_776_444_321L, DigitSorter.SortDescending(int.MaxValue));
    }

    [Fact]
    public void SortDescending_Negative_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => DigitSorter.SortDescending(-1));
    }
}
