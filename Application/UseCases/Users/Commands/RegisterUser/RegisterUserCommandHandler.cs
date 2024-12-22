using Application.Abstractions.Authorization;
using Application.Abstractions.Cqrs;
using Application.Common.Models;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
    {
        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        private IUnitOfWork _unitOfWork;
        private IPasswordHashEvaluator _passwordHashEvaluator;

        public RegisterUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork, IPasswordHashEvaluator passwordHashEvaluator)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHashEvaluator = passwordHashEvaluator;
            _roleRepository = roleRepository;
        }

        public async Task<BaseResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var (password, saltBase64) = _passwordHashEvaluator.HashPassword(request.Dto.Password!);

            request.Dto.Password = password;
            var user = request.Dto.ToUser(saltBase64);

            var roles = await _roleRepository.GetFilteredAsync<string>(null, new IFilter<Role>[] { });

            var isZeroUserRecords = !await _userRepository.AnyAsync(u => true, cancellationToken);

            // The very first created user is moderator
            // This is just to make the testing easy, and not an actual business logic since this is a test task.
            if (isZeroUserRecords)
            {
                user.RoleId = roles.Where(r => r.Code == RoleNames.Moderator).First().Id;
            }
            else
            {
                user.RoleId = roles.Where(r => r.Code == RoleNames.User).First().Id;
            }

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new BaseResponse();
        }
    }
}
