using Drinkful.Domain.User;
using Drinkful.Domain.User.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Drinkful.Infrastructure.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User> {
  public void Configure(EntityTypeBuilder<User> builder) {
    ConfigureUsersTable(builder);
    ConfigureUserDrinkIdsTable(builder);

    InitialData(builder);
  }

  private void ConfigureUsersTable(EntityTypeBuilder<User> builder) {
    builder.ToTable("Users");
    builder.HasKey(u => u.Id);
    builder.Property(u => u.Id)
      .ValueGeneratedNever()
      .HasConversion(id => id.Value, value => UserId.Create(value));
    builder.Property(u => u.Username)
      .HasMaxLength(30);
    builder.Property(u => u.Email)
      .HasMaxLength(30);
    builder.Property(u => u.PasswordHash)
      .HasMaxLength(500);
  }


  private void ConfigureUserDrinkIdsTable(EntityTypeBuilder<User> builder) {
    builder.OwnsMany(u => u.DrinkIds, dib => {
      dib.ToTable("UserDrinkIds");
      dib.WithOwner().HasForeignKey("UserId");
      dib.HasKey("Id");
      dib.Property(dId => dId.Value)
        .HasColumnName("DrinkId")
        .ValueGeneratedNever();
    });

    builder.Metadata.FindNavigation(nameof(User.DrinkIds))!
      .SetPropertyAccessMode(PropertyAccessMode.Field);
  }

  private void InitialData(EntityTypeBuilder<User> builder) {
    builder.HasData(User.Create(
      "example-user",
      "user@example.com",
      new PasswordHasher<string>().HashPassword("example-user", "password")));
  }
}
