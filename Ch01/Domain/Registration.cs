namespace eduHub.Ch01.Domain;

/// <summary>
/// Các trạng thái hợp lệ của phiếu đăng ký (không dùng chuỗi rải rác).
/// </summary>
public enum RegistrationStatus
{
    Pending,
    Confirmed,
    Cancelled
}

/// <summary>
/// Đại diện cho phiếu đăng ký tham gia sự kiện của sinh viên.
/// </summary>
public class Registration
{
    /// <summary>
    /// Mã định danh duy nhất của phiếu đăng ký.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Mã sinh viên đăng ký.
    /// </summary>
    public Guid MemberId { get; private set; }

    /// <summary>
    /// Mã sự kiện được đăng ký.
    /// </summary>
    public Guid EventId { get; private set; }

    /// <summary>
    /// Thời điểm gửi đăng ký.
    /// </summary>
    public DateTime RegisteredAt { get; private set; }

    /// <summary>
    /// Trạng thái phiếu đăng ký dạng Enum (Pending, Confirmed, Cancelled).
    /// </summary>
    public RegistrationStatus Status { get; private set; }

    /// <summary>
    /// Constructor private đảm bảo tính đóng gói (Encapsulation).
    /// </summary>
    private Registration(Guid id, Guid memberId, Guid eventId, DateTime registeredAt, RegistrationStatus status)
    {
        Id = id;
        MemberId = memberId;
        EventId = eventId;
        RegisteredAt = registeredAt;
        Status = status;
    }

    /// <summary>
    /// Static Factory Method tạo phiếu đăng ký mới (mặc định trạng thái Confirmed).
    /// </summary>
    public static Registration Create(Guid memberId, Guid eventId)
    {
        if (memberId == Guid.Empty)
            throw new ArgumentException("Mã thành viên không được để trống.", nameof(memberId));

        if (eventId == Guid.Empty)
            throw new ArgumentException("Mã sự kiện không được để trống.", nameof(eventId));

        return new Registration(
            Guid.NewGuid(),
            memberId,
            eventId,
            DateTime.Now,
            RegistrationStatus.Pending
        );
    }

    /// <summary>
    /// Xác nhận phiếu đăng ký.
    /// </summary>
    public void Confirm()
    {
        if (Status == RegistrationStatus.Confirmed)
            throw new InvalidOperationException("Phiếu đăng ký đã ở trạng thái xác nhận (Confirmed).");

        Status = RegistrationStatus.Confirmed;
    }

    /// <summary>
    /// Hủy phiếu đăng ký (thỏa mãn US-03).
    /// </summary>
    public void Cancel()
    {
        if (Status == RegistrationStatus.Cancelled)
            throw new InvalidOperationException("Phiếu đăng ký này đã bị hủy trước đó (Cancelled).");

        Status = RegistrationStatus.Cancelled;
    }
}
