using MerosWebApi.Core.Models.Mero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class TimePeriodShortResDto
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public static TimePeriodShortResDto Map(TimePeriod timePeriod)
        {
            return new TimePeriodShortResDto
            {
                StartTime = timePeriod.StartTime,
                EndTime = timePeriod.EndTime,
            };
        }
    }
}
