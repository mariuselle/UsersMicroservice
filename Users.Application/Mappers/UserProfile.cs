using AutoMapper;
using System;
using Users.Application.Models;
using Users.Application.Services;
using Users.Core.Entities;

namespace Users.Application.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.Password, option => option.MapFrom(src => EncryptionService.EncryptSHA256(src.Password)))
                .ForMember(dest => dest.CreatedDate, option => option.MapFrom(src => DateTime.Now));
            CreateMap<UserLoginDto, User>()
                .ForMember(dest => dest.Password, option => option.MapFrom(src => EncryptionService.EncryptSHA256(src.Password)));
        }
    }
}
