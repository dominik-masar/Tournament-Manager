namespace TournamentManager.TournamentContext
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<int> PlayerIds { get; set; }
        public List<int> RoundIds { get; set; }
        public bool Finished { get; set; }

        public Tournament() { }

        public Tournament(int id, string description, List<Player> players)
        {
            Id = id;
            Finished = false;
            Description = description;
            PlayerIds = new List<int>();
            RoundIds = new List<int>();
            // Gets closest larger power of 2
            var numberOfPlayers = (int)Math.Pow(2, Math.Ceiling(Math.Log2(players.Count)));
            PlayerIds.AddRange(players.Select(x => x.Id));
            // Adds dummy players (free wins) to fill player count to closest larger power of 2
            for (int _ = 0; _ < numberOfPlayers - players.Count; _++)
            {
                PlayerIds.Add(-1);
            }
            // Shuffles players
            PlayerIds.Shuffle();
        }

        public override string ToString()
        {
            return $"Id: {Id}; Description: {Description}";
        }
    }
}
