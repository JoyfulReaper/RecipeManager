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
        // TODO add logging
        // TODO add error checking

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IngredientService> _logger;

        public IngredientService(IUnitOfWork unitOfWork,
            ILogger<IngredientService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task AddIngredient(IngredientModel ingredient)
        {
            if (!await IngredientExists(ingredient.Name))
            {
                _unitOfWork.Ingredients.Add(ingredient);
            }
            else
            {
                throw new ArgumentException("Ingredient already exists!", nameof(ingredient));
            }
        }

        public void DeleteIngredient(IngredientModel ingredient)
        {
            _unitOfWork.Ingredients.Remove(ingredient);
            //_unitOfWork.Complete();
        }

        public void DeleteIngredientByName(string name)
        {
            _unitOfWork.Ingredients.DeleteIngredientByName(name);
            //_unitOfWork.Complete();
        }

        public async Task<List<IngredientModel>> GetAllIngredients()
        {
            var res = await _unitOfWork.Ingredients.GetAll();
            return res.ToList();
        }

        public async Task<IngredientModel> GetIngredientByName(string name)
        {
            var res = await _unitOfWork.Ingredients.Find(i => i.Name.ToLower() == name.ToLower());
            return res.FirstOrDefault();
        }

        public async Task<bool> IngredientExists(string name)
        {
            var res = await _unitOfWork.Ingredients.Find(i => i.Name.ToLower() == name.ToLower());
            return res.FirstOrDefault() != null;
        }

        public void UpdateIngredients()
        {
            _unitOfWork.Complete();
        }
    }
}
