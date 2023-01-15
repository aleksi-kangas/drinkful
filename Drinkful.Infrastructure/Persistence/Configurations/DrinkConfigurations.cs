using Drinkful.Domain.Drink;
using Drinkful.Domain.Drink.Entities;
using Drinkful.Domain.Drink.ValueObjects;
using Drinkful.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Drinkful.Infrastructure.Persistence.Configurations;

public class DrinkConfigurations : IEntityTypeConfiguration<Drink> {
  public void Configure(EntityTypeBuilder<Drink> builder) {
    ConfigureDrinksTable(builder);
    ConfigureDrinkCommentsTable(builder);
  }

  private void ConfigureDrinksTable(EntityTypeBuilder<Drink> builder) {
    builder.ToTable("Drinks");
    builder.HasKey(d => d.Id);
    builder.Property(d => d.Id)
      .ValueGeneratedNever()
      .HasConversion(id => id.Value, value => DrinkId.Create(value));
    builder.Property(d => d.Name)
      .HasMaxLength(50);
    builder.Property(d => d.Summary)
      .HasMaxLength(50);
    builder.Property(d => d.ImageUrl)
      .HasMaxLength(500);
    builder.Property(d => d.AuthorId)
      .HasConversion(id => id.Value, value => UserId.Create(value));
  }

  private void ConfigureDrinkCommentsTable(EntityTypeBuilder<Drink> builder) {
    builder.OwnsMany(d => d.Comments, cb => {
      cb.ToTable("DrinkComments");
      cb.WithOwner().HasForeignKey("DrinkId");
      cb.HasKey(nameof(DrinkComment.Id), "DrinkId");
      cb.Property(c => c.Id)
        .HasColumnName("CommentId")
        .ValueGeneratedNever()
        .HasConversion(id => id.Value, value => DrinkCommentId.Create(value));
      cb.Property(s => s.Content)
        .HasMaxLength(256);
      cb.Property(s => s.AuthorId)
        .HasConversion(id => id.Value, value => UserId.Create(value));
    });

    builder.Metadata.FindNavigation(nameof(Drink.Comments))!
      .SetPropertyAccessMode(PropertyAccessMode.Field);
  }
}
