using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.Exceptions
{
    public class EmailNotSentException : AppException
    {
        public EmailNotSentException()
        {
        }

        public EmailNotSentException(string message) : base(message)
        {
        }

        public EmailNotSentException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
