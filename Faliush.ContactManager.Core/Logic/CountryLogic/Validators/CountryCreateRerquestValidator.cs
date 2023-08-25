using Faliush.ContactManager.Core.Logic.CountryLogic.Queries;
using FluentValidation;

namespace Faliush.ContactManager.Core.Logic.CountryLogic.Validators;

public class CountryCreateRerquestValidator : AbstractValidator<CountryCreateRequest>
{
	public CountryCreateRerquestValidator()
	{
		RuleFor(x => x.Model.Name).NotNull().MaximumLength(40).MinimumLength(2);
	}
}
