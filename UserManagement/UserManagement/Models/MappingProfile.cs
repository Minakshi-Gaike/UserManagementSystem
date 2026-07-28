using AutoMapper;
using UserManagement.DTOs;

namespace UserManagement.Models
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<TblUser, UserLoginDTO>();
            CreateMap<TblUser, UserRegisterDTO>();
            CreateMap<UserRegisterDTO, TblUser>();
            CreateMap<UserLoginDTO, TblUser>();
            CreateMap<TblUser, UserDTO>();
        }
    }
}
