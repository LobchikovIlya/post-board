

using FluentValidation;
using PostBoard.Api.Data;

namespace PostBoard.Api.Validators;

public class BirthdayValidator : AbstractValidator<Birthday>
{
    public BirthdayValidator()
    {
        RuleFor(birthday => birthday.UserFullName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(birthday => birthday.Date)
            .GreaterThanOrEqualTo(new DateTime(1, 1, 1))
            .LessThanOrEqualTo(DateTime.UtcNow.Date);
    }
}
