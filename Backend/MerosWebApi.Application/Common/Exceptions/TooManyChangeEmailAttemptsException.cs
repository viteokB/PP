using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.Exceptions
{
    public class TooManyChangeEmailAttemptsException : AppException
    {
        public TooManyChangeEmailAttemptsException()
        {
        }

        public TooManyChangeEmailAttemptsException(string message) : base(message)
        {
        }

        public TooManyChangeEmailAttemptsException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
