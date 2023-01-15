using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Drinkful.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDrinkData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Drinks",
                columns: new[] { "Id", "AuthorId", "CreatedAt", "Description", "ImageUrl", "Name", "Summary", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00b29fa1-38b7-4718-aef3-58390f6653d3"), new Guid("77a64a67-1cb1-4bb4-b415-df41e1bd42de"), new DateTime(2023, 1, 15, 20, 18, 15, 56, DateTimeKind.Utc).AddTicks(4758), "A cappuccino is an espresso-based coffee drink that originated in Austria and was later popularized in Italy and is prepared with steamed milk foam. Variations of the drink involve the use of cream instead of milk, using non-dairy milk substitutes and flavoring with cinnamon or chocolate powder.", "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c8/Cappuccino_at_Sightglass_Coffee.jpg/1024px-Cappuccino_at_Sightglass_Coffee.jpg", "Cappuccino", "Coffee drink", new DateTime(2023, 1, 15, 20, 18, 15, 56, DateTimeKind.Utc).AddTicks(4761) },
                    { new Guid("fc3eb9a0-c574-46f0-b715-93b6df8be3fb"), new Guid("1ed1ad02-0caa-470c-942c-462ffb41ecdc"), new DateTime(2023, 1, 15, 20, 18, 15, 56, DateTimeKind.Utc).AddTicks(4783), "A Moscow mule is a cocktail made with vodka, ginger beer and lime juice, garnished with a slice or wedge of lime and a sprig of mint. The drink is a type of buck and is sometimes called a vodka buck. The Moscow mule is popularly served in a copper mug, which takes on the cold temperature of the liquid.", "https://upload.wikimedia.org/wikipedia/commons/8/89/Cocktail_Moscow_Mule_im_Kupferbecher_mit_Minze.jpg", "Moscow mule", "Cocktail", new DateTime(2023, 1, 15, 20, 18, 15, 56, DateTimeKind.Utc).AddTicks(4784) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: new Guid("00b29fa1-38b7-4718-aef3-58390f6653d3"));

            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: new Guid("fc3eb9a0-c574-46f0-b715-93b6df8be3fb"));
        }
    }
}
