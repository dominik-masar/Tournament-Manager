using System.Text.Json;
using TournamentManager.Helpers;
using TournamentManager.TournamentContext;

namespace TournamentManager.DatabaseContext
{
    public class RoundDBContext
    {
        private readonly string[] _paths = { "..", "..", "..", "DatabaseContext", "Data", "Rounds.json" };
        private readonly string _filePath;

        public RoundDBContext()
        {
            _filePath = Path.Combine(_paths);
            FileHelper.CreateFile(_filePath);
        }

        public void SaveRounds(IEnumerable<Round> Rounds)
        {
            if (Rounds.Select(p => p.Id).Distinct().Count() != Rounds.Count())
            {
                var duplicitRound = Rounds.GroupBy(c => c.Id).Select(g => new { id = g.Key, count = g.Count() }).Where(g => g.count > 1).First();
                var Round = Rounds.Where(c => c.Id == duplicitRound.id).First();
                throw new Exception($"Round with ID {Round.Id} already exists");
            }

            string jsonString = JsonSerializer.Serialize(Rounds);
            File.Delete(_filePath);
            using StreamWriter outputFile = new StreamWriter(_filePath);
            outputFile.WriteLine(jsonString);
        }

        public List<Round> ReadRounds()
        {
            string? line;
            using (StreamReader inputFile = new StreamReader(_filePath))
            {
                line = inputFile.ReadLine();
            }

            if (line == null)
            {
                return new List<Round>();
            }

            var model = JsonSerializer.Deserialize<List<Round>>(line);
            return model!;
        }
    }
}
