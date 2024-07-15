using EasyConsole;
using TournamentManager.ConsoleContext;
using TournamentManager.ConsoleContext.Pages;
using TournamentManager.ConsoleContext.Pages.IO;
using TournamentManager.ConsoleContext.Pages.Match;
using TournamentManager.ConsoleContext.Pages.Player;
using TournamentManager.ConsoleContext.Pages.Tournament;
using TournamentManager.DatabaseContext;
using TournamentManager.Helpers;

namespace TournamentManager
{
    public class TournamentManager : Program
    {
        public TournamentManager() : base("TournamentManager", breadcrumbHeader: true)
        {
            using (var db = new DbService())
            {
                IdHelper.Initialize(db);
                var opHandler = new OperationHandler(db);

                // Main page
                AddPage(new MainPage(this));

                // Secondary pages for options
                AddPage(new TournamentPage(this));
                AddPage(new PlayerPage(this));
                AddPage(new MatchPage(this));
                AddPage(new IOPage(this));

                // Action pages on player options page
                AddPage(new AddPlayerPage(this, opHandler));
                AddPage(new ListPlayersPage(this, opHandler));
                AddPage(new DeletePlayerPage(this, opHandler));

                // Action pages on tournament options page
                AddPage(new ListTournamentsPage(this, opHandler));
                AddPage(new StartTournamentPage(this, opHandler));
                AddPage(new ShowTournamentPage(this, opHandler));

                // Action pages on match options page
                AddPage(new ListMatchesPage(this, opHandler));
                AddPage(new AddResultPage(this, opHandler));

                // Action pages on IO options page
                AddPage(new ImportPage(this, opHandler));
                AddPage(new ExportPage(this, opHandler));

                SetPage<MainPage>();
            }
        }
    }
}
