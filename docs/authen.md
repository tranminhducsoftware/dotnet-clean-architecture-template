# Luồng Nghiệp Vụ Chi Tiết Cho Tính Năng Authentication

## 1. Login
### Mô tả:
Người dùng nhập thông tin đăng nhập (username/password) để truy cập hệ thống.

### Luồng nghiệp vụ:
1. Người dùng gửi yêu cầu đăng nhập với thông tin `username` và `password`.
2. Hệ thống kiểm tra thông tin:
    - Nếu thông tin không hợp lệ, trả về lỗi (401 Unauthorized).
    - Nếu hợp lệ, tạo `AccessToken` và `RefreshToken`.
3. Trả về `AccessToken` và `RefreshToken` cho người dùng.

---

## 2. Logout
### Mô tả:
Người dùng đăng xuất khỏi hệ thống, vô hiệu hóa token.

### Luồng nghiệp vụ:
1. Người dùng gửi yêu cầu đăng xuất kèm `AccessToken` hoặc `RefreshToken`.
2. Hệ thống xác thực token:
    - Nếu token không hợp lệ, trả về lỗi (401 Unauthorized).
    - Nếu hợp lệ, xóa token khỏi hệ thống (hoặc đánh dấu là đã thu hồi).
3. Trả về thông báo thành công.

---

## 3. Refresh Token
### Mô tả:
Cấp mới `AccessToken` khi token cũ hết hạn mà không cần đăng nhập lại.

### Luồng nghiệp vụ:
1. Người dùng gửi yêu cầu làm mới token kèm `RefreshToken`.
2. Hệ thống kiểm tra `RefreshToken`:
    - Nếu không hợp lệ, trả về lỗi (401 Unauthorized).
    - Nếu hợp lệ, tạo mới `AccessToken` và (tùy chọn) `RefreshToken`.
3. Trả về token mới cho người dùng.

---

## 4. Các Tính Năng Authentication Khác
### 4.1. Đổi Mật Khẩu
1. Người dùng gửi yêu cầu đổi mật khẩu kèm mật khẩu cũ và mật khẩu mới.
2. Hệ thống xác thực mật khẩu cũ:
    - Nếu không hợp lệ, trả về lỗi (401 Unauthorized).
    - Nếu hợp lệ, cập nhật mật khẩu mới.
3. Trả về thông báo thành công.

### 4.2. Quên Mật Khẩu
1. Người dùng gửi yêu cầu quên mật khẩu kèm email.
2. Hệ thống gửi email chứa liên kết đặt lại mật khẩu.
3. Người dùng truy cập liên kết và đặt mật khẩu mới.
4. Hệ thống cập nhật mật khẩu và trả về thông báo thành công.

### 4.3. Xác Thực 2 Yếu Tố (2FA)
1. Người dùng kích hoạt 2FA, nhận mã OTP qua email/SMS.
2. Khi đăng nhập, người dùng nhập mã OTP.
3. Hệ thống xác thực OTP:
    - Nếu không hợp lệ, trả về lỗi (401 Unauthorized).
    - Nếu hợp lệ, cho phép truy cập.
