namespace Persec.Interview;

/// <summary>
/// ข้อ 6: Tribonacci — แต่ละตัวคือผลรวมของ 3 ตัวก่อนหน้า
/// </summary>
public static class Tribonacci
{
    /// <summary>
    /// สร้างลำดับ Tribonacci จำนวน count ตัว (นับรวมค่าเริ่มต้น)
    /// </summary>
    /// <remarks>
    /// <para><b>ค่าเริ่มต้นน้อยกว่า 3 ตัว:</b> ตัวถัดไป = ผลรวมของค่าก่อนหน้าที่มีอยู่ ไม่เกิน 3 ตัว
    /// ซึ่งเท่ากับการสมมติว่ามีค่า 0 อยู่ก่อนลำดับ เช่น [1] → [1, 1, 2, 4, 7, 13, ...]
    /// ตรงกับ Tribonacci มาตรฐาน (0, 0, 1, 1, 2, 4, 7, 13, ...) และ [] → [0, 0, 0, ...]</para>
    /// <para><b>count น้อยกว่าจำนวนค่าเริ่มต้น:</b> คืนเฉพาะ count ตัวแรก</para>
    /// <para><b>Overflow:</b> ลำดับโตแบบ exponential (~1.84 เท่าต่อตัว) จะเกิน int เมื่อ count ใหญ่พอ
    /// ไม่ว่าจะใช้ type ใด จึงใช้ checked ให้ throw OverflowException ทันที แทนการคืนค่าผิดเงียบๆ</para>
    /// </remarks>
    /// <exception cref="ArgumentException">เมื่อค่าเริ่มต้นมีมากกว่า 3 ตัว</exception>
    /// <exception cref="ArgumentOutOfRangeException">เมื่อ count ติดลบ</exception>
    /// <exception cref="OverflowException">เมื่อค่าในลำดับเกิน int</exception>
    public static int[] Generate(IReadOnlyList<int> signature, int count)
    {
        ArgumentNullException.ThrowIfNull(signature);
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        if (signature.Count > 3)
            throw new ArgumentException("Signature must contain 0 to 3 numbers.", nameof(signature));

        var result = new int[count];

        for (int i = 0; i < count; i++)
        {
            if (i < signature.Count)
            {
                result[i] = signature[i];
                continue;
            }

            int sum = 0;
            for (int k = Math.Max(0, i - 3); k < i; k++)
            {
                sum = checked(sum + result[k]);
            }

            result[i] = sum;
        }

        return result;
    }
}
