using EasyConsole;

namespace TournamentManager.ConsoleContext.Pages.Match
{
    public class ImportPage : ActionPage
    {
        public ImportPage(Program program, OperationHandler operationHandler)
            : base(operationHandler, "Import Players", program)
        {

        }

        public override void Display()
        {
            base.DisplayWithAction<ImportPage>(_opHandler.Import).Wait();
        }
    }
}
