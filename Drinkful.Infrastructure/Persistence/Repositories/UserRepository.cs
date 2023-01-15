using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Drinkful.Infrastructure.Persistence.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository {
  public UserRepository(DrinkfulDbContext dbContext) : base(dbContext) { }

  public void Create(User user) {
    Add(user);
  }

  public async Task<User?> GetByEmail(string email, bool trackChanges) {
    return await FindByCondition(u => u.Email == email, trackChanges).FirstOrDefaultAsync();
  }

  public async Task<User?> GetByUsername(string username, bool trackChanges) {
    return await FindByCondition(u => u.Username == username, trackChanges).FirstOrDefaultAsync();
  }
}
