namespace Persec.Interview;

/// <summary>
/// ข้อ 1: ตรวจสอบว่าวงเล็บ ( ) [ ] { } เปิดปิดครบคู่และซ้อนกันถูกลำดับหรือไม่
/// </summary>
public static class BracketValidator
{
    /// <summary>
    /// คืน true เมื่อวงเล็บทุกตัวมีคู่ปิดที่ถูกชนิดและถูกลำดับ
    /// </summary>
    /// <remarks>
    /// ใช้ Stack: เจอวงเล็บเปิดให้ push, เจอวงเล็บปิดให้ pop แล้วเทียบว่าเป็นคู่เดียวกัน
    /// เมื่อจบ string แล้ว stack ต้องว่าง — Time O(n), Space O(n)
    /// <para>string ว่าง ถือว่าครบคู่ (ไม่มีวงเล็บค้าง)</para>
    /// <para>ถ้ามีตัวอักษรอื่นที่ไม่ใช่วงเล็บ 6 ตัวนี้ ถือว่า input ไม่ถูกต้อง คืน false</para>
    /// </remarks>
    public static bool IsBalanced(string? input)
    {
        if (input is null) return false;

        // ความยาวคี่ไม่มีทางจับคู่ได้ครบ — ตัดจบได้ทันที
        if (input.Length % 2 != 0) return false;

        var stack = new Stack<char>(input.Length / 2);

        foreach (char c in input)
        {
            switch (c)
            {
                case '(':
                case '[':
                case '{':
                    stack.Push(c);
                    break;

                case ')':
                case ']':
                case '}':
                    if (stack.Count == 0 || stack.Pop() != OpeningOf(c)) return false;
                    break;

                default:
                    return false;
            }
        }

        return stack.Count == 0;
    }

    private static char OpeningOf(char closing) => closing switch
    {
        ')' => '(',
        ']' => '[',
        _ => '{',
    };
}
