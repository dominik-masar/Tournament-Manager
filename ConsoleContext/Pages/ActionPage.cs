using EasyConsole;

namespace TournamentManager.ConsoleContext.Pages
{
    public class ActionPage : Page
    {
        protected OperationHandler _opHandler;
        public delegate bool Del();

        public ActionPage(OperationHandler operationHandler, string title, Program program)
            : base(title, program)
        {
            this._opHandler = operationHandler;
        }

        public async Task DisplayWithAction<T>(Del callback) where T : Page
        {
            base.Display();
            var result = await Task.Run(() => callback());
            if (result)
            {
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
                Program.NavigateBack();
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Do you want to go back? (Y/n)");
                var response = Console.ReadLine();
                if (response != null && response.Equals("n", StringComparison.OrdinalIgnoreCase)) Program.NavigateTo<T>();
                if (response != null && response.Equals("y", StringComparison.OrdinalIgnoreCase)) Program.NavigateBack();
            }
            Console.Write("Too many unsuccessfull retries! Returning back");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Thread.Sleep(400);
                    Console.Write(".");
                }
                Thread.Sleep(400);
                Console.Write("\b\b\b   \b\b\b");
            }
            Console.WriteLine();
            Program.NavigateBack();
        }
    }
}
