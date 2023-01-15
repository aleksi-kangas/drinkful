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
    
    InitialData(builder);
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

  private void InitialData(EntityTypeBuilder<Drink> builder) {
    builder.HasData(Drink.Create(
      "Cappuccino",
      "Coffee drink",
      "A cappuccino is an espresso-based coffee drink that originated in Austria and was later popularized in Italy and is prepared with steamed milk foam. Variations of the drink involve the use of cream instead of milk, using non-dairy milk substitutes and flavoring with cinnamon or chocolate powder.",
      "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c8/Cappuccino_at_Sightglass_Coffee.jpg/1024px-Cappuccino_at_Sightglass_Coffee.jpg",
      UserId.CreateUnique()));
    builder.HasData(Drink.Create(
      "Moscow mule",
      "Cocktail",
      "A Moscow mule is a cocktail made with vodka, ginger beer and lime juice, garnished with a slice or wedge of lime and a sprig of mint. The drink is a type of buck and is sometimes called a vodka buck. The Moscow mule is popularly served in a copper mug, which takes on the cold temperature of the liquid.",
      "https://upload.wikimedia.org/wikipedia/commons/8/89/Cocktail_Moscow_Mule_im_Kupferbecher_mit_Minze.jpg",
      UserId.CreateUnique()));
  }
}
