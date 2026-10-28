using System.Text;
using eduHub.Ch01.Domain;

Console.OutputEncoding = Encoding.UTF8;

const string separator = "============================================================";

Console.WriteLine(separator);
Console.WriteLine("         HỆ THỐNG QUẢN LÝ CLB & SỰ KIỆN SINH VIÊN EDUHUB    ");
Console.WriteLine(separator);
Console.WriteLine();

// ------------------------------------------------------------
// BƯỚC 1: Tạo CLB "Tech Club"
// ------------------------------------------------------------
Console.WriteLine(">>> BƯỚC 1: KHỞI TẠO CÂU LẠC BỘ");
var club = Club.Create("Tech Club", "CLB Công nghệ và Lập trình sinh viên ĐH Đồng Tháp");
Console.WriteLine($"[✓] Đã tạo CLB: {club.Name}");
Console.WriteLine($"    Mô tả     : {club.Description}");
Console.WriteLine($"    Ngày tạo  : {club.CreatedAt:dd/MM/yyyy HH:mm:ss}");
Console.WriteLine();

// ------------------------------------------------------------
// BƯỚC 2: Thêm 3 thành viên vào CLB
// ------------------------------------------------------------
Console.WriteLine(">>> BƯỚC 2: THÊM 3 THÀNH VIÊN VÀO CLB");
var member1 = Member.Create("DH24001", "Dương Quốc Minh Anh", "minhanh@gmail.com");
var member2 = Member.Create("DH24002", "Nguyễn Văn An", "an.nguyen@gmail.com");
var member3 = Member.Create("DH24003", "Trần Thị Bình", "binh.tran@gmail.com");

club.AddMember(member1);
club.AddMember(member2);
club.AddMember(member3);

Console.WriteLine($"[✓] Thêm thành viên thành công! Tổng số thành viên hiện tại: {club.MemberCount()}");
foreach (var m in club.Members)
{
    Console.WriteLine($"    - [{m.StudentCode}] {m.FullName} ({m.Email})");
}
Console.WriteLine();

// ------------------------------------------------------------
// BƯỚC 3: Tạo 1 sự kiện cho CLB (Workshop AI, sức chứa = 50)
// ------------------------------------------------------------
Console.WriteLine(">>> BƯỚC 3: TẠO SỰ KIỆN CHO CLB");
var workshopEvent = Event.Create(
    clubId: club.Id,
    title: "Workshop AI & Machine Learning Trong Kỷ Nguyên Mới",
    date: DateTime.Now.AddDays(7),
    capacity: 50,
    isOnline: false
);

Console.WriteLine($"[✓] Đã tạo sự kiện: {workshopEvent.Title}");
Console.WriteLine($"    Ngày diễn ra  : {workshopEvent.Date:dd/MM/yyyy HH:mm}");
Console.WriteLine($"    Sức chứa      : {workshopEvent.Capacity} chỗ");
Console.WriteLine($"    Hình thức     : {(workshopEvent.IsOnline ? "Online" : "Trực tiếp (Offline)")}");
Console.WriteLine();

// ------------------------------------------------------------
// BƯỚC 4: 2 thành viên đăng ký tham dự -> Status = Pending -> Gọi Confirm()
// ------------------------------------------------------------
Console.WriteLine(">>> BƯỚC 4: ĐĂNG KÝ VÀ XÁC NHẬN VÉ SỰ KIỆN");
var registrations = new List<Registration>();

// Thành viên 1 đăng ký
var reg1 = Registration.Create(member1.Id, workshopEvent.Id);
Console.WriteLine($"[+] {member1.FullName} gửi đăng ký -> Trạng thái: {reg1.Status}");
reg1.Confirm();
Console.WriteLine($"    [✓] Đã gọi Confirm() -> Trạng thái mới: {reg1.Status}");
registrations.Add(reg1);

// Thành viên 2 đăng ký
var reg2 = Registration.Create(member2.Id, workshopEvent.Id);
Console.WriteLine($"[+] {member2.FullName} gửi đăng ký -> Trạng thái: {reg2.Status}");
reg2.Confirm();
Console.WriteLine($"    [✓] Đã gọi Confirm() -> Trạng thái mới: {reg2.Status}");
registrations.Add(reg2);

// Kiểm tra sức chứa sau khi 2 người đăng ký
bool stillHasSlot = workshopEvent.HasCapacity(registrations.Count);
Console.WriteLine($"[i] Kiểm tra sự kiện còn chỗ trống không: {(stillHasSlot ? "Còn chỗ" : "Đã hết chỗ")}");
Console.WriteLine();

// ------------------------------------------------------------
// BƯỚC 5: 1 thành viên thực hiện check-in -> Tạo CheckIn record
// ------------------------------------------------------------
Console.WriteLine(">>> BƯỚC 5: ĐIỂM DANH THÀNH VIÊN TẠI SỰ KIỆN");
var checkIns = new List<CheckIn>();

// Thành viên 1 check-in bằng mã QR
var checkIn1 = CheckIn.Create(reg1.Id, CheckInMethod.QRCode);
checkIns.Add(checkIn1);

Console.WriteLine("[✓] Điểm danh thành công!");
Console.WriteLine($"    {checkIn1.Summary()}");
Console.WriteLine();

// ------------------------------------------------------------
// BƯỚC 6: In báo cáo tham dự
// ------------------------------------------------------------
Console.WriteLine(separator);
Console.WriteLine(">>> BƯỚC 6: BÁO CÁO TỔNG KẾT SỰ KIỆN");
Console.WriteLine(separator);
Console.WriteLine($"Sự kiện       : {workshopEvent.Title}");
Console.WriteLine($"Sức chứa tối đa: {workshopEvent.Capacity}");
Console.WriteLine($"Tổng số đăng ký: {registrations.Count}");
Console.WriteLine($"Tổng số check-in: {checkIns.Count}");
Console.WriteLine("Danh sách chi tiết người đăng ký:");

// Tạo danh sách tra cứu để in tên thành viên tương ứng với phiếu
var membersLookup = new[] { member1, member2, member3 }.ToDictionary(m => m.Id);

for (int i = 0; i < registrations.Count; i++)
{
    var reg = registrations[i];
    var member = membersLookup[reg.MemberId];
    bool isCheckedIn = checkIns.Any(c => c.RegistrationId == reg.Id);
    string checkInStatus = isCheckedIn ? "[ĐÃ CHECK-IN]" : "[CHƯA CHECK-IN]";

    Console.WriteLine($"  {i + 1}. {member.FullName} ({member.StudentCode}) - Trạng thái: {reg.Status} {checkInStatus}");
}
Console.WriteLine();

// ------------------------------------------------------------
// BƯỚC 7 (Bổ sung): Thử nghiệm vi phạm luật nghiệp vụ & Bắt ngoại lệ
// ------------------------------------------------------------
Console.WriteLine(separator);
Console.WriteLine(">>> BƯỚC 7: KIỂM THỬ RÀNG BUỘC NGHIỆP VỤ & BẮT NGOẠI LỆ");
Console.WriteLine(separator);

// Thử nghiệm 1: Xác nhận lại một phiếu đã hủy
try
{
    Console.WriteLine("[Thử nghiệm 1] Tạo phiếu mới, thực hiện Hủy (Cancel), sau đó cố tình gọi Confirm():");
    var testReg = Registration.Create(member3.Id, workshopEvent.Id);
    testReg.Cancel();
    Console.WriteLine($"    Phiếu đã hủy thành công: Status = {testReg.Status}");
    Console.WriteLine("    Đang cố gắng gọi testReg.Cancel() lần nữa...");
    testReg.Cancel(); // Vi phạm: hủy lại phiếu đã hủy
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"    [BẮT NGOẠI LỆ THÀNH CÔNG] Lỗi: {ex.Message}");
}

// Thử nghiệm 2: Cố tình thêm thành viên trùng lặp vào CLB
try
{
    Console.WriteLine("\n[Thử nghiệm 2] Cố tình thêm lại member1 vào CLB Tech Club lần thứ 2:");
    club.AddMember(member1); // Vi phạm: đã là thành viên
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"    [BẮT NGOẠI LỆ THÀNH CÔNG] Lỗi: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine(separator);
Console.WriteLine("            CHƯƠNG TRÌNH DEMO HOÀN TẤT THÀNH CÔNG!          ");
Console.WriteLine(separator);
