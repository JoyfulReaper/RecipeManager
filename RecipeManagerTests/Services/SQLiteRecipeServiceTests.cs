/*
MIT License
Copyright(c) 2021 Kyle Givler
https://github.com/JoyfulReaper
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using RecipeLibrary.Data;
using RecipeLibrary.Models;
using RecipeLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RecipeTests.Services
{
    public class SQLiteRecipeServiceTests : RecipeServiceTests, IDisposable
    {
        public SQLiteRecipeServiceTests() :
            base(new DbContextOptionsBuilder<RecipeManagerContext>().UseSqlite("Filename=TestRec.db")
                .Options)
        {}

        public void Dispose()
        {
            using (var context = new RecipeManagerContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task Test_WithDB_GetRecipeByName()
        {
            using (var context = new RecipeManagerContext(ContextOptions))
            {
                var recipeService = new RecipeService(new UnitOfWork(context), new NullLogger<RecipeService>());
                var recipe = await recipeService.GetRecipeByName("Apple Pie");

                Assert.NotNull(recipe);
                Assert.Equal("Apple Pie", recipe.Name);
            }
        }

        [Fact]
        public async Task Test_WithDB_GetAllRecipe()
        {
            using (var context = new RecipeManagerContext(ContextOptions))
            {
                var recipeService = new RecipeService(new UnitOfWork(context), new NullLogger<RecipeService>());

                var allRecipes = await recipeService.GetAllRecipes();

                Assert.NotNull(allRecipes);
                Assert.Equal(2, allRecipes.Count());
                Assert.Collection(allRecipes, item => Assert.Equal("Fruit Salad", item.Name),
                    item => Assert.Equal("Apple Pie", item.Name));

                Assert.Collection(allRecipes.FirstOrDefault().Ingredients, item => Assert.Equal("Apple", item.Name),
                    item => Assert.Equal("Orange", item.Name),
                    item => Assert.Equal("Peach", item.Name));
            }
        }

        [Fact]
        public async Task Test_WithDB_AddRecipe()
        {
            using (var context = new RecipeManagerContext(ContextOptions))
            {
                var recipeService = new RecipeService(new UnitOfWork(context), new NullLogger<RecipeService>());

                RecipeModel recipe = new RecipeModel
                {
                    Name = "Chips",
                    Ingredients = new List<IngredientModel> { new IngredientModel { Name = "Potato" },
                        new IngredientModel { Name = "Oil" }
                    }
                };

                recipeService.AddRecipe(recipe);
                recipeService.UpdateRecipes();

                var isItInDb = await recipeService.GetRecipeByName("Chips");

                Assert.NotNull(isItInDb);
                Assert.Equal("Chips", isItInDb.Name);

                Assert.Collection(isItInDb.Ingredients, item => Assert.Equal("Potato", item.Name),
                    item => Assert.Equal("Oil", item.Name));
            }
        }

        [Fact]
        public async Task Test_WithDB_AddIngredient()
        {
            //TODO re-write test

            //using (var context = new RecipeManagerContext(ContextOptions))
            //{
            //    var recipeService = new RecipeService(new UnitOfWork(context), new NullLogger<RecipeService>());
            //    var ingredientService = new IngredientService(new UnitOfWork(context), new NullLogger<IngredientService>());

            //    ingredientService.AddIngredient(new IngredientModel { Name = "Cherry" });

            //    await recipeService.AddIngredient(await recipeService.GetRecipeByName("Fruit Salad"),
            //        await ingredientService.GetIngredientByName("Cherry"));


            //    var isItInDb = await recipeService.GetRecipeByName("Fruit Salad");
            //    Assert.NotNull(isItInDb);
            //    Assert.Equal("Fruit Salad", isItInDb.Name);

            //    Assert.Collection(isItInDb.Ingredients, item => Assert.Equal("Apple", item.Name),
            //        item => Assert.Equal("Orange", item.Name),
            //        item => Assert.Equal("Peach", item.Name),
            //        item => Assert.Equal("Cherry", item.Name));
            //}
        }

        [Fact]
        public async Task Test_WithDB_RemoveIngredientFromRecipe()
        {
            //TODO - Re-write test

            //using (var context = new RecipeManagerContext(ContextOptions))
            //{
            //    var recipeService = new RecipeService(new UnitOfWork(context), new NullLogger<RecipeService>());
            //    var ingredentService = new IngredientService(new UnitOfWork(context), new NullLogger<IngredientService>());

            //    await recipeService.DeleteIngredient(await recipeService.GetRecipeByName("Apple Pie"),
            //        await ingredentService.GetIngredientByName("Crust"));

            //    var recipe = await recipeService.GetRecipeByName("Apple Pie");

            //    Assert.NotNull(recipe);
            //    Assert.Equal("Apple Pie", recipe.Name);

            //    Assert.Collection(recipe.Ingredients, item => Assert.Equal("Apple", item.Name),
            //        item => Assert.Equal("Sugar", item.Name));
            //}
        }

        [Fact]
        public async Task Test_WithDB_DeleteByNameRecipe()
        {
            using (var context = new RecipeManagerContext(ContextOptions))
            {
                var recipeService = new RecipeService(new UnitOfWork(context), new NullLogger<RecipeService>());
                recipeService.DeleteRecipeByName("Apple Pie");

                await Assert.ThrowsAsync<KeyNotFoundException>(async () => await recipeService.GetRecipeByName("Apple Pie"));
            }
        }
    }
}
