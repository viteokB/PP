using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.ValidatorOptions
{
    public class UserValidationOptions
    {
        public int PasswordMinLength { get; set; }

        public int FullnameMinLength { get; set; }

        public int FullnameMaxLength { get; set; }
    }
}
