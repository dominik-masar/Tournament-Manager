using TournamentManager.TournamentContext;

namespace TournamentManager.Helpers
{
    public static class PlayerHelper
    {
        public static void AddWin(Player p) { p.TotalWins++; p.TotalMatches++; }
        public static void AddLose(Player p) { p.TotalMatches++; }
    }
}
