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
    public class GetDetailsResDto : IMapWith<User>
    {
        public Guid Id { get; set; }
        public string Full_name { get; set; }

        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public bool IsActive { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, GetDetailsResDto>()
                .ForMember(res => res.Full_name,
                    opt => opt.MapFrom(user => user.Full_name));

            profile.CreateMap<User, GetDetailsResDto>()
                .ForMember(res => res.Email,
                    opt => opt.MapFrom(user => user.Email));

            profile.CreateMap<User, GetDetailsResDto>()
                .ForMember(res => res.CreatedAt,
                    opt => opt.MapFrom(user => user.CreatedAt));

            profile.CreateMap<User, GetDetailsResDto>()
                .ForMember(res => res.UpdatedAt,
                    opt => opt.MapFrom(user => user.UpdatedAt));

            profile.CreateMap<User, GetDetailsResDto>()
                .ForMember(res => res.LastLoginAt,
                    opt => opt.MapFrom(user => user.LastLoginAt));

            profile.CreateMap<User, GetDetailsResDto>()
                .ForMember(res => res.IsActive,
                    opt => opt.MapFrom(user => user.IsActive));
        }
    }
}
