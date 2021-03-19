using Microsoft.Extensions.Logging;
using RecipeLibrary.Data;
using RecipeLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeLibrary.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IngredientService> _logger;

        public IngredientService(IUnitOfWork unitOfWork,
            ILogger<IngredientService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task AddIngredient(Ingredient ingredient)
        {
            if (!await IngredientExists(ingredient.Name))
            {
                _unitOfWork.Ingredients.Add(ingredient);
                _logger.LogDebug("IngredientService: AddIngredient() - Ingredient {ingredient} added.", ingredient.Name);
            }
            else
            {
                _logger.LogDebug("IngredientService: AddIngredient() - Attempted to add Ingredient {ingredient} that already existed.", ingredient.Name);
                throw new ArgumentException("Ingredient already exists!", nameof(ingredient));
            }
        }

        public void DeleteIngredient(Ingredient ingredient)
        {
            _unitOfWork.Ingredients.Remove(ingredient);
            _logger.LogDebug("IngredientService: DeleteIngredient() - Deleted ingredient {ingredient}", ingredient.Name);
        }

        public void DeleteIngredientByName(string name)
        {
            _unitOfWork.Ingredients.DeleteIngredientByName(name);
            _logger.LogDebug("IngredientService: DeleteIngredientByName() - Deleted ingredient {ingredient} by name.", name);
        }

        public async Task<List<Ingredient>> GetAllIngredients()
        {
            var res = await _unitOfWork.Ingredients.GetAll();
            _logger.LogDebug("IngredientService: GetAllIngredient() - All ingredients were requested.");
            return res.ToList();
        }

        public async Task<Ingredient> GetIngredientByName(string name)
        {
            var res = await _unitOfWork.Ingredients.Find(i => i.Name.ToLower() == name.ToLower());
            _logger.LogDebug("IngredientService: GetIngredientByName() - Retreived ingredient {ingredient} by name.", name);
            return res.SingleOrDefault();
        }

        public async Task<bool> IngredientExists(string name)
        {
            var res = await _unitOfWork.Ingredients.Find(i => i.Name.ToLower() == name.ToLower());
            var exists = res.SingleOrDefault() != null;

            _logger.LogDebug("IngredientService: IngredientExists() - Checked if ingredient {ingredient} exists: {exists} ", name, exists);
            return exists;
        }

        public void UpdateIngredients()
        {
            _unitOfWork.Complete();
            _logger.LogDebug("IngredientService: Commited changes to database");
        }
    }
}
