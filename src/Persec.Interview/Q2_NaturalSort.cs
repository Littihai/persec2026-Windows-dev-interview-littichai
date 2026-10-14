namespace Persec.Interview;

/// <summary>
/// ข้อ 2: เรียง string แบบ "natural sort" — ส่วนที่เป็นตัวอักษรเทียบตามตัวอักษร
/// ส่วนที่เป็นตัวเลขเทียบตามค่าตัวเลข เช่น TH2 มาก่อน TH19 และ TH3Netflix อยู่ระหว่าง TH1 กับ TH7
/// </summary>
public static class NaturalSort
{
    /// <summary>
    /// คืน array ใหม่ที่เรียงแล้ว (ไม่แก้ไข array ต้นฉบับ)
    /// </summary>
    public static string[] Sort(IEnumerable<string> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        return items.OrderBy(x => x, NaturalStringComparer.Instance).ToArray();
    }
}

/// <summary>
/// IComparer สำหรับ natural sort แยกเป็น class เพื่อให้ reuse ได้กับ
/// List.Sort, OrderBy, SortedSet ฯลฯ
/// </summary>
/// <remarks>
/// วิธีเทียบ: เดินไปพร้อมกันทั้งสอง string ทีละ "ช่วง" (run)
/// <list type="bullet">
/// <item>ถ้าทั้งคู่เจอตัวเลข — ดึงตัวเลขทั้งช่วงมาเทียบค่ากัน โดยตัดเลข 0 นำหน้า
/// แล้วเทียบ "ความยาว" ก่อน ค่อยเทียบทีละหลัก จึงไม่ต้อง parse เป็น int
/// และไม่มีปัญหา overflow ไม่ว่าตัวเลขจะยาวแค่ไหน</item>
/// <item>ถ้าไม่ใช่ — เทียบตัวอักษรแบบไม่สนตัวพิมพ์เล็กใหญ่</item>
/// <item>ถ้าทุกช่วงเท่ากัน — string ที่สั้นกว่ามาก่อน (TH3 มาก่อน TH3Netflix)</item>
/// </list>
/// สุดท้ายถ้ายังเท่ากัน ใช้ ordinal เป็นตัวตัดสิน เพื่อให้ผลลัพธ์คงที่เสมอ (deterministic)
/// </remarks>
public sealed class NaturalStringComparer : IComparer<string>
{
    public static readonly NaturalStringComparer Instance = new();

    public int Compare(string? x, string? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        int i = 0, j = 0;

        while (i < x.Length && j < y.Length)
        {
            if (char.IsAsciiDigit(x[i]) && char.IsAsciiDigit(y[j]))
            {
                int result = CompareNumberRun(x, ref i, y, ref j);
                if (result != 0) return result;
            }
            else
            {
                int result = char.ToUpperInvariant(x[i]).CompareTo(char.ToUpperInvariant(y[j]));
                if (result != 0) return result;
                i++;
                j++;
            }
        }

        // ส่วนที่เหลือสั้นกว่า = มาก่อน
        int lengthResult = (x.Length - i).CompareTo(y.Length - j);
        if (lengthResult != 0) return lengthResult;

        return string.CompareOrdinal(x, y);
    }

    /// <summary>
    /// เทียบตัวเลขหนึ่งช่วงจากทั้งสอง string และเลื่อน index ไปท้ายช่วง
    /// </summary>
    private static int CompareNumberRun(string x, ref int i, string y, ref int j)
    {
        int xStart = i, yStart = j;
        while (i < x.Length && char.IsAsciiDigit(x[i])) i++;
        while (j < y.Length && char.IsAsciiDigit(y[j])) j++;

        ReadOnlySpan<char> xNum = x.AsSpan(xStart, i - xStart);
        ReadOnlySpan<char> yNum = y.AsSpan(yStart, j - yStart);

        ReadOnlySpan<char> xTrimmed = xNum.TrimStart('0');
        ReadOnlySpan<char> yTrimmed = yNum.TrimStart('0');

        // ตัวเลขที่มีหลักมากกว่า (หลังตัด 0 นำหน้า) มีค่ามากกว่าเสมอ
        int result = xTrimmed.Length.CompareTo(yTrimmed.Length);
        if (result != 0) return result;

        // จำนวนหลักเท่ากัน เทียบทีละหลักได้เลย
        result = xTrimmed.SequenceCompareTo(yTrimmed);
        if (result != 0) return Math.Sign(result);

        // ค่าเท่ากัน เช่น "2" กับ "02" — ให้ตัวที่มี 0 นำหน้าน้อยกว่ามาก่อน
        return xNum.Length.CompareTo(yNum.Length);
    }
}
