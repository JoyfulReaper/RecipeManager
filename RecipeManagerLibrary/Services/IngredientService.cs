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

        public Task AddIngredient(IngredientModel recipe)
        {
            throw new NotImplementedException();
        }

        public Task DeleteIngredient(IngredientModel recipe)
        {
            throw new NotImplementedException();
        }

        public Task DeleteIngredientByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<IngredientModel>> GetAllIngredients()
        {
            throw new NotImplementedException();
        }

        public Task<IngredientModel> GetIngredientByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IngredientExists(string name)
        {
            throw new NotImplementedException();
        }

        public Task UpdateIngredient(IngredientModel recipe)
        {
            throw new NotImplementedException();
        }
    }
}
