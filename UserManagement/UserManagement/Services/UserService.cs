using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Services
{
    public class UserService : IUserService
    {
        UserDbContext db;
        IExtraService extraService;
        IMapper mapper;
        public UserService(UserDbContext db,IExtraService extraService,IMapper mapper)
        {
           this.db = db;
            this.extraService = extraService;
            this.mapper = mapper;
        }
        public async Task AddUser (UserRegisterDTO u, string Password)
        {
            TblUser user = new TblUser()
            {
                UserName = u.UserName,
                EmailAddress = u.EmailAddress,
                ProfilePhoto = u.ProfilePhoto,
                Password = Password,
                Gender = u.Gender,
                BirthDate = u.BirthDate,
                MobileNumber = u.MobileNumber,
                CreatedAt = DateTime.Now,
                RegistrationDate = DateTime.Now,
               IsActive=0,
               Flag=1

            };
           await db.TblUsers.AddAsync(user);
            await db.SaveChangesAsync();
        }
        public async Task<UserDTO> CheckLogin(UserLoginDTO login)
        {
            UserDTO user = null;
            TblUser u = await db.TblUsers.FirstOrDefaultAsync(e => e.EmailAddress == login.EmailAddress && e.Password == login.Password);
            //manual mapping....
            //if (u != null)
            //{
            //    user = new UserDTO()
            //    {

            //        UserId = u.UserId,
            //        BirthDate = u.BirthDate,
            //        EmailAddress = u.EmailAddress,
            //        Gender = u.Gender,
            //        MobileNumber = u.MobileNumber,
            //        ProfilePhoto = u.ProfilePhoto,
            //        UserName = u.UserName,
            //    };
            //}
            //return user;

            if (u == null)
            {
                return null;
            }
            return mapper.Map<UserDTO>(u);

        }

        public async Task ChangePassword(ChangePasswordDTO p)
        {
            TblUser user =await db.TblUsers.FindAsync(p.UserID);
            if(user!=null)
            {
                user.Password = p.NewPassword;
                await db.SaveChangesAsync();
            }
           

        }

        

        
    }
}
