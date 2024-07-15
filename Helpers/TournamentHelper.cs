using System.Diagnostics;
using TournamentManager.DatabaseContext;
using TournamentManager.TournamentContext;

namespace TournamentManager.Helpers
{
    public class TournamentHelper
    {
        private readonly DbService _db;

        public TournamentHelper(DbService db)
        {
            _db = db;
        }

        public Round Start(Tournament t)
        {
            var round = GenerateRound(t, t.PlayerIds);
            t.RoundIds.Add(round.Id);
            return round;
        }

        public Round? NextRound(Tournament t)
        {
            var players = new List<int>();
            var lastRound = _db.GetRound(t.RoundIds.LastOrDefault());
            if (lastRound == null) return null;
            foreach (int matchId in lastRound!.MatchIds)
            {
                var match = _db.GetMatch(matchId);
                if (match == null || !match.Finished) return null;
                players.Add((int)match.WinnerId!);
            }
            var round = GenerateRound(t, players);
            t.RoundIds.Add(round.Id);
            return round;
        }

        public bool ReadyForNextRound(Tournament t)
        {
            var lastRound = _db.GetRound(t.RoundIds.LastOrDefault());
            if (lastRound == null || lastRound.MatchIds.Count == 1) return false;
            return lastRound!.MatchIds.Select(x => _db.GetMatch(x)).All(x => x?.Finished ?? false);
        }

        public bool GetWinner(Tournament t, out Player? winner)
        {
            winner = null;
            var lastRound = _db.GetRound(t.RoundIds.Last());
            if (lastRound == null || lastRound.MatchIds.Count != 1) return false;

            // There is no player/match with ID -69, so _db will return null.
            var winnerId = _db.GetMatch(lastRound.MatchIds.FirstOrDefault())?.WinnerId ?? -69;

            winner = _db.GetPlayer(winnerId);
            return winner != null;
        }

        private Round GenerateRound(Tournament t, List<int> players)
        {
            Debug.Assert(players.Count % 2 == 0 && players.Count >= 2);
            var matches = new List<int>();
            for (int i = 0; i < players.Count / 2; i++)
            {
                var match = new Match(IdHelper.GetNextMatchId, t.Id, players[i * 2], players[i * 2 + 1]);
                _db.Matches.Add(match);
                _db.SaveMatches();
                matches.Add(match.Id);
            }
            var round = new Round { Id = IdHelper.GetNextRoundId, MatchIds = matches };
            _db.Rounds.Add(round);
            _db.SaveRounds();
            return round;
        }
    }
}
