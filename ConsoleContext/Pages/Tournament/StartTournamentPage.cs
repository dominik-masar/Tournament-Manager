using EasyConsole;

namespace TournamentManager.ConsoleContext.Pages.Tournament
{
    public class StartTournamentPage : ActionPage
    {
        public StartTournamentPage(Program program, OperationHandler operationHandler)
            : base(operationHandler, "Start Tournament", program)
        {

        }

        public override void Display()
        {
            base.DisplayWithAction<StartTournamentPage>(_opHandler.StartTournament).Wait();
        }
    }
}
