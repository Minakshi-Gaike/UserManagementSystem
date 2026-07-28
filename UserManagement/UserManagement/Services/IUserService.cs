using UserManagement.DTOs;
using UserManagement.Models;
namespace UserManagement.Services
{
    public interface IUserService
    {
        Task AddUser(UserRegisterDTO u,string Password);
        Task<UserDTO> CheckLogin(UserLoginDTO login);
        Task ChangePassword(ChangePasswordDTO p);
    }
}
