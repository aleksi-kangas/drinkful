using Drinkful.Domain.Entities;

namespace Drinkful.Application.Common.Interfaces.Authentication; 

public interface IJwtGenerator {
  string GenerateToken(User user);
}