using EasyConsole;

namespace TournamentManager.ConsoleContext.Pages.Player
{
    public class AddPlayerPage : ActionPage
    {
        public AddPlayerPage(Program program, OperationHandler operationHandler)
            : base(operationHandler, "Add Player", program)
        {

        }

        public override void Display()
        {
            base.DisplayWithAction<AddPlayerPage>(_opHandler.AddPlayer).Wait();
        }
    }
}
