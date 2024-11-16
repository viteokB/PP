using MerosWebApi.Core.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MerosWebApi.Core.Models.Questions
{
    public class ShortTextQuestion : Field
    {
        public ShortTextQuestion(string text, bool required, List<string> answers)
            : base(text, nameof(ShortTextQuestion), required)
        {
            if (answers != null)
                throw new FieldException("Should have no answers");
        }

        public override List<string> SelectAnswer(params string[] answers)
        {
            if (answers.Length != 1)
                throw new FieldException("Should have only one answer");

            return new List<string>() { answers[0] };
        }

        public override List<string> Answers { get; }
    }
}
