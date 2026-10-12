namespace eduHub.Ch01.Domain;

/// <summary>
/// Đại diện cho một sinh viên / thành viên trong hệ thống EduHub.
/// </summary>
public class Member
{
    /// <summary>
    /// Mã định danh duy nhất của thành viên.
    /// </summary>
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Mã số sinh viên.
    /// </summary>
    public string StudentCode { get; private set; }
    
    /// <summary>
    /// Họ và tên của thành viên.
    /// </summary>
    public string FullName { get; private set; }
    
    /// <summary>
    /// Địa chỉ email liên hệ.
    /// </summary>
    public string Email { get; private set; }
    
    /// <summary>
    /// Thời điểm tham gia hệ thống.
    /// </summary>
    public DateTime JoineAt { get; private set; }
    
    /// <summary>
    /// Trạng thái hoạt động của tài khoản (true: đang hoạt động, false: tạm khóa/ngưng hoạt động).
    /// </summary>
    public bool IsActive { get; private set; }
    
    /// <summary>
    /// Constructor ở chế độ private để đảm bảo tính đóng gói (Encapsulation),
    /// ngăn chặn việc khởi tạo đối tượng tự do bằng toán tử new từ bên ngoài.
    /// </summary>
    private Member(Guid id, string studentCode, string fullName, string email, DateTime joineAt, bool isActive)
    {
        Id = id;
        StudentCode = studentCode;
        FullName = fullName;
        Email = email;
        JoineAt = joineAt;
        IsActive = isActive;
    }

    /// <summary>
    /// Static Factory Method khởi tạo thành viên mới có xác thực dữ liệu đầu vào.
    /// </summary>
    /// <param name="studentCode">Mã số sinh viên (không được rỗng).</param>
    /// <param name="fullName">Họ và tên sinh viên (không được rỗng).</param>
    /// <param name="email">Email sinh viên (phải hợp lệ và chứa '@').</param>
    /// <returns>Một đối tượng Member mới với trạng thái IsActive = true.</returns>
    /// <exception cref="ArgumentException">Ném ra khi dữ liệu đầu vào không hợp lệ.</exception>
    public static Member Create(string studentCode, string fullName, string email)
    {
        if (string.IsNullOrWhiteSpace(studentCode))
        {
            throw new ArgumentException(("Mã số sinh viên không được để trống!"), nameof(studentCode));
        }

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException(("Họ và tên không được để trống!"), nameof(fullName));
        }

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new ArgumentException(("Email không hợp lệ!"), nameof(email));
        }

        return new Member(
            Guid.NewGuid(),
            studentCode.Trim(),
            fullName.Trim(),
            email.Trim(),
            DateTime.Now,
            isActive: true
        );
    }

    /// <summary>
    /// Kích hoạt lại tài khoản thành viên.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }
    
    /// <summary>
    /// Tạm khóa hoặc vô hiệu hóa tài khoản thành viên.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }
}