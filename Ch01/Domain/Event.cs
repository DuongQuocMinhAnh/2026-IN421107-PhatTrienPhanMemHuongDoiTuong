namespace eduHub.Ch01.Domain;

public class Event
{
    /// <summary>
    /// Mã định danh của sự kiện 
    /// </summary>
    public Guid Id { get; private set; }
    
    /// <sumary>
    /// Mã định danh của CLB tổ chức sự kiện
    /// </sumary>
    public Guid ClubId { get; private set; }
    
    ///<sumary>
    /// Tiêu đề của sự kiện
    /// </sumary>
    public string Title { get; private set; }
    
    ///<sumary>
    /// Ngày tổ chức sự kiện
    /// </sumary>
    public DateTime Date { get; private set; }
    
    ///<sumary>
    /// Sức chứa của sự kiện
    ///</sumary>
    public int Capacity { get; private set; }
    
    /// <sumary>
    /// Hình thức tổ chức
    /// </sumary>
    public bool IsOnline { get; private set; }
    
    ///<sumary>
    /// Hàm khởi tạo private
    /// </sumary>
    private Event(Guid id, Guid clubId, string title, DateTime date, int capacity, bool isOnline)
    {
        Id = id;
        ClubId = clubId;
        Title = title;
        Date = date;
        Capacity = capacity;
        IsOnline = isOnline;
    }
    /// <summary>
    /// Static Factory Method tạo sự kiện mới có kiểm tra tính hợp lệ của dữ liệu đầu vào.
    /// </summary>
    public static Event Create(Guid clubId, string title, DateTime date, int capacity, bool isOnline)
    {
        if (clubId == Guid.Empty)
            throw new ArgumentException("Mã CLB không được để trống.", nameof(clubId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tiêu đề sự kiện không được để trống.", nameof(title));

        if (capacity <= 0)
            throw new ArgumentException("Sức chứa sự kiện phải lớn hơn 0.", nameof(capacity));

        return new Event(
            Guid.NewGuid(),
            clubId,
            title.Trim(),
            date,
            capacity,
            isOnline
        );
    }
    
    /// <summary>
    /// Kiểm tra xem sự kiện còn chỗ trống hay không dựa trên số lượng đã đăng ký.
    /// </summary>
    /// <param name="registeredCount">Số lượng người đã đăng ký hiện tại.</param>
    /// <returns>True nếu còn chỗ trống, False nếu đã đủ hoặc vượt sức chứa.</returns>
    public bool HasCapacity(int registeredCount)
    {
        return registeredCount < Capacity;
    }
}