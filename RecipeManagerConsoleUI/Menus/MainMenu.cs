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
using System;

namespace RecipeConsole.Menus
{
    public class MainMenu : IMainMenu
    {
        private readonly IIngredientMenu _ingredientMenu;
        private readonly IRecipeMenu _recipeMenu;
        private readonly ILogger _logger;

        private enum MainMenuOption { InValid = 0, RecipeMenu = 1, IngredientMenu = 2, Quit = 3 };

        public MainMenu(IIngredientMenu ingredientMenu,
            IRecipeMenu recipeMenu,
            ILogger<MainMenu> logger)
        {
            _ingredientMenu = ingredientMenu;
            _recipeMenu = recipeMenu;
            _logger = logger;
        }

        public void Show()
        {
            while (true)
            {
                ConsoleHelper.DefaultColor = ConsoleColor.Blue;
                ConsoleHelper.ColorWriteLine(ConsoleColor.Yellow, "Welcome to Recipe Manager");
                ConsoleHelper.ColorWriteLine(ConsoleColor.Cyan, "https://github.com/JoyfulReaper/RecipeManager");
                Console.WriteLine();
                ConsoleHelper.ColorWriteLine("1.) Recipe Menu");
                ConsoleHelper.ColorWriteLine("2.) Ingredient Menu");
                Console.WriteLine();
                ConsoleHelper.ColorWriteLine(ConsoleColor.Red, "3.) Quit");
                Console.WriteLine();


                string input = string.Empty;
                int option = 0;
                bool valid = false;

                while (!valid)
                {
                    ConsoleHelper.ColorWrite(ConsoleColor.Yellow, "Please select an option: ");
                    input = Console.ReadLine();

                    valid = ConsoleHelper.ValidateInt(input, (int)MainMenuOption.RecipeMenu, (int)MainMenuOption.Quit, out option);

                    if (!Enum.IsDefined(typeof(MainMenuOption), option))
                    {
                        _logger.LogWarning("Option could not be converted to MainMenuOption Enum");
                        valid = false;
                    }

                }

                MainMenuOption choice = (MainMenuOption)option;

                if (choice == MainMenuOption.Quit)
                {
                    return;
                }

                ExecuteMenuSelection(choice);
            }
        }

        private void ExecuteMenuSelection(MainMenuOption option)
        {
            switch (option)
            {
                case MainMenuOption.InValid:
                    _logger.LogWarning("ExecuteMenuSelection recieved invalid option");
                    break;
                case MainMenuOption.RecipeMenu:
                    Console.WriteLine();
                    _recipeMenu.Show();
                    break;
                case MainMenuOption.IngredientMenu:
                    Console.WriteLine();
                    _ingredientMenu.Show();
                    break;
                default:
                    break;
            }
        }
    }
}