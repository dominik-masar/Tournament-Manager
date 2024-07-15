namespace TournamentManager.TournamentContext
{
    public class Match
    {
        // Node of tree structure for tournament
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
        public int? WinnerId { get; set; }
        public bool Finished => WinnerId != null;
        public Tuple<int, int>? Result { get; set; }

        public Match() { }

        public Match(int id, int tournamentId, int p1Id, int p2Id)
        {
            Id = id;
            TournamentId = tournamentId;
            Player1Id = p1Id;
            Player2Id = p2Id;
            if (Player1Id == -1)
            {
                WinnerId = Player2Id;
                Result = Tuple.Create(0, 1);
            }
            else if (Player2Id == -1)
            {
                WinnerId = Player1Id;
                Result = Tuple.Create(1, 0);
            }
        }
    }
}
