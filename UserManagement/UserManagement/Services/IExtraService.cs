using UserManagement.Models;

namespace UserManagement.Services
{
    public interface IExtraService
    {
        Task<string> GeneratePassword(int size);
        Task <string> GenerateOTP(int size);
        Task SendEmail(EmailModel m);
    }
}
