using MerosWebApi.Application.Common.SecurityHelpers;
using MerosWebApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Interfaces
{
    public interface ITokenGenerator
    {
        public string GenerateAccessToken(string userId);

        public RefreshToken GenerateRefreshToken();
    }
}
