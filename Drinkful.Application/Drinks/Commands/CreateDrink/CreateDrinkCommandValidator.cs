using FluentValidation;

namespace Drinkful.Application.Drinks.Commands.CreateDrink;

public class CreateDrinkCommandValidator : AbstractValidator<CreateDrinkCommand> {
  public CreateDrinkCommandValidator() {
    RuleFor(x => x.Name).NotEmpty();
    RuleFor(x => x.Summary).NotEmpty();
    RuleFor(x => x.Description).NotEmpty();
    RuleFor(x => x.ImageUrl).NotEmpty();
    RuleFor(x => x.AuthorId).NotEmpty().Length(36);
  }
}