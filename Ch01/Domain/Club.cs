namespace eduHub.Ch01.Domain;

/// <summary>
/// Đại diện cho một Câu lạc bộ sinh viên trong hệ thống EduHub.
/// </summary>
public class Club
{
    private readonly List<Member> _members = new();

    /// <summary>
    /// Mã định danh duy nhất của câu lạc bộ.
    /// </summary>
    public Guid Id {get; private set; }
    
    ///<sumary>
    /// Tên câu lạc bộ
    /// </sumary>
    public string Name { get; private set; }
    
    ///<sumary>
    /// Giới thiệu tóm tắt và mục tiêu hoạt động của câu lạc bộ
    /// </sumary>
    public string Description { get; private set; }
    
    ///<sumary>
    /// Ngày giờ thành lập CLB
    /// </sumary>
    public DateTime CreatedAt { get; private set; }
    
    ///<sumary>
    /// Danh sách thành viên câu lạc bộ
    /// </sumary>
    public IReadOnlyCollection<Member> Members => _members.AsReadOnly();
    
    ///<sumary>
    /// Constructor private đảm bảo tính đóng gói
    /// </sumary>
    private Club(Guid id, string name, string description, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = createdAt;
    }

    ///<sumary>
    /// Static Factory Method khởi tạo CLB mới.
    /// </sumary>
    public static Club Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Tên câu lạc bộ không được để trống!", nameof(name));
        }

        return new Club(
            Guid.NewGuid(),
            name.Trim(),
            description.Trim() ?? string.Empty,
            DateTime.Now);
    }
    
    /// <summary>
    /// Thêm thành viên vào CLB.
    /// </summary>
    public void AddMember(Member member)
    {
        ArgumentNullException.ThrowIfNull(member);

        // Kiểm tra chống trùng lặp thành viên
        if (_members.Any(m => m.Id == member.Id || m.StudentCode.Equals(member.StudentCode, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Sinh viên này đã là thành viên của CLB '{Name}'.");

        _members.Add(member);
    }
    
    /// <summary>
    /// Trả về số lượng thành viên hiện tại của CLB.
    /// </summary>
    public int MemberCount()
    {
        return _members.Count;
    }
}