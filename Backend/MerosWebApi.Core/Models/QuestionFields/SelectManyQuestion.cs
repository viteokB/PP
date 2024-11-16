using MerosWebApi.Core.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MerosWebApi.Core.Models.Questions
{
    internal class SelectManyQuestion : Field, IHavePossibleAnswers
    {
        public SelectManyQuestion(string questionText, bool required, List<string> answers)
            : base(questionText, nameof(SelectManyQuestion), required)
        {
            if (answers.Count < 1)
                throw new FieldException($"Поле {nameof(SelectOneQuestion)} должно иметь как минимум один вариант");
            
            PossibleAnswers = answers;
        }

        public override List<string> Answers => PossibleAnswers;

        public void AddPossibleAnswer(string answer)
        {
            if (string.IsNullOrWhiteSpace(answer))
                throw new FieldException($"String should be not null, empty, or whitespace");

            PossibleAnswers.Add(answer);
        }

        public void RemovePossibleAnswer(string answer)
        {
            PossibleAnswers.Remove(answer);
        }

        public override List<string> SelectAnswer(params string[] answers)
        {
            if (answers.Length == 0)
                throw new FieldException("Should have more than zero possible answer");

            foreach (var answer in answers)
            {
                if(!answers.Any(ans => ans == answer))
                    throw new FieldException("Questions answers doesn't exists in possible answers");
            }
                

            return new List<string>() { answers[0] };
        }
    }
}
