# Persec 2026 – Windows Developer Pre-Interview

คำตอบแบบทดสอบก่อนสัมภาษณ์ตำแหน่ง Windows Developer เขียนด้วย C# (.NET 10) พร้อม unit test (xUnit)

ผู้ทำ: Littichai Yorach

## วิธีรัน

ต้องมี [.NET 10 SDK](https://dotnet.microsoft.com/download)

```bash
dotnet test
```

หรือเปิด `Persec.Interview.sln` ด้วย Visual Studio 2022 / Rider แล้วรันใน Test Explorer

## โครงสร้าง

```
src/Persec.Interview/            คำตอบ 1 ไฟล์ต่อ 1 ข้อ
  Q1_BracketValidator.cs         BracketValidator.IsBalanced
  Q2_NaturalSort.cs              NaturalSort.Sort, NaturalStringComparer
  Q3_Autocomplete.cs             Autocomplete.Search
  Q4_RomanNumeral.cs             RomanNumeral.ToRoman, RomanNumeral.FromRoman
  Q5_DigitSorter.cs              DigitSorter.SortDescending
  Q6_Tribonacci.cs               Tribonacci.Generate
tests/Persec.Interview.Tests/    test ของแต่ละข้อ — ตัวอย่างจากโจทย์ทุกข้อ + edge cases
```

## แนวคิดและสมมติฐานที่เลือก

บางข้อโจทย์ไม่ได้ระบุพฤติกรรมในบางกรณี จึงเลือกตามเหตุผลด้านล่าง และมี test ยืนยันทุกข้อ

### ข้อ 1 – วงเล็บครบคู่
- ใช้ `Stack<char>` — O(n) และไม่ใช้ recursion จึงรับ input ที่ซ้อนลึกมากได้ (มี test 100,000 ชั้น)
- string ว่าง → `true` (ไม่มีวงเล็บค้าง), ตัวอักษรอื่นที่ไม่ใช่วงเล็บ 6 ตัว หรือ `null` → `false`

### ข้อ 2 – เรียง string
- จากตัวอย่าง (`TH2` ก่อน `TH19`, `TH3Netflix` อยู่ระหว่าง `TH1` กับ `TH7`) จึงใช้ **natural sort**: ช่วงตัวอักษรเทียบแบบตัวอักษร ช่วงตัวเลขเทียบแบบค่าตัวเลข
- เทียบตัวเลขด้วยการตัด 0 นำหน้าแล้วเทียบจำนวนหลักก่อน **ไม่ parse เป็น int** จึงไม่ overflow แม้ตัวเลขยาวกว่า `long`
- เขียนเป็น `IComparer<string>` เพื่อนำไปใช้กับ `List.Sort`, `OrderBy`, `SortedSet` ได้
- ไม่สนตัวพิมพ์เล็กใหญ่ และไม่แก้ไข array ต้นฉบับ

### ข้อ 3 – Autocomplete
- จัดกลุ่มตามตำแหน่งที่พบคำค้นครั้งแรก: ขึ้นต้น → อยู่กลาง → อยู่ท้าย
- คำในกลุ่มเดียวกันคงลำดับเดิมของ `items` (ใช้ `OrderBy` ซึ่งเป็น stable sort) ตรงกับตัวอย่างที่ `Mother` มาก่อน `Worthy` (ทั้งสองคำมี `th` อยู่กลางคำ)
- search ว่าง หรือ `maxResult <= 0` → array ว่าง

### ข้อ 4 – เลขโรมัน
- รองรับช่วง 1–3999 (เลขโรมันมาตรฐานไม่มี 0 และเขียนได้สูงสุด 3999) นอกช่วง → `ArgumentOutOfRangeException`
- `ToRoman` ใช้ตาราง 13 ค่าที่รวมคู่ลบ (CM, CD, XC, …) ไว้แล้ว จึงเป็น greedy ล้วน
- `FromRoman` ตรวจรูปแบบด้วย **round-trip** — แปลงเป็นตัวเลขแล้วแปลงกลับ ถ้าไม่ตรงกับ input แปลว่าผิดรูปแบบ (เช่น `IIII`, `IC`, `VV`) → `FormatException` ได้การตรวจครบทุกกฎโดยไม่ต้องเขียนกฎแยกเอง
- มี test round-trip ครบทุกค่า 1–3999

### ข้อ 5 – เรียงหลักจากมากไปน้อย
- **รับ `int` แต่คืนค่าเป็น `long`** เพราะ input ที่ถูกต้องตามโจทย์ทำให้ผลลัพธ์เกิน `int.MaxValue` ได้ เช่น `1999999999 → 9999999991` และ C# ไม่ตรวจ overflow เป็นค่าเริ่มต้น ถ้าคืน `int` จะได้ค่าติดลบผิดๆ ออกมาเงียบๆ ส่วน `int` มีไม่เกิน 10 หลัก จึงไม่มีทางเกิน `long`
- ใช้ counting sort กับหลัก 0–9 — O(จำนวนหลัก) ไม่ต้องแปลงเป็น string
- ค่าติดลบ → `ArgumentOutOfRangeException`, 0 → 0

### ข้อ 6 – Tribonacci
- **ค่าเริ่มต้นน้อยกว่า 3 ตัว:** ตัวถัดไป = ผลรวมของค่าก่อนหน้าที่มีอยู่ไม่เกิน 3 ตัว เท่ากับการสมมติว่ามี 0 อยู่ก่อนลำดับ ทำให้ `[1]` ได้ `[1, 1, 2, 4, 7, 13, …]` ซึ่งตรงกับ Tribonacci มาตรฐาน (0, 0, 1, 1, 2, 4, 7, 13, …) และ `[]` ได้ `[0, 0, 0, …]`
- จำนวนที่ขอน้อยกว่าค่าเริ่มต้น → คืนเฉพาะตัวแรกๆ ตามจำนวน
- ลำดับโตแบบ exponential จะเกิน `int` เมื่อขอจำนวนมากพอไม่ว่าใช้ type ใด จึงใช้ `checked` ให้ throw `OverflowException` แทนการคืนค่าผิด
- **หมายเหตุ:** ตัวอย่าง `[2, 3, 1, 6, 9, 16]` ในโจทย์ไม่ตรงนิยาม เพราะ 3 + 1 + 6 = 10 ค่าที่ถูกต้องคือ `[2, 3, 1, 6, 10, 17]` (มี test ยืนยัน)
