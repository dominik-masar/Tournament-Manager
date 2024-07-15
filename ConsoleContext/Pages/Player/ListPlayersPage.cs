using EasyConsole;

namespace TournamentManager.ConsoleContext.Pages.Player
{
    public class ListPlayersPage : ActionPage
    {
        public ListPlayersPage(Program program, OperationHandler operationHandler)
            : base(operationHandler, "List Players", program)
        {

        }

        public override void Display()
        {
            base.DisplayWithAction<ListPlayersPage>(_opHandler.ListPlayers).Wait();
        }
    }
}
