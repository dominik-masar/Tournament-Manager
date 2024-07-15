using EasyConsole;

namespace TournamentManager.ConsoleContext.Pages.Match
{
    public class AddResultPage : ActionPage
    {
        public AddResultPage(Program program, OperationHandler operationHandler)
            : base(operationHandler, "Add Result", program)
        {

        }

        public override void Display()
        {
            base.DisplayWithAction<AddResultPage>(_opHandler.AddResult).Wait();
        }
    }
}
