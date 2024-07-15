using EasyConsole;
using TournamentManager.ConsoleContext.Pages.Match;

namespace TournamentManager.ConsoleContext.Pages.IO
{
    public class IOPage : MenuPage
    {
        public IOPage(Program program) : base("Player Options", program,
                  new Option("Import players", () => program.NavigateTo<ImportPage>()),
                  new Option("Export tournament", () => program.NavigateTo<ExportPage>()))
        { }
    }
}
