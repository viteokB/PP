using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.Exceptions
{
    public class TimePeriodBusyException : AppException
    {
        public TimePeriodBusyException()
        {
        }

        public TimePeriodBusyException(string message) : base(message)
        {
        }

        public TimePeriodBusyException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
