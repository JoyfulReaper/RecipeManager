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
using RecipeLibrary.Models;
using RecipeLibrary.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeConsole.Menus
{
    public class IngredientMenu : IIngredientMenu
    {
        private readonly IIngredientService _ingredientService;
        private readonly ILogger<IngredientMenu> _logger;
        private readonly int _ingredientsPerPage = 8;

        private enum IngredientMenuOption { InValid = 0, NewIngredient = 1, LookUpIngredient = 2, ShowIngredient = 3, DeleteIngredient = 4, GoBack = 5 };

        public IngredientMenu(IIngredientService ingredientService,
            ILogger<IngredientMenu> logger)
        {
            _ingredientService = ingredientService;
            _logger = logger;
        }

        public async Task Show()
        {
            ConsoleHelper.DefaultColor = ConsoleColor.Blue;
            ConsoleHelper.ColorWriteLine(ConsoleColor.Yellow, "Ingredient Menu");
            Console.WriteLine();
            ConsoleHelper.ColorWriteLine("1.) New Ingredient");
            ConsoleHelper.ColorWriteLine("2.) Lookup Ingredient");
            ConsoleHelper.ColorWriteLine("3.) Show Ingredient List");
            ConsoleHelper.ColorWriteLine("4.) Delete Ingredient");
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

                valid = ConsoleHelper.ValidateInt(input, (int)IngredientMenuOption.NewIngredient, (int)IngredientMenuOption.GoBack, out option);

                if (!Enum.IsDefined(typeof(IngredientMenuOption), option))
                {
                    _logger.LogWarning("IngredientMenu: Option could not be converted to IngredientMenuOption Enum");
                    valid = false;
                }

            }

            IngredientMenuOption choice = (IngredientMenuOption)option;
            await ExecuteMenuSelection(choice);
        }

        private async Task ExecuteMenuSelection(IngredientMenuOption option)
        {
            switch (option)
            {
                case IngredientMenuOption.InValid:
                    _logger.LogWarning("IngredientMenu: ExecuteMenuSelection() - recieved invalid option");
                    break;
                case IngredientMenuOption.NewIngredient:
                    await NewIngredient();
                    break;
                case IngredientMenuOption.LookUpIngredient:
                    await LookupIngredient();
                    break;
                case IngredientMenuOption.ShowIngredient:
                    await ListIngredients();
                    break;
                case IngredientMenuOption.DeleteIngredient:
                    await DeleteIngredient();
                    break;
                case IngredientMenuOption.GoBack:
                    Console.WriteLine();
                    break;
                default:
                    _logger.LogError("IngredientMenu: ExecuteMenuSelection() - default case hit. Option: {option}", (int)option);
                    break;
            }
        }

        private async Task LookupIngredient()
        {
            ConsoleHelper.ColorWrite("What ingredient would you like to lookup: ");
            var name = Console.ReadLine();

            Console.WriteLine();

            if (await _ingredientService.IngredientExists(name))
            {
                ConsoleHelper.ColorWriteLine(ConsoleColor.DarkYellow, $"{name} exists.");
            }
            else
            {
                ConsoleHelper.ColorWriteLine(ConsoleColor.DarkYellow, $"{name} does not exist.");
            }

            Console.WriteLine();
            await this.Show();
        }

        private async Task DeleteIngredient()
        {
            ConsoleHelper.ColorWrite("What ingredient would you like to delete: ");
            var name = Console.ReadLine();

            try
            {
                _ingredientService.DeleteIngredientByName(name);
                _ingredientService.UpdateIngredients();

                ConsoleHelper.ColorWriteLine(ConsoleColor.Green, $"'{name}' has been deleted.");
            }
            catch (ArgumentException)
            {
                ConsoleHelper.ColorWriteLine(ConsoleColor.DarkYellow, $"{name} does not exist.");
            }

            Console.WriteLine();
            await this.Show();
        }

        private async Task NewIngredient()
        {
            ConsoleHelper.ColorWrite("What ingredient would you like to add: ");
            var name = Console.ReadLine();

            IngredientModel newIngreditent = new IngredientModel { Name = name };
            try
            {
                await _ingredientService.AddIngredient(newIngreditent);
                _ingredientService.UpdateIngredients();

                ConsoleHelper.ColorWriteLine(ConsoleColor.Green, $"'{newIngreditent.Name}' has been added.");
            }
            catch (ArgumentException)
            {
                ConsoleHelper.ColorWriteLine(ConsoleColor.DarkYellow, $"{name} already exists.");
            }

            Console.WriteLine();
            await this.Show();
        }

        private async Task ListIngredients()
        {
            Console.WriteLine();
            ConsoleHelper.ColorWriteLine("Known Ingredients: ");

            List<IngredientModel> ingredientList = await _ingredientService.GetAllIngredients();

            for (int i = 0; i < ingredientList.Count; i++)
            {
                if (i % _ingredientsPerPage == 0 && i != 0)
                {
                    Console.WriteLine();
                    ConsoleHelper.ColorWriteLine(ConsoleColor.Yellow,"Press enter for next page.");
                    Console.ReadLine();
                }
                Console.WriteLine(ingredientList[i].Name);
            }
            Console.WriteLine();
            await this.Show();
        }
    }
}
