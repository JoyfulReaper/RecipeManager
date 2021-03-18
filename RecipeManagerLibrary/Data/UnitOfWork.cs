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

using Microsoft.Extensions.Logging;
using RecipeLibrary.Repositories;

namespace RecipeLibrary.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RecipeManagerContext _context;
        private readonly ILogger _logger;

        public UnitOfWork(RecipeManagerContext context,
            ILogger<IUnitOfWork> logger)
        {
            _context = context;
            _logger = logger;

            Ingredients = new IngredientRepository(_context);
            Recipes = new RecipeRepository(_context);
        }

        public IIngredientRepository Ingredients { get; private set; }

        public IIRecipeRepository Recipes { get; private set; }

        public int Complete()
        {
            _logger.LogTrace("UnitOfWork: Complete() - Saving database changes.");
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
