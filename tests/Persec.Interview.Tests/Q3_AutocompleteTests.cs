namespace Persec.Interview.Tests;

public class Q3_AutocompleteTests
{
    private static readonly string[] SpecItems = ["Mother", "Think", "Worthy", "Apple", "Android"];

    [Fact]
    public void Search_SpecExample()
    {
        Assert.Equal(new[] { "Think", "Mother" }, Autocomplete.Search("th", SpecItems, 2));
    }

    [Fact]
    public void Search_SpecExample_AllMatchesOrdered()
    {
        Assert.Equal(new[] { "Think", "Mother", "Worthy" }, Autocomplete.Search("th", SpecItems, 10));
    }

    [Fact]
    public void Search_OrdersStartThenMiddleThenEnd()
    {
        string[] items = ["Bath", "Mother", "Think"];
        Assert.Equal(new[] { "Think", "Mother", "Bath" }, Autocomplete.Search("th", items, 10));
    }

    [Fact]
    public void Search_IgnoresCase()
    {
        string[] items = ["MOTHER", "think"];
        Assert.Equal(new[] { "think", "MOTHER" }, Autocomplete.Search("Th", items, 10));
    }

    [Fact]
    public void Search_ExactMatch_CountsAsStart()
    {
        string[] items = ["Bath", "th"];
        Assert.Equal(new[] { "th", "Bath" }, Autocomplete.Search("th", items, 10));
    }

    [Fact]
    public void Search_KeepsOriginalOrderWithinSameGroup()
    {
        string[] items = ["Thumb", "Think", "Thank"];
        Assert.Equal(new[] { "Thumb", "Think", "Thank" }, Autocomplete.Search("th", items, 10));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Search_NonPositiveMaxResult_ReturnsEmpty(int maxResult)
    {
        Assert.Empty(Autocomplete.Search("th", SpecItems, maxResult));
    }

    [Fact]
    public void Search_EmptySearch_ReturnsEmpty()
    {
        Assert.Empty(Autocomplete.Search("", SpecItems, 5));
    }

    [Fact]
    public void Search_NoMatch_ReturnsEmpty()
    {
        Assert.Empty(Autocomplete.Search("xyz", SpecItems, 5));
    }

    [Fact]
    public void Search_SkipsNullItems()
    {
        string?[] items = ["Think", null, "Mother"];
        Assert.Equal(new[] { "Think", "Mother" }, Autocomplete.Search("th", items, 5));
    }
}
