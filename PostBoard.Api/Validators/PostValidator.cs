using FluentValidation;
using PostBoard.Api.Models;

namespace PostBoard.Api.Validators;

public class PostValidator : AbstractValidator<Post>
{
    public PostValidator()
    {
        RuleFor(post => post.Title).NotEmpty().MaximumLength(50);

        RuleFor(post => post.Body).NotEmpty().MaximumLength(1000);
    }
}