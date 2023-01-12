using Drinkful.Domain.Drink;
using Drinkful.Domain.Drink.ValueObjects;
using Drinkful.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Drinkful.Infrastructure.Persistence.Configurations;

public class DrinkConfigurations : IEntityTypeConfiguration<Drink> {
  public void Configure(EntityTypeBuilder<Drink> builder) {
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


    builder.OwnsMany(d => d.CommentIds, cib => {
      cib.ToTable("DrinkCommentIds");
      cib.WithOwner().HasForeignKey("DrinkId");
      cib.HasKey("Id");
      cib.Property(c => c.Value)
        .HasColumnName("CommentId")
        .ValueGeneratedNever();
    });
    
    builder.Metadata.FindNavigation(nameof(Drink.CommentIds))!
      .SetPropertyAccessMode(PropertyAccessMode.Field);
  }
}
