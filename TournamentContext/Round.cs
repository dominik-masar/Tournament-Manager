namespace TournamentManager.TournamentContext
{
    public class Round
    {
        public int Id { get; set; }
        public List<int> MatchIds { get; set; } = new List<int>();
    }
}
