namespace Persec.Interview;

/// <summary>
/// ข้อ 5: เรียงหลักของตัวเลขจากมากไปน้อย เช่น 3008 → 8300
/// </summary>
public static class DigitSorter
{
    /// <summary>
    /// คืนตัวเลขที่เกิดจากการเรียงหลักจากมากไปน้อย
    /// </summary>
    /// <remarks>
    /// <para><b>ทำไมคืนค่าเป็น long:</b> input ที่เป็น int ถูกต้องตามโจทย์ก็ทำให้ผลลัพธ์เกิน
    /// int.MaxValue ได้ เช่น 1999999999 → 9999999991 และการคำนวณใน C# ไม่ตรวจ overflow
    /// เป็นค่าเริ่มต้น ถ้าคืน int จะได้ค่าติดลบผิดๆ ออกมาเงียบๆ
    /// ส่วน int มีไม่เกิน 10 หลัก จึงไม่มีทางเกินช่วงของ long (19 หลัก)</para>
    /// <para>ใช้ counting sort กับหลัก 0–9 แทนการแปลงเป็น string แล้ว sort —
    /// Time O(d) เมื่อ d = จำนวนหลัก และไม่สร้าง object ชั่วคราว</para>
    /// <para>0 ไม่ใช่ positive แต่ไม่ทำให้เกิดความเสียหาย จึงยอมรับและคืน 0</para>
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">เมื่อ number ติดลบ</exception>
    public static long SortDescending(int number)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(number);

        Span<int> digitCounts = stackalloc int[10];

        do
        {
            digitCounts[number % 10]++;
            number /= 10;
        }
        while (number > 0);

        long result = 0;

        for (int digit = 9; digit >= 0; digit--)
        {
            for (int k = 0; k < digitCounts[digit]; k++)
            {
                result = result * 10 + digit;
            }
        }

        return result;
    }
}
