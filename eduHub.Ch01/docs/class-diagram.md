# EduHub — Sơ đồ lớp miền nghiệp vụ (Domain Class Diagram)

Sơ đồ thể hiện 5 thực thể chính, 2 kiểu liệt kê (enum) cùng các thuộc tính đóng gói, Static Factory Method `Create(...)` và phương thức nghiệp vụ.

```mermaid
classDiagram
    class Member {
        +Guid Id
        +string StudentCode
        +string FullName
        +string Email
        +DateTime JoinedAt
        +bool IsActive
        +Create(studentCode, fullName, email)$ Member
        +Activate() void
        +Deactivate() void
    }

    class Club {
        +Guid Id
        +string Name
        +string Description
        +DateTime CreatedAt
        +IReadOnlyCollection~Member~ Members
        +Create(name, description)$ Club
        +AddMember(member) void
        +MemberCount() int
    }

    class Event {
        +Guid Id
        +Guid ClubId
        +string Title
        +DateTime Date
        +int Capacity
        +bool IsOnline
        +Create(clubId, title, date, capacity, isOnline)$ Event
        +HasCapacity(registeredCount) bool
    }

    class RegistrationStatus {
        <<enumeration>>
        Pending
        Confirmed
        Cancelled
    }

    class Registration {
        +Guid Id
        +Guid MemberId
        +Guid EventId
        +DateTime RegisteredAt
        +RegistrationStatus Status
        +Create(memberId, eventId)$ Registration
        +Confirm() void
        +Cancel() void
    }


    class CheckInMethod {
        <<enumeration>>
        QRCode
        Manual
    }

    class CheckIn {
        +Guid Id
        +Guid RegistrationId
        +DateTime CheckedInAt
        +CheckInMethod Method
        +Create(registrationId, method)$ CheckIn
        +Summary() string
    }


    Club "1" --> "*" Member : quan ly thanh vien
    Club "1" --> "*" Event : to chuc
    Event "1" --> "*" Registration : tiep nhan
    Member "1" --> "*" Registration : thuc hien
    Registration "1" --> "0..1" CheckIn : xac nhan tham gia
    Registration --> RegistrationStatus : trang thai
    CheckIn --> CheckInMethod : hinh thuc

