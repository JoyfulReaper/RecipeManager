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

using Microsoft.Extensions.DependencyInjection;
using RecipeConsole.Menus;
using RecipeLibrary.Data;
using Serilog;
using System;
using System.Threading.Tasks;

namespace RecipeConsoleUI
{
    internal class Program
    {
        internal async static Task Main(string[] args)
        {
            Bootstrap.SetupLogging();

            var logger = Log.ForContext<Program>();
            logger.Debug("Recipe Manager Starting");

            var host = Bootstrap.CreateHostBuilder(args).Build();

            try
            {
                //TODO Don't have the DbContext as a dependency here
                var context = host.Services.GetRequiredService<RecipeManagerContext>();
                bool dbWasCreated = await context.Database.EnsureCreatedAsync();

                if (dbWasCreated)
                {
                    host.Services.GetRequiredService<IDataSeed>().Seed();
                }
            }
            catch (Exception ex)
            {
                logger.Fatal("Unable to seed database: {exception}", ex.Message);
                Console.WriteLine("Exception occured error while seeding data:");
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                Environment.Exit(1);
            }

            try
            {
                host.Services.GetRequiredService<IMainMenu>().Show();
            }
            catch (Exception ex)
            {
                logger.Fatal("Unhandeled Exception: {exception}", ex.Message);
                Console.WriteLine("Unhandeled Exception Occured:");
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                Environment.Exit(1);
            }
        }
    }
}
