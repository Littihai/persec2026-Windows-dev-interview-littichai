namespace Persec.Interview;

/// <summary>
/// ข้อ 3: Autocomplete — หาคำใน items ที่มี search เป็นส่วนหนึ่ง (ไม่สนตัวพิมพ์เล็กใหญ่)
/// แล้วเรียงตามตำแหน่งที่พบ: ขึ้นต้น → อยู่กลาง → อยู่ท้าย
/// </summary>
public static class Autocomplete
{
    private enum MatchPosition
    {
        Start = 0,
        Middle = 1,
        End = 2,
    }

    /// <remarks>
    /// <para>ใช้ตำแหน่งที่พบ search "ครั้งแรก" ในคำเป็นตัวจัดกลุ่ม
    /// คำที่เท่ากับ search พอดีนับเป็น "ขึ้นต้น"</para>
    /// <para>ใช้ OrderBy ซึ่งเป็น stable sort — คำที่อยู่กลุ่มเดียวกันจะคงลำดับเดิมของ items
    /// (ตรงกับตัวอย่างในโจทย์ที่ Mother มาก่อน Worthy)</para>
    /// <para>search ว่าง หรือ maxResult &lt;= 0 คืน array ว่าง; item ที่เป็น null จะถูกข้าม</para>
    /// </remarks>
    public static string[] Search(string search, IEnumerable<string?> items, int maxResult)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (string.IsNullOrEmpty(search) || maxResult <= 0) return [];

        return items
            .Select(item => (Item: item, Position: Classify(item, search)))
            .Where(x => x.Position is not null)
            .OrderBy(x => x.Position)
            .Take(maxResult)
            .Select(x => x.Item!)
            .ToArray();
    }

    /// <summary>
    /// คืนกลุ่มของคำ หรือ null ถ้าคำนั้นไม่มี search อยู่เลย
    /// </summary>
    private static MatchPosition? Classify(string? item, string search)
    {
        if (item is null) return null;

        int index = item.IndexOf(search, StringComparison.OrdinalIgnoreCase);

        if (index < 0) return null;
        if (index == 0) return MatchPosition.Start;
        if (index + search.Length == item.Length) return MatchPosition.End;
        return MatchPosition.Middle;
    }
}
