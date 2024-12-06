using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.Exceptions
{
    public class PhormAnswerNotFoundException : AppException
    {
        public PhormAnswerNotFoundException()
        {
        }

        public PhormAnswerNotFoundException(string message) : base(message)
        {
        }

        public PhormAnswerNotFoundException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
