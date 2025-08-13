### Sử dụng `private set;` trong Clean Architecture hơn `required string`

Trong Clean Architecture, việc sử dụng `private set;` thường được ưu tiên hơn so với `required string` vì các lý do sau:

1. **Đảm bảo tính bất biến (Immutability):**
    - `private set;` cho phép bạn kiểm soát việc thay đổi giá trị của thuộc tính. Điều này giúp đảm bảo rằng đối tượng chỉ có thể được sửa đổi thông qua các phương thức hoặc logic cụ thể, tránh việc thay đổi không mong muốn từ bên ngoài.

    ```csharp
    public class User
    {
         public string Name { get; private set; }

         public User(string name)
         {
              Name = name ?? throw new ArgumentNullException(nameof(name));
         }

         public void UpdateName(string newName)
         {
              Name = newName ?? throw new ArgumentNullException(nameof(newName));
         }
    }
    ```

2. **Tăng tính đóng gói (Encapsulation):**
    - Với `private set;`, bạn có thể kiểm soát cách dữ liệu được cập nhật, giúp bảo vệ các quy tắc nghiệp vụ (business rules) trong domain.

3. **Hỗ trợ tốt hơn cho Domain-Driven Design (DDD):**
    - Trong DDD, các thực thể (entities) thường cần đảm bảo rằng trạng thái của chúng chỉ thay đổi thông qua các phương thức cụ thể. `private set;` giúp thực hiện điều này một cách rõ ràng.

4. **Khả năng mở rộng:**
    - Nếu sau này cần thêm logic khi thay đổi giá trị, bạn có thể dễ dàng thêm vào các phương thức mà không cần thay đổi cách truy cập thuộc tính.

Ngược lại, `required string` (C# 11) chỉ đảm bảo rằng thuộc tính được khởi tạo trong quá trình tạo đối tượng, nhưng không kiểm soát được việc thay đổi giá trị sau đó.

### Khi nào sử dụng `required string`?
`required string` có thể hữu ích trong các trường hợp đơn giản, khi bạn chỉ cần đảm bảo rằng thuộc tính được khởi tạo và không có logic nghiệp vụ phức tạp liên quan.

```csharp
public class User
{
     public required string Name { get; init; }
}
```

Tuy nhiên, trong các ứng dụng Clean Architecture hoặc DDD, `private set;` thường là lựa chọn tốt hơn để đảm bảo tính toàn vẹn của dữ liệu và tuân thủ các nguyên tắc thiết kế.


### Quy tắc “3 nấc” dễ nhớ

1. **DTO/Input contracts**  
    - Sử dụng `required { get; set; }` để đảm bảo các thuộc tính được khởi tạo đầy đủ khi nhận dữ liệu đầu vào.

    ```csharp
    public class CreateUserRequest
    {
          public required string Name { get; set; }
          public required string Email { get; set; }
    }
    ```

2. **Entity “chỉ-lưu-dữ-liệu” (không rules, không invariant)**  
    - Có thể sử dụng `required set;` để đơn giản hóa việc khởi tạo và cập nhật dữ liệu.

    ```csharp
    public class SimpleEntity
    {
          public required string Name { get; set; }
    }
    ```

3. **Entity có quy tắc (đa số bài toán thật)**  
    - Sử dụng `private set;` kết hợp với các phương thức có tên ý nghĩa để đảm bảo tuân thủ các quy tắc nghiệp vụ.

    ```csharp
    public class Order
    {
          public string Status { get; private set; }

          public Order(string initialStatus)
          {
                 Status = initialStatus ?? throw new ArgumentNullException(nameof(initialStatus));
          }

          public void UpdateStatus(string newStatus)
          {
                 if (string.IsNullOrEmpty(newStatus))
                      throw new ArgumentException("Status cannot be empty.", nameof(newStatus));

                 Status = newStatus;
          }
    }
    ```

    