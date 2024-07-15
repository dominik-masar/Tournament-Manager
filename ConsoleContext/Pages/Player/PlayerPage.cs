using EasyConsole;

namespace TournamentManager.ConsoleContext.Pages.Player
{
    public class PlayerPage : MenuPage
    {
        public PlayerPage(Program program)
            : base("Player Options", program,
                  new Option("Add player", () => program.NavigateTo<AddPlayerPage>()),
                  new Option("List players", () => program.NavigateTo<ListPlayersPage>()),
                  new Option("Delete player", () => program.NavigateTo<DeletePlayerPage>()))
        { }
    }
}
