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
    public class SQLiteIngredientServiceTests : IngredientServiceTests, IDisposable
    {
        public SQLiteIngredientServiceTests() :
            base(new DbContextOptionsBuilder<RecipeManagerContext>().UseSqlite("Filename=TestIng.db")
                .Options)
        { }

        public void Dispose()
        {
            using (var context = new RecipeManagerContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task Test_WithDB_GetIngredientByName()
        {
            using (var context = new RecipeManagerContext(ContextOptions))
            {
                var ingredientService = new IngredientService(new UnitOfWork(context, new NullLogger<UnitOfWork>()), new NullLogger<IngredientService>());
                var ingredient = await ingredientService.GetIngredientByName("Apple");
    
                Assert.NotNull(ingredient);
                Assert.Equal("Apple", ingredient.Name);
            }
        }

        [Fact]
        public async Task Test_WithDB_GetAllIngredients()
        {
            using (var context = new RecipeManagerContext(ContextOptions))
            {
                var ingredientService = new IngredientService(new UnitOfWork(context, new NullLogger<UnitOfWork>()), new NullLogger<IngredientService>());

                var allIngredients = await ingredientService.GetAllIngredients();

                Assert.NotNull(allIngredients);
                Assert.Equal(3, allIngredients.Count());
                Assert.Collection(allIngredients, item => Assert.Equal("Apple", item.Name),
                    item => Assert.Equal("Orange", item.Name),
                    item => Assert.Equal("Peach", item.Name));
            }
        }

        [Fact]
        public async Task Test_WithDB_AddIngredient()
        {
            using (var context = new RecipeManagerContext(ContextOptions))
            {
                var ingredientService = new IngredientService(new UnitOfWork(context, new NullLogger<UnitOfWork>()), new NullLogger<IngredientService>());
                ingredientService.AddIngredient(new IngredientModel { Name = "Carrot" });

                var isItInDb = await ingredientService.GetIngredientByName("Carrot");

                Assert.NotNull(isItInDb);
                Assert.Equal("Carrot", isItInDb.Name);
            }
        }

        [Fact]
        public async Task Test_WithDB_DeleteIngredientByName()
        {
            using (var context = new RecipeManagerContext(ContextOptions))
            {
                var ingredientService = new IngredientService(new UnitOfWork(context, new NullLogger<UnitOfWork>()), new NullLogger<IngredientService>());
                ingredientService.DeleteIngredientByName("Apple");


                await Assert.ThrowsAsync<KeyNotFoundException>(async () => await ingredientService.GetIngredientByName("Apple"));
            }
        }
    }
}
