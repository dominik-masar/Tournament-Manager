using EasyConsole;

namespace TournamentManager.ConsoleContext.Pages.Tournament
{
    public class TournamentPage : MenuPage
    {
        public TournamentPage(Program program)
            : base("Tournament Options", program,
                  new Option("Start tournament", () => program.NavigateTo<StartTournamentPage>()),
                  new Option("List tournaments", () => program.NavigateTo<ListTournamentsPage>()),
                  new Option("Show tournament", () => program.NavigateTo<ShowTournamentPage>()))
        { }
    }
}
