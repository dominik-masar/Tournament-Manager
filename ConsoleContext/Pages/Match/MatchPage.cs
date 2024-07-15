using EasyConsole;

namespace TournamentManager.ConsoleContext.Pages.Match
{
    public class MatchPage : MenuPage
    {
        public MatchPage(Program program)
            : base("Player Options", program,
                  new Option("Add result", () => program.NavigateTo<AddResultPage>()),
                  new Option("List matches", () => program.NavigateTo<ListMatchesPage>()))
        { }
    }
}
