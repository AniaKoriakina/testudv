using FluentValidation;
using testudv.Application.Commands;
using testudv.Application.Services;

namespace testudv.Application.Validators;

public class GetPostsValidator : AbstractValidator<GetPostsCommand>
{
    public GetPostsValidator()
    {
        RuleFor(x => x.Domain)
            .NotEmpty().WithMessage("Домен не может быть пустым.")
            .Matches(@"^[a-zA-Z0-9._-]+$").WithMessage("Домен должен быть действительным.");

        RuleFor(x => x.Count)
            .GreaterThan(0).WithMessage("Количество постов должно быть больше 0.");
    }
}