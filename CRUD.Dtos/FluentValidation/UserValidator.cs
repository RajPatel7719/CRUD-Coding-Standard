using CRUD.Dtos.Request;
using FluentValidation;

namespace CRUD.Dtos.FluentValidation;
public class UserValidator : AbstractValidator<UserRequest>
{
	public UserValidator()
	{
		//RuleFor(x => x.JwtToken).NotNull().NotEmpty().Must((model, field) => field.DecodeJwtToken(model.JwtToken)).WithMessage("Jwt token and logged partner id is not matched");

		//RuleFor(x => x.JwtToken).NotNull().NotEmpty();

		RuleFor(x => x.FirstName).NotNull().NotEmpty();
		RuleFor(x => x.Email).NotNull().NotEmpty();
		RuleFor(x => x.Operation).NotNull().NotEmpty();
	}
}
