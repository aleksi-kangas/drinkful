namespace Drinkful.Application.Services.Authentication; 

public interface IAuthenticationService {
  AuthenticationResult Login(string email, string password);
  
  AuthenticationResult Register(string username, string email, string password);
}