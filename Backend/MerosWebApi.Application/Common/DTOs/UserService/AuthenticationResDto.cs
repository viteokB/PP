using AutoMapper;
using MerosWebApi.Application.Common.Mapping;
using MerosWebApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.DTOs.UserService
{
    public class AuthenticationResDto : IMapWith<User>
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, AuthenticationResDto>()
                .ForMember(res => res.Id,
                    opt => opt.MapFrom(user => user.Id))
                .ForMember(res => res.FullName,
                    opt => opt.MapFrom(user => user.Full_name))
                .ForMember(res => res.Email,
                    opt => opt.MapFrom(user => user.Email));
        }
    }
}
