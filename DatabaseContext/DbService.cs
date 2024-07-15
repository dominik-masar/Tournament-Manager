using TournamentManager.Helpers;
using TournamentManager.TournamentContext;

namespace TournamentManager.DatabaseContext
{
    public class DbService : IDisposable
    {
        public ICollection<Tournament> Tournaments { get; set; }
        public ICollection<Match> Matches { get; set; }
        public ICollection<Player> Players { get; set; }
        public ICollection<Round> Rounds { get; set; }

        private MatchDBContext _matchDB;
        private PlayerDBContext _playerDB;
        private TournamentDBContext _tournamentDB;
        private RoundDBContext _roundDB;

        public DbService()
        {
            _matchDB = new MatchDBContext();
            _playerDB = new PlayerDBContext();
            _tournamentDB = new TournamentDBContext();
            _roundDB = new RoundDBContext();
            Tournaments = _tournamentDB.ReadTournaments();
            Players = _playerDB.ReadPlayers();
            if (Players.All(x => x.Id != -1)) Players.Add(new Player(-1, "Dummy")); // Add Dummy player for free wins
            Matches = _matchDB.ReadMatches();
            Rounds = _roundDB.ReadRounds();
        }

        public void SaveTournaments()
        {
            _tournamentDB.SaveTournaments(Tournaments);
        }

        public void SaveMatches()
        {
            _matchDB.SaveMatches(Matches);
        }

        public void SavePlayers()
        {
            _playerDB.SavePlayers(Players);
        }

        public void SaveRounds()
        {
            _roundDB.SaveRounds(Rounds);
        }

        public void Dispose()
        {
            SaveTournaments();
            SaveMatches();
            SavePlayers();
            SaveRounds();
        }

        public Player? GetPlayer(int id)
        {
            return Players.Where(x => x.Id == id).FirstOrDefault();
        }

        public Tournament? GetTournament(int id)
        {
            return Tournaments.Where(x => x.Id == id).FirstOrDefault();
        }

        public Round? GetRound(int id)
        {
            return Rounds.Where(x => x.Id == id).FirstOrDefault();
        }

        public Match? GetMatch(int id)
        {
            return Matches.Where(x => x.Id == id).FirstOrDefault();
        }

        public Player AddPlayer(string nickname)
        {
            var result = new Player(IdHelper.GetNextPlayerId, nickname);
            Players.Add(result);
            SavePlayers();
            return result;
        }

        public bool DeletePlayer(int id)
        {
            if (!Players.Any(x => x.Id == id)) return false;
            Players = Players.Where(x => x.Id != id).ToList();
            SavePlayers();
            return true;
        }
    }
}
