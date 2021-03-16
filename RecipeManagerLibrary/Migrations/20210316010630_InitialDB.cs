using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeLibrary.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeModelId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeModelId);
                });

            migrationBuilder.CreateTable(
                name: "ingredients",
                columns: table => new
                {
                    IngredientModelId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    RecipeModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredients", x => x.IngredientModelId);
                    table.ForeignKey(
                        name: "FK_ingredients_Recipes_RecipeModelId",
                        column: x => x.RecipeModelId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ingredients_RecipeModelId",
                table: "ingredients",
                column: "RecipeModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ingredients");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
