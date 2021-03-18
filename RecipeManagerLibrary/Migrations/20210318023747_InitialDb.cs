using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeLibrary.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    IngredientModelId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar(75)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.IngredientModelId);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeModelId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeModelId);
                });

            migrationBuilder.CreateTable(
                name: "IngredientModelRecipeModel",
                columns: table => new
                {
                    IngredientsIngredientModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    recipesRecipeModelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientModelRecipeModel", x => new { x.IngredientsIngredientModelId, x.recipesRecipeModelId });
                    table.ForeignKey(
                        name: "FK_IngredientModelRecipeModel_Ingredients_IngredientsIngredientModelId",
                        column: x => x.IngredientsIngredientModelId,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientModelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredientModelRecipeModel_Recipes_recipesRecipeModelId",
                        column: x => x.recipesRecipeModelId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeModelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientModelRecipeModel_recipesRecipeModelId",
                table: "IngredientModelRecipeModel",
                column: "recipesRecipeModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IngredientModelRecipeModel");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
