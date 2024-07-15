using System.Text.Json;
using TournamentManager.Helpers;
using TournamentManager.TournamentContext;

namespace TournamentManager.DatabaseContext
{
    public class PlayerDBContext
    {
        private readonly string[] _paths = { "..", "..", "..", "DatabaseContext", "Data", "Players.json" };
        private readonly string _filePath;

        public PlayerDBContext()
        {
            _filePath = Path.Combine(_paths);
            FileHelper.CreateFile(_filePath);
        }

        public void SavePlayers(IEnumerable<Player> players)
        {
            if (players.Select(p => p.Id).Distinct().Count() != players.Count())
            {
                var duplicitPlayer = players.GroupBy(c => c.Id).Select(g => new { id = g.Key, count = g.Count() }).Where(g => g.count > 1).First();
                var player = players.Where(c => c.Id == duplicitPlayer.id).First();
                throw new Exception($"Player with ID {player.Id} already exists");
            }

            string jsonString = JsonSerializer.Serialize(players);
            File.Delete(_filePath);
            using StreamWriter outputFile = new StreamWriter(_filePath);
            outputFile.WriteLine(jsonString);
        }

        public List<Player> ReadPlayers()
        {
            string? line;
            using (StreamReader inputFile = new StreamReader(_filePath))
            {
                line = inputFile.ReadLine();
            }

            if (line == null)
            {
                return new List<Player>();
            }

            var model = JsonSerializer.Deserialize<List<Player>>(line);
            return model!;
        }
    }
}
