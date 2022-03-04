using rpatest.Abstractions;
using rpatest.Abstractions.Steps;
using rpatest.Infrastructure;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace rpatest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var searchString = await StringGenerator.Method1("query.txt");
            //var searchString = await StringGenerator.Method2("query.txt");

            Console.WriteLine($"Generated string: {searchString}");

            SearchInGoogle(searchString);
        }

        private static void SearchInGoogle(string searchString)
        {
            var controlService = new ControlService();

            const int winKey = 70;
            const int rKey = 61;
            const int backspaceKey = 2;
            const string openBrowserCommandString = "chrome.exe --force-renderer-accessibility \"http://www.google.com\"";

            var steps = new List<Step>
            {
                new HotKeyStep("Open Run window", new int[] { winKey, rKey }, controlService),
                new HotKeyStep("Clear previous input", new int[] { backspaceKey }, controlService),
                new TypeTextStep("Type command to open Chrome browser", new ControlInfo { Parent="Run", Id ="1001", Name="Open:", Type="edit", Class="Edit" }, openBrowserCommandString, controlService),
                new ClickStep("Click on OK button", new ControlInfo {Parent="Run", Id="1", Name="OK", Type="button", Class="Button" }, controlService),
                new TypeTextStep("Type search string", new ControlInfo { Parent="Google - Google Chrome", Name="Найти", Type="combo box" }, searchString, controlService),
                new ClickStep("Click on search button", new ControlInfo {Parent="Google - Google Chrome", Name="Поиск в Google", Type="button" }, controlService)
            };

            var player = new ProcessPlayer();
            player.Play(steps);
        }
    }
}
