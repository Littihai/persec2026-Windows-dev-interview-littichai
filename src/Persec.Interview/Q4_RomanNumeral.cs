namespace Persec.Interview;

/// <summary>
/// ข้อ 4: แปลงเลขฐานสิบ ↔ เลขโรมันแบบ Modern Roman Numerals (ช่วง 1–3999)
/// </summary>
public static class RomanNumeral
{
    public const int MinValue = 1;
    public const int MaxValue = 3999;

    // เรียงจากมากไปน้อย รวมคู่ลบ (CM, CD, XC, XL, IX, IV) ไว้ในตารางเลย
    // เพื่อให้การแปลงเป็นแบบ greedy ล้วนๆ ไม่ต้องมี if พิเศษ
    private static readonly (int Value, string Symbol)[] Table =
    [
        (1000, "M"), (900, "CM"), (500, "D"), (400, "CD"),
        (100, "C"), (90, "XC"), (50, "L"), (40, "XL"),
        (10, "X"), (9, "IX"), (5, "V"), (4, "IV"),
        (1, "I"),
    ];

    /// <summary>
    /// แปลง int เป็นเลขโรมัน เช่น 1989 → "MCMLXXXIX"
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">เมื่อค่าอยู่นอกช่วง 1–3999
    /// (เลขโรมันไม่มี 0 และสัญลักษณ์มาตรฐานเขียนได้สูงสุด 3999)</exception>
    public static string ToRoman(int number)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(number, MinValue);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(number, MaxValue);

        var builder = new System.Text.StringBuilder();

        foreach (var (value, symbol) in Table)
        {
            while (number >= value)
            {
                builder.Append(symbol);
                number -= value;
            }
        }

        return builder.ToString();
    }

    /// <summary>
    /// แปลงเลขโรมันเป็น int เช่น "MCMLXXXIX" → 1989 (รับได้ทั้งตัวพิมพ์เล็กและใหญ่)
    /// </summary>
    /// <remarks>
    /// อ่านจากซ้ายไปขวา ถ้าสัญลักษณ์ถัดไปมีค่ามากกว่าตัวปัจจุบันให้ "ลบ" ไม่อย่างนั้นให้ "บวก"
    /// <para>ตรวจรูปแบบด้วยการแปลงกลับ (round-trip): ถ้า ToRoman(ผลลัพธ์) ไม่ตรงกับ input
    /// แปลว่าเขียนผิดรูปแบบ เช่น "IIII", "IC", "VV" — ได้การตรวจที่ครบทุกกฎ
    /// โดยไม่ต้องเขียนกฎทีละข้อเอง</para>
    /// </remarks>
    /// <exception cref="FormatException">เมื่อ input ไม่ใช่เลขโรมันที่ถูกต้อง</exception>
    public static int FromRoman(string roman)
    {
        if (string.IsNullOrWhiteSpace(roman))
            throw new FormatException("Roman numeral must not be empty.");

        string normalized = roman.Trim().ToUpperInvariant();
        int total = 0;

        for (int i = 0; i < normalized.Length; i++)
        {
            int current = ValueOf(normalized[i]);
            int next = i + 1 < normalized.Length ? ValueOf(normalized[i + 1]) : 0;

            total += current < next ? -current : current;
        }

        if (total < MinValue || total > MaxValue || ToRoman(total) != normalized)
            throw new FormatException($"'{roman}' is not a valid Roman numeral.");

        return total;
    }

    private static int ValueOf(char symbol) => symbol switch
    {
        'I' => 1,
        'V' => 5,
        'X' => 10,
        'L' => 50,
        'C' => 100,
        'D' => 500,
        'M' => 1000,
        _ => throw new FormatException($"'{symbol}' is not a Roman numeral symbol."),
    };
}
