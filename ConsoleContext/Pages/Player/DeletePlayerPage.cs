using EasyConsole;

namespace TournamentManager.ConsoleContext.Pages.Player
{
    public class DeletePlayerPage : ActionPage
    {
        public DeletePlayerPage(Program program, OperationHandler operationHandler) 
            : base(operationHandler, "Delete Player", program)
        {

        }

        public override void Display()
        {
            base.DisplayWithAction<AddPlayerPage>(_opHandler.DeletePlayer).Wait();
        }
    }
}
