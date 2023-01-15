using Drinkful.Application.Common.Interfaces.Persistence;
using Drinkful.Domain.Drink;
using Drinkful.Domain.Drink.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Drinkful.Infrastructure.Persistence.Repositories;

public class DrinkRepository : RepositoryBase<Drink>, IDrinkRepository {
  public DrinkRepository(DrinkfulDbContext dbContext) : base(dbContext) { }

  public void Create(Drink drink) {
    Add(drink);
  }

  public async Task<Drink?> GetByIdAsync(DrinkId drinkId, bool trackChanges) {
    return await FindByCondition(d => d.Id == drinkId, false)
      .Include(d => d.Comments)
      .FirstOrDefaultAsync();
  }

  public async Task<IEnumerable<Drink>> GetAllAsync() {
    return await FindAll(false).ToListAsync();
  }
}
