using Faliush.ContactManager.Core.Logic.PersonLogic.Queries;
using Faliush.ContactManager.Models.Base;
using FluentValidation;

namespace Faliush.ContactManager.Core.Logic.PersonLogic.Validators;

public class PersonCreateRequestValidator : AbstractValidator<PersonCreateRequest>
{
	public PersonCreateRequestValidator()
	{
		RuleFor(x => x.Model.FirstName).NotNull().MaximumLength(30).MinimumLength(2);
		RuleFor(x => x.Model.LastName).NotNull().MaximumLength(30).MinimumLength(2);
		RuleFor(x => x.Model.Email).EmailAddress();
		RuleFor(x => x.Model.Address).MaximumLength(80);
		RuleFor(x => x.Model.Gender).NotNull().IsInEnum();
		RuleFor(x => x.Model.DateOfBirth.Year).LessThan(DateTime.UtcNow.Year).GreaterThan(DateTime.UtcNow.Year - 100);
    }
}
