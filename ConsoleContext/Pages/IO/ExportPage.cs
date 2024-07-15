using EasyConsole;

namespace TournamentManager.ConsoleContext.Pages.Match
{
    public class ExportPage : ActionPage
    {
        public ExportPage(Program program, OperationHandler operationHandler)
            : base(operationHandler, "Export Tournament", program)
        {

        }

        public override void Display()
        {
            base.DisplayWithAction<ExportPage>(_opHandler.ExportToFile).Wait();
        }
    }
}
