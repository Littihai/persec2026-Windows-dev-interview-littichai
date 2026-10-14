namespace Persec.Interview.Tests;

public class Q6_TribonacciTests
{
    [Fact]
    public void Generate_SpecExample1()
    {
        Assert.Equal(new[] { 1, 3, 5, 9, 17 }, Tribonacci.Generate([1, 3, 5], 5));
    }

    [Fact]
    public void Generate_SpecExample2()
    {
        Assert.Equal(new[] { 2, 2, 2 }, Tribonacci.Generate([2, 2, 2], 3));
    }

    [Fact]
    public void Generate_SpecExample3()
    {
        Assert.Equal(new[] { 10, 10, 10, 30 }, Tribonacci.Generate([10, 10, 10], 4));
    }

    [Fact]
    public void Generate_SpecIllustration_OnesSequence()
    {
        Assert.Equal(new[] { 1, 1, 1, 3, 5, 9 }, Tribonacci.Generate([1, 1, 1], 6));
    }

    [Fact]
    public void Generate_SpecIllustration_CorrectedSecondExample()
    {
        // โจทย์ยกตัวอย่าง [2, 3, 1, 6, 9, 16] แต่ 3 + 1 + 6 = 10 ไม่ใช่ 9
        // ค่าที่ถูกต้องตามนิยามคือ [2, 3, 1, 6, 10, 17]
        Assert.Equal(new[] { 2, 3, 1, 6, 10, 17 }, Tribonacci.Generate([2, 3, 1], 6));
    }

    [Fact]
    public void Generate_EmptySignature_ProducesZeros()
    {
        Assert.Equal(new[] { 0, 0, 0, 0 }, Tribonacci.Generate([], 4));
    }

    [Fact]
    public void Generate_SingleOne_MatchesStandardTribonacci()
    {
        // เท่ากับ Tribonacci มาตรฐาน 0, 0, 1, 1, 2, 4, 7, 13, 24 ... (ไม่นับ 0, 0 ข้างหน้า)
        Assert.Equal(new[] { 1, 1, 2, 4, 7, 13, 24 }, Tribonacci.Generate([1], 7));
    }

    [Fact]
    public void Generate_TwoNumberSignature()
    {
        Assert.Equal(new[] { 3, 4, 7, 14, 25 }, Tribonacci.Generate([3, 4], 5));
    }

    [Fact]
    public void Generate_ThreeNumberSignatureWithZero()
    {
        Assert.Equal(new[] { 5, 2, 0, 7, 9 }, Tribonacci.Generate([5, 2, 0], 5));
    }

    [Fact]
    public void Generate_CountZero_ReturnsEmpty()
    {
        Assert.Empty(Tribonacci.Generate([1, 2, 3], 0));
    }

    [Fact]
    public void Generate_CountLessThanSignature_Truncates()
    {
        Assert.Equal(new[] { 1, 2 }, Tribonacci.Generate([1, 2, 3], 2));
    }

    [Fact]
    public void Generate_NegativeCount_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Tribonacci.Generate([1, 1, 1], -1));
    }

    [Fact]
    public void Generate_SignatureLongerThanThree_Throws()
    {
        Assert.Throws<ArgumentException>(() => Tribonacci.Generate([1, 1, 1, 1], 5));
    }

    [Fact]
    public void Generate_ValueExceedsInt_ThrowsOverflow()
    {
        // ใช้ checked จึง throw แทนการคืนค่าติดลบผิดๆ
        Assert.Throws<OverflowException>(() => Tribonacci.Generate([1, 1, 1], 100));
    }
}
