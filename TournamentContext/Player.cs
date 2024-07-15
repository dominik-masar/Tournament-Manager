namespace TournamentManager.TournamentContext
{
    public class Player
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public int TotalWins { get; set; } = 0;
        public int TotalMatches { get; set; } = 0;
        public double? WinRatio
        {
            get => (double)TotalWins / Math.Max(TotalMatches, 1);
        }

        public Player() { }

        public Player(int id, string name)
        {
            Id = id;
            Nickname = name;
        }

        public void AddWin() { TotalWins++; TotalMatches++; }
        public void AddLose() { TotalMatches++; }

        public override string ToString()
        {
            return $"{Id}. {Nickname}; WinRatio: {WinRatio:0.00}";
        }
    }
}
