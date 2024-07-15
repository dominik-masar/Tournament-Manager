using TournamentManager.DatabaseContext;

namespace TournamentManager.Helpers
{
    public static class IdHelper
    {
        private static int _matchId = 0;
        private static int _tournamentId = 0;
        private static int _playerId = 0;
        private static int _roundId = 0;

        public static int GetNextMatchId => ++_matchId;
        public static int GetNextTournamentId => ++_tournamentId;
        public static int GetNextPlayerId => ++_playerId;
        public static int GetNextRoundId => ++_roundId;

        public static void Initialize(DbService db)
        {
            _matchId = (db.Matches.Count > 0) ? db.Matches.Max(x => x.Id) : 0;
            _tournamentId = (db.Tournaments.Count > 0) ? db.Tournaments.Max(x => x.Id) : 0;
            _playerId = (db.Players.Count > 1) ? db.Players.Max(x => x.Id) : 0;
            _roundId = (db.Rounds.Count > 0) ? db.Rounds.Max(x => x.Id) : 0;
        }
    }
}
