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

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RecipeConsole.Menus;
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
