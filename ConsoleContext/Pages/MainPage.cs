using EasyConsole;
using TournamentManager.ConsoleContext.Pages.IO;
using TournamentManager.ConsoleContext.Pages.Match;
using TournamentManager.ConsoleContext.Pages.Player;
using TournamentManager.ConsoleContext.Pages.Tournament;

namespace TournamentManager.ConsoleContext.Pages
{
    public class MainPage : MenuPage
    {
        public MainPage(Program program)
            : base("Main Page", program,
                  new Option("Tournament options", () => program.NavigateTo<TournamentPage>()),
                  new Option("Player options", () => program.NavigateTo<PlayerPage>()),
                  new Option("Match options", () => program.NavigateTo<MatchPage>()),
                  new Option("Import/Export options", () => program.NavigateTo<IOPage>()),
                  new Option("Exit", () => Environment.Exit(0)))
        { }
    }
}
