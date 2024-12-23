using Domain.Abstractions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users.Commands.RegisterUser
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Dto.Username).NotNull().NotEmpty().MaximumLength(256);
            RuleFor(x => x.Dto.Password).NotNull().NotEmpty().MaximumLength(256);

            RuleFor(x => x.Dto.Username)
                .MustAsync(async (username, cancellationToken) =>
                {
                    var usernameExists = await userRepository.AnyAsync(u => u.Username == username, cancellationToken);
                    return !usernameExists; // Username should be unique (i.e. it should not exist in the db)
                })
                .WithErrorCode("Conflict")
                .WithMessage("Username already exists, please choose another one");
        }
    }
}
