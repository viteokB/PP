using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.Exceptions
{
    public class TimeExpiredException : AppException
    {
        public TimeExpiredException()
        {
        }

        public TimeExpiredException(string message) : base(message)
        {
        }

        public TimeExpiredException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
