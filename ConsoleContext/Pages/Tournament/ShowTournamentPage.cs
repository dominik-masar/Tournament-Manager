using EasyConsole;

namespace TournamentManager.ConsoleContext.Pages.Tournament
{
    public class ShowTournamentPage : ActionPage
    {
        public ShowTournamentPage(Program program, OperationHandler operationHandler)
            : base(operationHandler, "Show Tournament", program)
        {

        }

        public override void Display()
        {
            base.DisplayWithAction<ShowTournamentPage>(_opHandler.Export).Wait();
        }
    }
}
