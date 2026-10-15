using EduHub.Ch02.Logging;
using EduHub.Ch02.Notifications;
using EduHub.Ch02.Patterns.Observers;
using EduHub.Ch02.Patterns.Strategies;
using EduHub.Ch02.Repositories;
using EduHub.Ch02.Services;

// ═══════════════════════════════════════════════════════════════
//  EduHub Ch02 — Refactoring với SOLID và 4 Design Patterns
//  (Factory, Decorator, Strategy, Observer)
// ═══════════════════════════════════════════════════════════════

// ═══ Composition root: nơi DUY NHẤT dùng "new" cho class cụ thể (DIP) ═══
IAppLogger logger = new ConsoleLogger(); // hoặc new FileLogger("club-log.txt");

// 1. Factory Pattern: tạo INotificationService theo NotificationChannel
INotificationServiceFactory notificationFactory = new NotificationServiceFactory();
INotificationService notifier = notificationFactory.Create(NotificationChannel.Email);

// 2. Decorator Pattern: LoggingClubRepository bọc InMemoryClubRepository để ghi log
IClubRepository clubRepo = new LoggingClubRepository(new InMemoryClubRepository(), logger);
IMemberRepository memberRepo = new InMemoryMemberRepository();
IEventRepository eventRepo = new InMemoryEventRepository();

// Dependency Injection qua Constructor (SRP, DIP)
IClubService clubService = new ClubService(clubRepo, memberRepo, notifier, logger);
IEventService eventService = new EventService(clubRepo, memberRepo, eventRepo);
IReportService reportService = new ReportService(clubRepo, memberRepo, eventRepo);

// ═══ 1. Nghiệp vụ tạo CLB & Thêm thành viên (SRP) ═══
Console.WriteLine("== 1. Tạo CLB và thêm thành viên ==");
var club = clubService.CreateClub("Tech Club", "Câu lạc bộ công nghệ", "president@gmail.com");
var a = clubService.AddMember(club.Id, "Nguyễn Văn A", "a@student.dthu.edu.vn");
var b = clubService.AddMember(club.Id, "Trần Thị B", "b@student.dthu.edu.vn");

// ═══ 2. Observer Pattern: đăng ký 2 Observer, phát sinh sự kiện ═══
Console.WriteLine("\n== 2. Observer: tạo sự kiện -> 2 observer được thông báo ==");
eventService.Subscribe(new LogNotifier(logger));
eventService.Subscribe(new EmailNotifier(memberRepo, notifier));
var ev = eventService.CreateEvent(club.Id, "Workshop SOLID", new DateTime(2026, 11, 20));
eventService.Register(ev.Id, a.Id);
eventService.Register(ev.Id, b.Id);
eventService.CheckIn(ev.Id, a.Id);

// ═══ 3. Strategy Pattern: cùng dữ liệu thống kê, xuất ra 2 format (Console & CSV) ═══
Console.WriteLine("\n== 3. Strategy: báo cáo Console ==");
IReportStrategy console = new ConsoleReportStrategy();
Console.Write(reportService.Generate(console));

Console.WriteLine("== 3. Strategy: báo cáo CSV ==");
IReportStrategy csv = new CsvReportStrategy();
Console.Write(reportService.Generate(csv));

// ═══ 4. Factory Pattern: đổi kênh thông báo linh hoạt (OCP) ═══
Console.WriteLine("\n== 4. Factory: gửi thông báo qua các kênh ==");
foreach (var channel in Enum.GetValues<NotificationChannel>())
    notificationFactory.Create(channel).Send("a@student.dthu.edu.vn", $"Thử nghiệm kênh {channel}");

// ═══ 5. Kiểm tra kiểm soát lỗi nghiệp vụ Domain ═══
Console.WriteLine("\n== 5. Kiểm tra validate nghiệp vụ (Domain) ==");
try
{
    clubService.CreateClub("", "mô tả", "p@gmail.com");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"  Bắt lỗi đúng như kỳ vọng: {ex.Message}");
}
