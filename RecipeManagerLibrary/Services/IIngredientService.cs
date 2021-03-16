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
    public interface IIngredientService
    {
        /// <summary>
        /// Add A ingredient to the database
        /// </summary>
        /// <param name="recipe">Ingredient to add</param>
        /// <returns></returns>
        void AddIngredient(IngredientModel ingredient);

        /// <summary>
        /// Delete an ingredient from the database
        /// </summary>
        /// <param name="ingredient">Ingredient to delete</param>
        /// <returns></returns>
        void DeleteIngredient(IngredientModel ingredient);

        /// <summary>
        /// Delete an ingredient from the database
        /// </summary>
        /// <param name="name">Ingredient to delete</param>
        /// <returns></returns>
        void DeleteIngredientByName(string name);

        /// <summary>
        /// Save changes to Ingredients
        /// </summary>
        /// <param name="Ingredient"></param>
        /// <returns></returns>
        void UpdateIngredients();

        /// <summary>
        /// Get a ingredient by name
        /// </summary>
        /// <param name="name">Name of the recipe</param>
        /// <returns>The requested ingredeint</returns>
        Task<IngredientModel> GetIngredientByName(string name);

        /// <summary>
        /// Get all ingredients from the DB
        /// </summary>
        /// <param name="name">Name of the Ingredient</param>
        /// <returns>All ingredients</returns>
        Task<List<IngredientModel>> GetAllIngredients();

        /// <summary>
        /// Check if a Ingredient already exists
        /// </summary>
        /// <param name="name">Ingredient to check for</param>
        /// <returns></returns>
        Task<bool> IngredientExists(string name);
    }
}
