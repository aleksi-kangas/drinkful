using Drinkful.Domain.User;

namespace Drinkful.Application.Common.Interfaces.Authentication;

public interface IJwtGenerator {
  string GenerateToken(User user);
}
