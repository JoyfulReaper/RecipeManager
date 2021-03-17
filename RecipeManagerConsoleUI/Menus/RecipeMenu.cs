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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecipeLibrary.Services;
using RecipeLibrary.Models;
using Microsoft.Extensions.Logging;

namespace RecipeConsole.Menus
{
    public class RecipeMenu : IRecipeMenu
    {
        private readonly IRecipeService _recipeService;
        private readonly IIngredientService _ingredientService;
        private readonly ILogger _logger;
        private readonly int _recipePerPage = 8;

        private enum RecipeMenuOption { InValid = 0, NewRecipe = 1, LookUpRecipe = 2, ShowRecipe = 3, DeleteRecipe = 4, GoBack = 5 };

        public RecipeMenu(IRecipeService recipeService,
            IIngredientService ingredientService,
            ILogger<RecipeMenu> logger)
        {
            _recipeService = recipeService;
            _ingredientService = ingredientService;
            _logger = logger;
        }

        public async Task Show()
        {
            ConsoleHelper.DefaultColor = ConsoleColor.Blue;
            ConsoleHelper.ColorWriteLine(ConsoleColor.Yellow, "Recipe Menu");
            Console.WriteLine();
            ConsoleHelper.ColorWriteLine("1.) New Recipe");
            ConsoleHelper.ColorWriteLine("2.) Lookup Recipe");
            ConsoleHelper.ColorWriteLine("3.) Show Recipe List");
            ConsoleHelper.ColorWriteLine("4.) Delete Recipe");
            Console.WriteLine();
            ConsoleHelper.ColorWriteLine(ConsoleColor.Red, "5.) Back to Main Menu");
            Console.WriteLine();

            string input = string.Empty;
            int option = 0;
            bool valid = false;

            while (!valid)
            {
                ConsoleHelper.ColorWrite(ConsoleColor.Yellow, "Please select an option: ");
                input = Console.ReadLine();

                valid = ConsoleHelper.ValidateInt(input, (int)RecipeMenuOption.NewRecipe, (int)RecipeMenuOption.GoBack, out option);

                if (!Enum.IsDefined(typeof(RecipeMenuOption), option))
                {
                    _logger.LogWarning("Option could not be converted to RecipeMenuOption Enum");
                    valid = false;
                }

            }

            RecipeMenuOption choice = (RecipeMenuOption)option;
            await ExecuteMenuSelection(choice);
        }

        private async Task ExecuteMenuSelection(RecipeMenuOption option)
        {
            switch (option)
            {
                case RecipeMenuOption.InValid:
                    _logger.LogWarning("ExecuteMenuSelection recived Invalid Option.");
                    break;
                case RecipeMenuOption.NewRecipe:
                    Console.WriteLine();
                    await NewRecipe();
                    break;
                case RecipeMenuOption.LookUpRecipe:
                    Console.WriteLine();
                    await LookupRecipe();
                    break;
                case RecipeMenuOption.ShowRecipe:
                    Console.WriteLine();
                    await ListRecipe();
                    break;
                case RecipeMenuOption.DeleteRecipe:
                    Console.WriteLine();
                    await DeleteRecipe();
                    break;
                case RecipeMenuOption.GoBack:
                    Console.WriteLine();
                    break;
                default:
                    _logger.LogError("RecipeMenu: ExecuteMenuSelection() - default case hit. Option: {option}", (int)option);
                    break;
            }
        }

        private async Task ListRecipe()
        {
            Console.WriteLine();
            ConsoleHelper.ColorWriteLine("Known Recipes: ");

            List<RecipeModel> recipeList = await _recipeService.GetAllRecipes();

            for (int i = 0; i < recipeList.Count; i++)
            {
                if (i % _recipePerPage == 0 && i != 0)
                {
                    Console.WriteLine();
                    ConsoleHelper.ColorWriteLine(ConsoleColor.Yellow, "Press enter for next page.");
                    Console.ReadLine();
                }
                Console.WriteLine(recipeList[i].Name);
            }
            Console.WriteLine();
            await this.Show();
        }

        private async Task DeleteRecipe()
        {
            ConsoleHelper.ColorWrite("What recipe would you like to delete: ");
            var name = Console.ReadLine();

            try
            {
                _recipeService.DeleteRecipeByName(name);
                _recipeService.UpdateRecipes();
            }
            catch (ArgumentException)
            {
                ConsoleHelper.ColorWriteLine(ConsoleColor.DarkYellow, $"{name} does not exist.");
            }

            Console.WriteLine();
            await this.Show();
        }

        private async Task NewRecipe()
        {
            ConsoleHelper.ColorWrite("What recipe would you like to add: ");
            var name = Console.ReadLine();

            RecipeModel recipe = new RecipeModel { Name = name };

            bool another = true;
            List<IngredientModel> ingredients = new List<IngredientModel>();

            while (another)
            {
                ConsoleHelper.ColorWrite("What ingredeient would you like to add: ");
                var ingredient = Console.ReadLine();

                if(await _ingredientService.IngredientExists(ingredient))
                {
                    ingredients.Add(await _ingredientService.GetIngredientByName(ingredient));
                }
                else
                {
                    ConsoleHelper.ColorWriteLine(ConsoleColor.DarkYellow, "The ingredient does not exist!");
                    ConsoleHelper.ColorWrite("Would you like to add it? (Y/n): ");
                    var add = Console.ReadLine();

                    if (Char.ToUpperInvariant(add[0]) == 'N')
                    {
                        ConsoleHelper.ColorWriteLine(ConsoleColor.Red, "Recipe not added.");
                        Console.WriteLine();
                        return;
                    }

                    ingredients.Add(new IngredientModel { Name = ingredient });
                }

                ConsoleHelper.ColorWrite("Would you like to add another ingredient? (y/N): ");
                var addAnother = Console.ReadLine();

                if (Char.ToUpperInvariant(addAnother[0]) != 'Y')
                {
                    another = false;
                }
            }

            recipe.Ingredients = ingredients;

            try
            {
                await _recipeService.AddRecipe(recipe);
                _recipeService.UpdateRecipes();

                ConsoleHelper.ColorWriteLine(ConsoleColor.Green, $"'{recipe.Name}' has been added.");
            }
            catch (ArgumentException)
            {
                ConsoleHelper.ColorWriteLine(ConsoleColor.DarkYellow, $"{name} already exists.");
            }

            Console.WriteLine();
            await this.Show();
        }

        private async Task LookupRecipe()
        {
            ConsoleHelper.ColorWrite("What Recipe would you like to lookup: ");
            var name = Console.ReadLine();

            Console.WriteLine();

            if(await _recipeService.RecipeExists(name))
            {
                var recipe = await _recipeService.GetRecipeByName(name);
                ConsoleHelper.ColorWriteLine(ConsoleColor.DarkYellow, $"{name} exists.");

                ConsoleHelper.ColorWriteLine(ConsoleColor.DarkYellow, "The Ingredients are: ");
                foreach (var ingredient in recipe.Ingredients)
                {
                    ConsoleHelper.ColorWriteLine(ConsoleColor.White, ingredient.Name);
                }
            }
            else
            {
                ConsoleHelper.ColorWriteLine(ConsoleColor.DarkYellow, $"{name} does not exist.");
            }

            Console.WriteLine();
            await this.Show();
        }
    }
}
