using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RecipeConsole.Menus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RecipeConsoleUI
{
    public class Application : IHostedService
    {
        private readonly ILogger<Application> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IMainMenu _mainMenu;

        public Application(ILogger<Application> logger,
            IHostApplicationLifetime appLifetime,
            IMainMenu mainMenu)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _mainMenu = mainMenu;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Application: StartAsync() - RecipeManger Starting");

            _mainMenu.Show();

            _appLifetime.StopApplication();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Application: StopAsync() - RecipeManger Stopping");

            return Task.CompletedTask;
        }
    }
}
