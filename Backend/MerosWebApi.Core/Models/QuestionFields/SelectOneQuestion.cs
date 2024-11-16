using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Core.Models.Exceptions;
using Microsoft.VisualBasic;

namespace MerosWebApi.Core.Models.Questions
{
    public class SelectOneQuestion : Field, IHavePossibleAnswers
    {
        public SelectOneQuestion(string questionText, bool required, List<string> answers)
            : base(questionText, nameof(SelectOneQuestion), required)
        {
            if (answers.Count < 1)
                throw new FieldException($"Поле {nameof(SelectOneQuestion)} должно иметь как минимум один вариант");
            
            PossibleAnswers = answers;
        }

        public override List<string> Answers => PossibleAnswers;

        public void AddPossibleAnswer(string answer)
        {
            if(string.IsNullOrWhiteSpace(answer))
                throw new FieldException($"String should be not null, empty, or whitespace");

            PossibleAnswers.Add(answer);
        }

        public void RemovePossibleAnswer(string answer)
        {
            PossibleAnswers.Remove(answer);
        }

        public override List<string> SelectAnswer(params string[] answers)
        {
            if (answers.Length != 1)
                throw new FieldException("Should have only one answer");

            if(!PossibleAnswers.Any(ans => ans == answers[0]))
                throw new FieldException("Answer question doesn't exists in possible answers");

            return new List<string>() { answers[0] };
        }
    }
}
