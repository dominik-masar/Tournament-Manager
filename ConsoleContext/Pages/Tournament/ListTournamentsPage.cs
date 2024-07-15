using EasyConsole;

namespace TournamentManager.ConsoleContext.Pages.Tournament
{
    public class ListTournamentsPage : ActionPage
    {
        public ListTournamentsPage(Program program, OperationHandler operationHandler)
            : base(operationHandler, "List Tournaments", program)
        {

        }

        public override void Display()
        {
            base.DisplayWithAction<ListTournamentsPage>(_opHandler.ListTournaments).Wait();
        }
    }
}
