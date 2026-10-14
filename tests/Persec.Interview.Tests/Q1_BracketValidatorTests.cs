namespace Persec.Interview.Tests;

public class Q1_BracketValidatorTests
{
    [Theory]
    // ตัวอย่างจากโจทย์
    [InlineData("()", true)]
    [InlineData("([]]", false)]
    [InlineData("([{}])", true)]
    [InlineData("([[{}]]]", false)]
    [InlineData(")", false)]
    [InlineData("(]}])", false)]
    [InlineData("([)]", false)]
    [InlineData("{", false)]
    [InlineData("([{)", false)]
    public void IsBalanced_ExamplesFromSpec(string input, bool expected)
    {
        Assert.Equal(expected, BracketValidator.IsBalanced(input));
    }

    [Theory]
    [InlineData("", true)]               // ไม่มีวงเล็บค้าง
    [InlineData("()[]{}", true)]         // หลายคู่ต่อกัน
    [InlineData("{[()()]}", true)]       // ซ้อนหลายชั้น
    [InlineData("((", false)]            // เปิดค้าง ความยาวคู่
    [InlineData("))((", false)]          // จำนวนครบแต่ลำดับผิด
    [InlineData("(a)", false)]           // มีตัวอักษรอื่นปน
    public void IsBalanced_EdgeCases(string input, bool expected)
    {
        Assert.Equal(expected, BracketValidator.IsBalanced(input));
    }

    [Fact]
    public void IsBalanced_Null_ReturnsFalse()
    {
        Assert.False(BracketValidator.IsBalanced(null));
    }

    [Fact]
    public void IsBalanced_VeryDeepNesting_DoesNotOverflowStack()
    {
        // ใช้ Stack<T> ไม่ใช่ recursion จึงรับ input ลึกมากได้
        string input = new string('(', 100_000) + new string(')', 100_000);
        Assert.True(BracketValidator.IsBalanced(input));
    }
}
