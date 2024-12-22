using Application.Abstractions.Authorization;
using Application.Abstractions.Cqrs;
using Application.Common.Models;
using Application.Dtos.Outgoing;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Users.Queries.AuthorizeUser
{
    public class AuthorizeUserQueryHandler : IQueryHandler<AuthorizeUserQuery, AccessTokenResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashEvaluator _passwordHashEvaluator;
        private readonly ITokenGenerator _accessTokenGenerator;

        public AuthorizeUserQueryHandler(IUserRepository userRepository, IPasswordHashEvaluator passwordHashEvaluator, ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHashEvaluator = passwordHashEvaluator;
            _accessTokenGenerator = tokenGenerator;
        }


        public async Task<BaseResponse<AccessTokenResponse>> Handle(AuthorizeUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserWithRole(u => u.Username == request.Dto.Username, cancellationToken);

            if (user == null)
            {
                return new BaseResponse<AccessTokenResponse>(false, null);
            }

            var isPasswordMatch = _passwordHashEvaluator.PasswordMatch(request.Dto.Password!, user.Salt, user.Password);

            if (!isPasswordMatch)
            {
                return new BaseResponse<AccessTokenResponse>(false, null);
            }

            var token = _accessTokenGenerator.GenerateAccessToken(user.Username, user.Role.Code);

            return new BaseResponse<AccessTokenResponse>(true, new AccessTokenResponse(token));
        }
    }
}
