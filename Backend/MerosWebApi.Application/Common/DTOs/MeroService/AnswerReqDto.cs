using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class AnswerReqDto
    {
        public string QuestionText { get; set; }

        public List<string> QuestionAnswers { get; set; }
    }
}
