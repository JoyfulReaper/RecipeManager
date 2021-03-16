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

        public void AddRecipe(RecipeModel recipe)
        {
            _unitOfWork.Recipes.Add(recipe);
        }

        public void DeleteRecipe(RecipeModel recipe)
        {
            _unitOfWork.Recipes.Remove(recipe);
        }

        public void DeleteRecipeByName(string name)
        {
            _unitOfWork.Recipes.DeleteRecipeByName(name);
        }

        public async Task<List<RecipeModel>> GetAllRecipes()
        {
            var res = await _unitOfWork.Recipes.GetAllRecipesWithIngredients();
            return res.ToList();
        }

        public async Task<RecipeModel> GetRecipeByName(string name)
        {
            var res =  await _unitOfWork.Recipes.Find(r => r.Name == name);
            return res.FirstOrDefault();
        }

        public async Task<bool> RecipeExists(string name)
        {
            var res = await _unitOfWork.Recipes.Find(i => i.Name == name);
            return res.FirstOrDefault() != null;
        }

        public void UpdateRecipes(RecipeModel recipe)
        {
            _unitOfWork.Complete();
        }
    }
}
