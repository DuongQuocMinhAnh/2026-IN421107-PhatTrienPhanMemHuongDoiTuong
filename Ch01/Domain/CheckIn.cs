namespace eduHub.Ch01.Domain;

/// <summary>
/// Các hình thức điểm danh hợp lệ tại sự kiện.
/// </summary>
public enum CheckInMethod
{
    QRCode,
    Manual
}

/// <summary>
/// Đại diện cho bản ghi điểm danh sự kiện theo đúng bảng đặc tả đề bài.
/// </summary>
public class CheckIn
{
    /// <summary>
    /// Mã định danh duy nhất của bản ghi điểm danh.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Mã phiếu đăng ký được điểm danh.
    /// </summary>
    public Guid RegistrationId { get; private set; }

    /// <summary>
    /// Thời điểm thực hiện điểm danh.
    /// </summary>
    public DateTime CheckedInAt { get; private set; }

    /// <summary>
    /// Hình thức điểm danh dạng Enum (QRCode hoặc Manual).
    /// </summary>
    public CheckInMethod Method { get; private set; }

    /// <summary>
    /// Constructor private đảm bảo tính đóng gói (Encapsulation).
    /// </summary>
    private CheckIn(Guid id, Guid registrationId, DateTime checkedInAt, CheckInMethod method)
    {
        Id = id;
        RegistrationId = registrationId;
        CheckedInAt = checkedInAt;
        Method = method;
    }

    /// <summary>
    /// Static Factory Method tạo bản ghi điểm danh mới có kiểm tra dữ liệu hợp lệ.
    /// </summary>
    public static CheckIn Create(Guid registrationId, CheckInMethod method)
    {
        if (registrationId == Guid.Empty)
            throw new ArgumentException("Mã phiếu đăng ký không được để trống.", nameof(registrationId));

        return new CheckIn(
            Guid.NewGuid(),
            registrationId,
            DateTime.Now,
            method
        );
    }

    /// <summary>
    /// Trả về chuỗi mô tả tóm tắt thông tin điểm danh (theo đúng yêu cầu đề bài).
    /// </summary>
    public string Summary()
    {
        return $"[CheckIn] Phiếu: {RegistrationId.ToString()[..8]}... | Thời gian: {CheckedInAt:HH:mm:ss dd/MM/yyyy} | Hình thức: {Method}";
    }
}