# Phân tích vi phạm SOLID trong ClubManager.cs

Tài liệu này ghi lại chi tiết các vi phạm nguyên lý SOLID trong file [ClubManager.cs](file:///Users/duongquocminhanh/RiderProjects/IN4211-ch02-solid-patterns-git/starter/Ch02.Refactor/ClubManager.cs) (phiên bản starter) cùng với hậu quả và đề xuất cách refactor.

---

## 1. Vi phạm SRP (Single Responsibility Principle - Nguyên lý đơn trách nhiệm)

- **Vị trí dòng:** 
  - Toàn bộ class `ClubManager` (Dòng 28–117), đặc biệt là method `CreateClub` (Dòng 35–64) và `AddMemberToClub` (Dòng 67–84).
- **Hiện trạng:**
  - `ClubManager` đóng vai trò là một "God class" ôm đồm quá nhiều trách nhiệm:
    1. Quản lý thông tin CLB và kiểm tra ràng buộc nghiệp vụ (validate tên CLB, sĩ số tối đa).
    2. Quản lý danh sách thành viên và sự kiện.
    3. Trực tiếp ghi log vào hệ thống tập tin đĩa (`club-log.txt`).
    4. Trực tiếp định dạng và gửi thông báo (in ra Console với logic phân nhánh domain email).
    5. Tính toán thống kê và xuất báo cáo.
- **Hậu quả:**
  - Class có quá nhiều lý do để thay đổi (thay đổi luật validate, đổi cách lưu trữ, đổi cơ chế log, đổi kênh gửi tin, đổi định dạng báo cáo).
  - Code bị ghép nối chặt (tight coupling), khó bảo trì, khó tái sử dụng và cực kỳ khó viết Unit Test độc lập cho từng chức năng. Sửa đổi một chức năng dễ gây lỗi ngoài ý muốn (side effects) cho chức năng khác.
- **Đề xuất cách sửa (Refactor):**
  - Đẩy các quy tắc nghiệp vụ cốt lõi vào tầng Domain: `Club.Create(...)` chịu trách nhiệm validate tên CLB, `Club.HasRoom(...)` kiểm tra giới hạn sĩ số.
  - Tách `ClubManager` thành các Service nhỏ theo từng ngữ cảnh nghiệp vụ:
    - `ClubService`: Quản lý nghiệp vụ câu lạc bộ.
    - `EventService`: Quản lý nghiệp vụ sự kiện.
    - `ReportService`: Quản lý nghiệp vụ tạo báo cáo.
  - Tách riêng cơ chế ghi log sang `IAppLogger` và cơ chế gửi thông báo sang `INotificationService`.

---

## 2. Vi phạm DIP (Dependency Inversion Principle - Nguyên lý đảo ngược phụ thuộc)

- **Vị trí dòng:**
  - Dòng 48, 79, 94, 111: `File.AppendAllText(_logFilePath, ...)`
  - Dòng 54, 58, 62, 82, 83, 97, 108: `Console.WriteLine(...)`
  - Dòng 30–31: Sử dụng trực tiếp `List<Club> _clubs` và `List<Member> _members` làm kho dữ liệu cứng trong bộ nhớ.
- **Hiện trạng:**
  - Module cấp cao (`ClubManager` chứa business logic) phụ thuộc trực tiếp vào các module/chi tiết I/O cấp thấp cụ thể (`System.IO.File`, `System.Console`) và cấu trúc lưu trữ nội bộ (`List<T>`).
- **Hậu quả:**
  - **Không thể viết Unit Test tự động:** Mỗi lần chạy test đều tạo/ghi đè file thật `club-log.txt` trên ổ đĩa, gây chậm và phụ thuộc môi trường I/O.
  - **Mất tính linh hoạt:** Không thể thay thế cơ chế ghi log (ví dụ chuyển sang NLog, Serilog, hay Cloud Logging) hoặc chuyển kho lưu trữ dữ liệu sang Database/ORM (SQL Server, PostgreSQL, Entity Framework) nếu không sửa hoặc viết lại toàn bộ class.
- **Đề xuất cách sửa (Refactor):**
  - Định nghĩa các interface trừu tượng:
    - `IAppLogger` với phương thức `Info(string message)`. Có thể cài đặt các lớp cụ thể: `ConsoleLogger`, `FileLogger`.
    - `IClubRepository`, `IMemberRepository`, `IEventRepository` để trừu tượng hóa việc truy xuất dữ liệu.
  - Áp dụng **Dependency Injection**: Tiêm (inject) các interface này vào Service thông qua Constructor (Constructor Injection). Service chỉ tương tác với Interface trừu tượng, không gọi trực tiếp `File` hay `Console`.

---

## 3. Vi phạm OCP (Open/Closed Principle - Nguyên lý Đóng/Mở)

- **Vị trí dòng:**
  - Dòng 52–63: Chuỗi `if/else` kiểm tra domain email (`@gmail.com`, `@yahoo.com`, generic) trong `CreateClub`.
  - Dòng 81–83: Hardcode gửi email và Slack notification trong `AddMemberToClub`.
  - Dòng 103–113: Hardcode xuất báo cáo trực tiếp ra màn hình Console trong `PrintClubReport`.
- **Hiện trạng:**
  - Hệ thống "mở cho việc sửa đổi và đóng cho việc mở rộng" (ngược hoàn toàn với OCP). Logic chọn kênh thông báo phụ thuộc vào chuỗi `if/else`, định dạng xuất báo cáo bị ghim cứng vào Console.
- **Hậu quả:**
  - Mỗi khi muốn hỗ trợ thêm kênh thông báo mới (SMS, Webhook, Zalo, Telegram...) hoặc muốn xuất báo cáo dưới dạng khác (CSV, PDF, JSON...), ta bắt buộc phải mở `ClubManager` ra và chỉnh sửa code cũ.
  - Làm tăng nguy cơ phá vỡ các chức năng đang hoạt động ổn định (regression bugs).
- **Đề xuất cách sửa (Refactor):**
  - **Áp dụng Factory Pattern:** Định nghĩa `INotificationService` và enum `NotificationChannel`. Tạo `NotificationServiceFactory` để khởi tạo dịch vụ thông báo tương ứng. Khi thêm kênh mới chỉ cần tạo class mới hiện thực `INotificationService` mà không cần sửa logic nghiệp vụ của `ClubService`.
  - **Áp dụng Strategy Pattern:** Định nghĩa `IReportStrategy` với method `Generate(...)`. Tách thành `ConsoleReportStrategy`, `CsvReportStrategy`. Khi cần xuất định dạng mới, chỉ cần thêm một Strategy mới mà không sửa đổi `ReportService`.

---

## 4. Vi phạm SRP & OCP khi phát sinh sự kiện (Tight Coupling với Notification)

- **Vị trí dòng:**
  - Dòng 88–99 (`CreateEvent`).
- **Hiện trạng:**
  - Khi một sự kiện được tạo, method `CreateEvent` vừa thực hiện tạo Event, vừa duyệt qua danh sách thành viên `_members` để gửi email thông báo:
    ```csharp
    foreach (var m in _members.Where(m => m.ClubId == clubId))
        Console.WriteLine($"Email to {m.Email}: new event '{title}' at {club.Name}");
    ```
- **Hậu quả:**
  - Nghiệp vụ tạo sự kiện bị gắn chặt (tightly coupled) với hành vi thông báo qua email.
  - Nếu sau này cần thêm các hành vi khác khi sự kiện được tạo (ví dụ: ghi Audit Log, gửi tin nhắn SMS, tạo lịch Google Calendar, gửi Push Notification), ta liên tục phải sửa phương thức `CreateEvent`.
- **Đề xuất cách sửa (Refactor):**
  - **Áp dụng Observer Pattern:**
    - Định nghĩa interface `IEventObserver` với phương thức `OnEventCreated(Event ev, Club club)`.
    - Tạo các observer cụ thể như `EmailNotifier`, `LogNotifier`.
    - `EventService` cho phép đăng ký danh sách Observer (`Subscribe(IEventObserver observer)`). Khi `CreateEvent` được gọi, `EventService` chỉ cần lặp qua các observer đã đăng ký và bắn thông báo (`observer.OnEventCreated(ev, club)`). `EventService` hoàn toàn không cần biết các observer xử lý chi tiết như thế nào.

---

## Bảng tổng hợp vi phạm & giải pháp kiến trúc

| STT | Vị trí dòng | Nguyên lý vi phạm | Hậu quả chính | Giải pháp Refactor (Mẫu thiết kế áp dụng) |
|---|---|---|---|---|
| **1** | 28–33, 35–64, 67–84 | **SRP** (Single Responsibility) | God class ôm đồm quá nhiều việc, khó bảo trì, khó unit test | Tách thành `ClubService`, `EventService`, `ReportService`; chuyển ràng buộc về Domain Entity (`Club`, `Member`) |
| **2** | 48, 79, 94, 111 | **DIP** (Dependency Inversion) | Phụ thuộc trực tiếp vào `System.IO.File`, `Console`, không mock được khi test | Định nghĩa `IAppLogger`, `IClubRepository`; tiêm qua Constructor Injection; dùng **Decorator Pattern** (`LoggingClubRepository`) để ghi log |
| **3** | 52–63, 81–83 | **OCP** (Open/Closed) | Thêm kênh thông báo mới phải sửa mã nguồn `ClubManager` (chuỗi `if/else`) | Sử dụng **Factory Pattern** (`NotificationServiceFactory`) tạo `INotificationService` đa hình |
| **4** | 103–113 | **SRP & OCP** | Ghim cứng báo cáo ra `Console`, không hỗ trợ định dạng khác | Sử dụng **Strategy Pattern** (`IReportStrategy`: `ConsoleReportStrategy`, `CsvReportStrategy`) |
| **5** | 88–99 | **SRP & OCP** | Nghiệp vụ tạo sự kiện bị ghép chặt với cơ chế gửi email cho thành viên | Sử dụng **Observer Pattern** (`IEventObserver`: `EmailNotifier`, `LogNotifier`) |
