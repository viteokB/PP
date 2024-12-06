using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Core.Models.PhormAnswer;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class AnswerResDto
    {
        public string QuestionText { get; set; }

        public List<string> QuestionAnswers { get; set; }

        public static AnswerResDto Map(Answer answer)
        {
            return new AnswerResDto
            {
                QuestionAnswers = answer.QuestionAnswers,
                QuestionText = answer.QuestionText
            };
        }
    }
}
