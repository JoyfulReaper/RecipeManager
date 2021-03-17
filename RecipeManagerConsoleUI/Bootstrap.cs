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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RecipeConsole.Menus;
using RecipeLibrary.Data;
using RecipeLibrary.Repositories;
using RecipeLibrary.Services;
using Serilog;
using System;
using System.IO;

namespace RecipeConsoleUI
{
    internal static class Bootstrap
    {
        /// <summary>
        /// Register services and app configs here
        /// </summary>
        /// <returns>IHostBuilder</returns>
        internal static IHostBuilder CreateHostBuilder(string[] args)
        {
            SetupLogging();

            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<RecipeManagerContext>(options =>
                    {
                        options.UseSqlite(hostContext.Configuration.GetConnectionString("Default"));
                    })
                    .AddTransient<IUnitOfWork, UnitOfWork>()
                    .AddTransient<IIngredientRepository, IngredientRepository>()
                    .AddTransient<IIRecipeRepository, RecipeRepository>()
                    .AddTransient<IRecipeService, RecipeService>()
                    .AddTransient<IIngredientService, IngredientService>()
                    .AddTransient<IMainMenu, MainMenu>()
                    .AddTransient<IIngredientMenu, IngredientMenu>()
                    .AddTransient<IRecipeMenu, RecipeMenu>()
                    .AddHostedService<Application>();
                });
        }

        private static void SetupLogging()
        {
            var configBuilder = new ConfigurationBuilder();

            configBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .ReadFrom.Configuration(configBuilder.Build())
                .Enrich.FromLogContext()
                .CreateLogger();

            ILogger logger = Log.ForContext(typeof(Bootstrap));
            logger.Debug("Logging setup successfully");
        }
    }
}