using Microsoft.EntityFrameworkCore;
using RecipeLibrary.Models;

namespace RecipeLibrary.Data
{
    public class RecipeManagerContext : DbContext
    {
        public RecipeManagerContext(DbContextOptions options) : base(options) { }

        public DbSet<RecipeModel> Recipes { get; set;}
        public DbSet<IngredientModel> ingredients { get; set; }
    }
}
