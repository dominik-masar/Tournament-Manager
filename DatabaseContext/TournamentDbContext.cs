using System.Text.Json;
using TournamentManager.Helpers;
using TournamentManager.TournamentContext;

namespace TournamentManager.DatabaseContext
{
    public class TournamentDBContext
    {
        private readonly string[] _paths = { "..", "..", "..", "DatabaseContext", "Data", "Tournaments.json" };
        private readonly string _filePath;

        public TournamentDBContext()
        {
            _filePath = Path.Combine(_paths);
            FileHelper.CreateFile(_filePath);
        }

        public void SaveTournaments(IEnumerable<Tournament> Tournaments)
        {
            if (Tournaments.Select(p => p.Id).Distinct().Count() != Tournaments.Count())
            {
                var duplicitTournament = Tournaments.GroupBy(c => c.Id).Select(g => new { id = g.Key, count = g.Count() }).Where(g => g.count > 1).First();
                var Tournament = Tournaments.Where(c => c.Id == duplicitTournament.id).First();
                throw new Exception($"Tournament with ID {Tournament.Id} already exists");
            }

            string jsonString = JsonSerializer.Serialize(Tournaments);
            File.Delete(_filePath);
            using StreamWriter outputFile = new StreamWriter(_filePath);
            outputFile.WriteLine(jsonString);
        }

        public List<Tournament> ReadTournaments()
        {
            string? line;
            using (StreamReader inputFile = new StreamReader(_filePath))
            {
                line = inputFile.ReadLine();
            }

            if (line == null)
            {
                return new List<Tournament>();
            }

            var model = JsonSerializer.Deserialize<List<Tournament>>(line);
            return model!;
        }
    }
}
