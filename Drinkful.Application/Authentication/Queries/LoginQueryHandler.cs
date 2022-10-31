using Drinkful.Application.Authentication.Common;
using Drinkful.Application.Common.Interfaces.Authentication;
using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Drinkful.Application.Authentication.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>> {
  private readonly IJwtGenerator _jwtGenerator;
  private readonly IUserRepository _userRepository;

  public LoginQueryHandler(IJwtGenerator jwtGenerator, IUserRepository userRepository) {
    _jwtGenerator = jwtGenerator;
    _userRepository = userRepository;
  }

  public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query,
    CancellationToken cancellationToken) {
    var user = _userRepository.GetByEmail(query.Email);
    if (user is null) {
      return Errors.Authentication.InvalidCredentials;
    }

    var passwordHasher = new PasswordHasher<string>();
    var result = passwordHasher.VerifyHashedPassword(
      user.Username, user.PasswordHash, query.Password);
    if (result == PasswordVerificationResult.Failed) {
      return Errors.Authentication.InvalidCredentials;
    }

    var token = _jwtGenerator.GenerateToken(user);
    return new AuthenticationResult(user, token);
  }
}