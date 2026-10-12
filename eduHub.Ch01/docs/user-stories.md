EduHub -- Đặc tả yêu cầu người dùng (UserStory)
-------------
Nhóm 1: Tác nhân sinh viên (Student)

US-01: Sinh viên tham gia một CLB
1. Chưa là thành viên -> Tham gia -> Có tên trong danh sách
2. Đã là thành viên -> Tham gia lại -> Bị từ chối vì đã có tên trong danh sách

US-02: Sinh viên đăng ký tham gia sự kiện
1. Sự kiện còn chỗ -> Đăng ký -> Đăng ký thành công
2. Sự kiện hết chỗ -> Đăng ký -> Đăng ký không thành công (Hết chỗ)

US-03: Sinh viên huỷ phiếu tham gia sự kiện
1. Phiếu đăng ký ở trạng thái "Thành công" -> Huỷ đăng ký -> Phiếu đăng ký ở trạng thái "Đã huỷ"
2. Phiếu đăng ký ở trạng thái "Đã huỷ" -> Huỷ đăng ký lại -> Bị từ chối vì phiếu đăng kí hiện tại đang ở trạng thái "Đã huỷ"

---------------
Nhóm 2: Tác nhân Ban CLB (CLB Lead)

US-04: Ban CLB tạo sự kiện mới
1. Nhập thông tin hợp lệ và chỗ trống > 0 -> Tạo sự kiện -> Thêm thành công
2. Chỗ trống <= 0 -> Tạo sự kiện -> Bị từ chối vì chỗ trống không đủ

US-05: Ban CLB điểm danh sinh viên tại sự kiện
1. Phiếu đăng ký ở trạng thái "Thành công" -> Điểm danh -> Điểm danh thành công
2. Phiếu đăng ký ở trạng thái 'Đã huỷ' -> Điểm danh -> Điểm danh thất bại

----------------
Nhóm 3: Tác nhân Quản trị viên (Admin)

US-06: Admin kích hoạt lại trạng thái CLB
1. Hồ sơ CLB hợp lệ -> Phê duyệt -> Chuyển trạng thái "Đang hoạt động"
2. Hồ sơ CLB thiếu thông tin -> Phê duyệt -> Thất bị do thiếu thông tin