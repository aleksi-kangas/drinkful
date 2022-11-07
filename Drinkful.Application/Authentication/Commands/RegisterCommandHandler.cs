using Drinkful.Application.Authentication.Common;
using Drinkful.Application.Common.Interfaces.Authentication;
using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.Common.Errors;
using Drinkful.Domain.User;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Drinkful.Application.Authentication.Commands;

public class RegisterCommandHandler :
  IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>> {
  private readonly IJwtGenerator _jwtGenerator;
  private readonly IUserRepository _userRepository;

  public RegisterCommandHandler(IJwtGenerator jwtGenerator, IUserRepository userRepository) {
    _jwtGenerator = jwtGenerator;
    _userRepository = userRepository;
  }

  public async Task<ErrorOr<AuthenticationResult>> Handle(
    RegisterCommand command,
    CancellationToken cancellationToken) {
    if (_userRepository.GetByEmail(command.Email) is not null) {
      return Errors.Authentication.DuplicateEmail;
    }

    if (_userRepository.GetByUsername(command.Username) is not null) {
      return Errors.Authentication.DuplicateUsername;
    }

    var passwordHasher = new PasswordHasher<string>();
    var passwordHash = passwordHasher.HashPassword(command.Username, command.Password);
    var user = User.Create(command.Username, command.Email, passwordHash);
    _userRepository.Create(user);
    var token = _jwtGenerator.GenerateToken(user);
    return new AuthenticationResult(user, token);
  }
}