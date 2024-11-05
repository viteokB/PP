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
    public class RegisterReqDto
    {
        public string Full_name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
