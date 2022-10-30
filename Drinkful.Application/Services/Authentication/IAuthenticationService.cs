using ErrorOr;

namespace Drinkful.Application.Services.Authentication; 

public interface IAuthenticationService {
  ErrorOr<AuthenticationResult> Login(string email, string password);
  
  ErrorOr<AuthenticationResult> Register(string username, string email, string password);
}