using System.Text.Json;
using TournamentManager.Helpers;
using TournamentManager.TournamentContext;

namespace TournamentManager.DatabaseContext
{
    public class MatchDBContext
    {
        private readonly string[] _paths = { "..", "..", "..", "DatabaseContext", "Data", "Matches.json" };
        private readonly string _filePath;

        public MatchDBContext()
        {
            _filePath = Path.Combine(_paths);
            FileHelper.CreateFile(_filePath);
        }

        public void SaveMatches(IEnumerable<Match> Matches)
        {
            if (Matches.Select(p => p.Id).Distinct().Count() != Matches.Count())
            {
                var duplicitMatch = Matches.GroupBy(c => c.Id).Select(g => new { id = g.Key, count = g.Count() }).Where(g => g.count > 1).First();
                var Match = Matches.Where(c => c.Id == duplicitMatch.id).First();
                throw new Exception($"Match with ID {Match.Id} already exists");
            }

            string jsonString = JsonSerializer.Serialize(Matches);
            File.Delete(_filePath);
            using StreamWriter outputFile = new StreamWriter(_filePath);
            outputFile.WriteLine(jsonString);
        }

        public List<Match> ReadMatches()
        {
            string? line;
            using (StreamReader inputFile = new StreamReader(_filePath))
            {
                line = inputFile.ReadLine();
            }

            if (line == null)
            {
                return new List<Match>();
            }

            var model = JsonSerializer.Deserialize<List<Match>>(line);
            return model!;
        }
    }
}
