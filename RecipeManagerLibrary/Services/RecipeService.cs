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

        public Task AddIngredient(RecipeModel recipe, IngredientModel ingredient)
        {
            throw new NotImplementedException();
        }

        public Task AddRecipe(RecipeModel recipe)
        {
            throw new NotImplementedException();
        }

        public Task DeleteIngredient(RecipeModel recipe, IngredientModel ingredient)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRecipe(RecipeModel recipe)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRecipeByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<RecipeModel>> GetAllRecipes()
        {
            throw new NotImplementedException();
        }

        public Task<RecipeModel> GetRecipeByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RecipeExists(string name)
        {
            throw new NotImplementedException();
        }

        public Task UpdateReciept(RecipeModel recipe)
        {
            throw new NotImplementedException();
        }
    }
}
