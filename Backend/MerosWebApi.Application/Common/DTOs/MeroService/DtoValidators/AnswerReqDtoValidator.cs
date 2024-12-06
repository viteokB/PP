using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace MerosWebApi.Application.Common.DTOs.MeroService.DtoValidators
{
    public class AnswerReqDtoValidator : AbstractValidator<AnswerReqDto>
    {
        public AnswerReqDtoValidator()
        {
            RuleFor(answer => answer.QuestionText)
                .NotEmpty().WithMessage("Поле вопроса должно иметь содержимое");

            RuleFor(answer => answer.QuestionAnswers)
                .Must((answer) => ValidateQuestionAnswer(answer))
                .WithMessage("Ответ вопроса должен быть либо null, либо содержать список не пустых строк");
        }

        private bool ValidateQuestionAnswer(List<string> answer)
        {
            if (answer == null)
                return true;
            if (answer.All(a => !string.IsNullOrWhiteSpace(a)))
                return true;

            return false;
        }
    }
}
