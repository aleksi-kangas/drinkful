using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Drinkful.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: new Guid("00b29fa1-38b7-4718-aef3-58390f6653d3"));

            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: new Guid("fc3eb9a0-c574-46f0-b715-93b6df8be3fb"));

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDrinkIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DrinkId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDrinkIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDrinkIds_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Drinks",
                columns: new[] { "Id", "AuthorId", "CreatedAt", "Description", "ImageUrl", "Name", "Summary", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("6916a283-6a6a-4f61-a7ca-8d9d4330053c"), new Guid("8276556f-de98-47c6-a106-056e86ac7fa1"), new DateTime(2023, 1, 15, 20, 59, 44, 881, DateTimeKind.Utc).AddTicks(6082), "A cappuccino is an espresso-based coffee drink that originated in Austria and was later popularized in Italy and is prepared with steamed milk foam. Variations of the drink involve the use of cream instead of milk, using non-dairy milk substitutes and flavoring with cinnamon or chocolate powder.", "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c8/Cappuccino_at_Sightglass_Coffee.jpg/1024px-Cappuccino_at_Sightglass_Coffee.jpg", "Cappuccino", "Coffee drink", new DateTime(2023, 1, 15, 20, 59, 44, 881, DateTimeKind.Utc).AddTicks(6084) },
                    { new Guid("c3e050cb-f237-4212-b2a5-be74b85f5b6a"), new Guid("bcc255c0-ea46-4913-8fc5-59d9521e5ba2"), new DateTime(2023, 1, 15, 20, 59, 44, 881, DateTimeKind.Utc).AddTicks(6105), "A Moscow mule is a cocktail made with vodka, ginger beer and lime juice, garnished with a slice or wedge of lime and a sprig of mint. The drink is a type of buck and is sometimes called a vodka buck. The Moscow mule is popularly served in a copper mug, which takes on the cold temperature of the liquid.", "https://upload.wikimedia.org/wikipedia/commons/8/89/Cocktail_Moscow_Mule_im_Kupferbecher_mit_Minze.jpg", "Moscow mule", "Cocktail", new DateTime(2023, 1, 15, 20, 59, 44, 881, DateTimeKind.Utc).AddTicks(6105) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "Username" },
                values: new object[] { new Guid("7952a877-84db-48d1-8838-990c7908af6c"), "user@example.com", "AQAAAAIAAYagAAAAEFHe5GZ+w410L0GxF1QF7MSuZ7jYUWzemupL097swbVDdMfqM//Vu1WOe7QwESL4qw==", "example-user" });

            migrationBuilder.CreateIndex(
                name: "IX_UserDrinkIds_UserId",
                table: "UserDrinkIds",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDrinkIds");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: new Guid("6916a283-6a6a-4f61-a7ca-8d9d4330053c"));

            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: new Guid("c3e050cb-f237-4212-b2a5-be74b85f5b6a"));

            migrationBuilder.InsertData(
                table: "Drinks",
                columns: new[] { "Id", "AuthorId", "CreatedAt", "Description", "ImageUrl", "Name", "Summary", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("00b29fa1-38b7-4718-aef3-58390f6653d3"), new Guid("77a64a67-1cb1-4bb4-b415-df41e1bd42de"), new DateTime(2023, 1, 15, 20, 18, 15, 56, DateTimeKind.Utc).AddTicks(4758), "A cappuccino is an espresso-based coffee drink that originated in Austria and was later popularized in Italy and is prepared with steamed milk foam. Variations of the drink involve the use of cream instead of milk, using non-dairy milk substitutes and flavoring with cinnamon or chocolate powder.", "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c8/Cappuccino_at_Sightglass_Coffee.jpg/1024px-Cappuccino_at_Sightglass_Coffee.jpg", "Cappuccino", "Coffee drink", new DateTime(2023, 1, 15, 20, 18, 15, 56, DateTimeKind.Utc).AddTicks(4761) },
                    { new Guid("fc3eb9a0-c574-46f0-b715-93b6df8be3fb"), new Guid("1ed1ad02-0caa-470c-942c-462ffb41ecdc"), new DateTime(2023, 1, 15, 20, 18, 15, 56, DateTimeKind.Utc).AddTicks(4783), "A Moscow mule is a cocktail made with vodka, ginger beer and lime juice, garnished with a slice or wedge of lime and a sprig of mint. The drink is a type of buck and is sometimes called a vodka buck. The Moscow mule is popularly served in a copper mug, which takes on the cold temperature of the liquid.", "https://upload.wikimedia.org/wikipedia/commons/8/89/Cocktail_Moscow_Mule_im_Kupferbecher_mit_Minze.jpg", "Moscow mule", "Cocktail", new DateTime(2023, 1, 15, 20, 18, 15, 56, DateTimeKind.Utc).AddTicks(4784) }
                });
        }
    }
}
