# EduHub Ch03 — Đáp án mẫu (dành cho giảng viên)

Clean Architecture 4 lớp + REST API (Controller-based) cho Club, Swagger UI, unit test lớp Application bằng Moq. .NET 10, lưu trong bộ nhớ.

## Chạy
```bash
cd 02_Projects/ch03/solution
dotnet build EduHub.sln          # 0 lỗi, 0 cảnh báo
dotnet test                      # 5 passed
dotnet run --project EduHub.API  # http://localhost:5000/swagger
```
`launchSettings.json` đặt sẵn `http://localhost:5000` và môi trường Development (cần để Swagger bật).

## Demo nhanh (curl)
```bash
curl -i localhost:5000/api/clubs                                   # 200 []
curl -i -X POST localhost:5000/api/clubs -H 'Content-Type: application/json' \
     -d '{"name":"CLB Lập trình","description":"Học C# và .NET"}'  # 201 + Location
curl -i localhost:5000/api/clubs/{id}                              # 200 / 404
curl -i -X POST localhost:5000/api/clubs -H 'Content-Type: application/json' \
     -d '{"name":"","description":"x"}'                            # 400 + {"message": ...}
curl -i -X DELETE localhost:5000/api/clubs/{id}                    # 204 / 404
```

## Ghi chú cho GV
- Phạm vi Chương 3: kiểm tra đầu vào đơn giản trong handler (tên không rỗng, tối đa 50 ký tự) và trả status code đúng. ProblemDetails, Serilog, DTO/mapping, Repository/EF Core thuộc Chương 4.
- Repository đăng ký `AddScoped` đúng đề, nhưng danh sách `_clubs` là `static` (có lock); nếu không, mỗi request một bản mới nên POST xong GET vẫn rỗng. Đây là câu hỏi hay để hỏi SV.
- Starter chia sẵn Domain, `InMemoryMemberRepository` và 4 test; SV điền TODO 1 đến 8 và thêm test lấy CLB theo Id (bản này có 5 test).
- Chưa làm MembersController (bonus).
