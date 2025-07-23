using CleanArchExample.Application.Interfaces.Services;

namespace CleanArchExample.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public Task SendAsync(string to, string subject, string body)
        {
            // Giả lập gửi email (ghi log hoặc console)
            Console.WriteLine($"[Email] To: {to} | Subject: {subject} | Body: {body}");
            return Task.CompletedTask;
        }
    }


}