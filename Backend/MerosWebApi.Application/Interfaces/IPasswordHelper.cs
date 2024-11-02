using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Interfaces
{
    public interface IPasswordHelper
    {
        (byte[] passwordHash, byte[] passwordSalt) CreateHash(string password);

        bool VerifyHash(string password, byte[] hash, byte[] salt);

        string GenerateRandomString(int length);
    }
}
