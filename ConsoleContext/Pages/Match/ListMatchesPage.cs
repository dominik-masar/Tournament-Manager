using EasyConsole;

namespace TournamentManager.ConsoleContext.Pages.Match
{
    public class ListMatchesPage : ActionPage
    {
        public ListMatchesPage(Program program, OperationHandler operationHandler)
            : base(operationHandler, "List Matches", program)
        {

        }

        public override void Display()
        {
            base.DisplayWithAction<ListMatchesPage>(_opHandler.ListMatches).Wait();
        }
    }
}
