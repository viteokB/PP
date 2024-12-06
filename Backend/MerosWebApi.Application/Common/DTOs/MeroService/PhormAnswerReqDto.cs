using MerosWebApi.Core.Models.Mero;
using MerosWebApi.Core.Models.PhormAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class PhormAnswerReqDto
    {
        public string MeroId { get; set; }

        public List<AnswerReqDto> Answers { get; set; }

        public string TimePeriodId { get; set; }
    }
}
