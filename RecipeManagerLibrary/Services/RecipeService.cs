using Microsoft.Extensions.Logging;
using RecipeLibrary.Data;
using RecipeLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeLibrary.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RecipeService> _logger;

        public RecipeService(IUnitOfWork unitOfWork,
            ILogger<RecipeService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task AddRecipe(Recipe recipe)
        {
            if (!await RecipeExists(recipe.Name))
            {
                _unitOfWork.Recipes.Add(recipe);
                _logger.LogDebug("RecipeService: AddRecipe() - Recipe {recipe} added.", recipe.Name);
            }
            else
            {
                _logger.LogDebug("RecipeService: AddRecipe() - Attempted to add recipe {recipe} that already existed.", recipe.Name);
                throw new ArgumentException("Recipe already exists!", nameof(recipe));
            }
        }

        public void DeleteRecipe(Recipe recipe)
        {
            _unitOfWork.Recipes.Remove(recipe);
            _logger.LogDebug("RecipeService: DeleteRecipe() - Recipe {recipe} deleted.", recipe.Name);
        }

        public void DeleteRecipeByName(string name)
        {
            _unitOfWork.Recipes.DeleteRecipeByName(name);
            _logger.LogDebug("RecipeService: DeleteRecipeByName() - Deleted recipe {recipe} by name.", name);
        }

        public async Task<List<Recipe>> GetAllRecipes()
        {
            var res = await _unitOfWork.Recipes.GetAllRecipesWithIngredients();
            _logger.LogDebug("RecipeService: GetAllRecipes() - All recipes were requested.");
            return res.ToList();
        }

        public async Task<Recipe> GetRecipeByName(string name)
        {
            var res =  await _unitOfWork.Recipes.Find(r => r.Name.ToLower() == name.ToLower(), "Ingredients");
            _logger.LogDebug("RecipeService: GetRecipeByName() - Retreived recipe {recipe} by name.", name);
            return res.SingleOrDefault();
        }

        public async Task<bool> RecipeExists(string name)
        {
            var res = await _unitOfWork.Recipes.Find(i => i.Name.ToLower() == name.ToLower());
            var exists = res.SingleOrDefault() != null;

            _logger.LogDebug("RecipeService: RecipeExists() - Checked if recipe {recipe} exists: {exists} ", name, exists);
            return exists;
        }

        public void UpdateRecipes()
        {
            _unitOfWork.Complete();
            _logger.LogDebug("RecipeService: Commited changes to database");
        }
    }
}
