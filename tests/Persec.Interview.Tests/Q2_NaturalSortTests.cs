namespace Persec.Interview.Tests;

public class Q2_NaturalSortTests
{
    [Fact]
    public void Sort_SpecExample1()
    {
        string[] input = ["TH19", "SG20", "TH2"];
        Assert.Equal(new[] { "SG20", "TH2", "TH19" }, NaturalSort.Sort(input));
    }

    [Fact]
    public void Sort_SpecExample2()
    {
        string[] input = ["TH10", "TH3Netflix", "TH1", "TH7"];
        Assert.Equal(new[] { "TH1", "TH3Netflix", "TH7", "TH10" }, NaturalSort.Sort(input));
    }

    [Fact]
    public void Sort_ShorterStringFirst_WhenPrefixEqual()
    {
        string[] input = ["TH3Netflix", "TH3"];
        Assert.Equal(new[] { "TH3", "TH3Netflix" }, NaturalSort.Sort(input));
    }

    [Fact]
    public void Sort_HandlesNumbersLargerThanLong()
    {
        // เทียบตัวเลขด้วยความยาว ไม่ได้ parse จึงไม่ overflow
        string[] input = ["A100000000000000000000000", "A99999999999999999999999", "A2"];
        Assert.Equal(
            new[] { "A2", "A99999999999999999999999", "A100000000000000000000000" },
            NaturalSort.Sort(input));
    }

    [Fact]
    public void Sort_LeadingZeros_ComparedByValue()
    {
        string[] input = ["TH010", "TH9", "TH02"];
        Assert.Equal(new[] { "TH02", "TH9", "TH010" }, NaturalSort.Sort(input));
    }

    [Fact]
    public void Sort_IsCaseInsensitive()
    {
        string[] input = ["th10", "TH2"];
        Assert.Equal(new[] { "TH2", "th10" }, NaturalSort.Sort(input));
    }

    [Fact]
    public void Sort_DoesNotMutateInput()
    {
        string[] input = ["TH19", "TH2"];
        NaturalSort.Sort(input);
        Assert.Equal(new[] { "TH19", "TH2" }, input);
    }

    [Fact]
    public void Sort_EmptyInput_ReturnsEmpty()
    {
        Assert.Empty(NaturalSort.Sort([]));
    }

    [Fact]
    public void Comparer_IsConsistent()
    {
        var comparer = NaturalStringComparer.Instance;
        Assert.True(comparer.Compare("TH2", "TH19") < 0);
        Assert.True(comparer.Compare("TH19", "TH2") > 0);
        Assert.Equal(0, comparer.Compare("TH2", "TH2"));
    }
}
