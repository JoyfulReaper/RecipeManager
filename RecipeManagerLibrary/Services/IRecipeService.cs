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

using RecipeLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeLibrary.Services
{
    public interface IRecipeService
    {
        /// <summary>
        /// Add A recipe to the database
        /// </summary>
        /// <param name="recipe">Recipe to add</param>
        /// <returns></returns>
        void AddRecipe(RecipeModel recipe);

        /// <summary>
        /// Add A recipe to the database
        /// </summary>
        /// <param name="recipe">Recipe to add</param>
        /// <returns></returns>
        Task<List<RecipeModel>> GetAllRecipes();

        /// <summary>
        /// Delete A recipe from the database
        /// </summary>
        /// <param name="recipe">Recipe to delete</param>
        /// <returns></returns>
        void DeleteRecipe(RecipeModel recipe);

        /// <summary>
        /// Delete A recipe from the database
        /// </summary>
        /// <param name="name">Name of the recipe to delete</param>
        /// <returns></returns>
        void DeleteRecipeByName(string name);

        /// <summary>
        /// Update an existing recipe
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns></returns>
        void UpdateRecipes(RecipeModel recipe);

        /// <summary>
        /// Get a recipe by name
        /// </summary>
        /// <param name="name">Name of the recipe</param>
        /// <returns>The requested reciep</returns>
        Task<RecipeModel> GetRecipeByName(string name);

        /// <summary>
        /// Check if a reciepe already exists
        /// </summary>
        /// <param name="name">Recipe to check for</param>
        /// <returns></returns>
        Task<bool> RecipeExists(string name);
    }
}
