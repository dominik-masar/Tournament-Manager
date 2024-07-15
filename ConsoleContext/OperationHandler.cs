using EasyConsole;
using System.Text;
using TournamentManager.DatabaseContext;
using TournamentManager.Helpers;
using TournamentManager.TournamentContext;

namespace TournamentManager.ConsoleContext
{
    public class OperationHandler
    {
        private DbService _db;
        private TournamentHelper _tournamentHelper;

        public OperationHandler(DbService dbContext)
        {
            _db = dbContext;
            _tournamentHelper = new TournamentHelper(dbContext);
        }

        public bool AddPlayer()
        {
            Console.Write("Nickname: ");
            var nickname = Console.ReadLine();
            if (!CheckNickname(nickname)) return true;

            var player = _db.AddPlayer(nickname!);
            Console.WriteLine($"Player {nickname} with ID {player.Id} created!");
            return true;
        }

        public bool DeletePlayer()
        {
            var id = Input.ReadInt("Id of player to be deleted:", 1, int.MaxValue);
            if (!_db.DeletePlayer(id)) return HandleError("There is no player with given ID!");

            Console.WriteLine($"Player with ID {id} deleted!");
            return true;
        }

        public bool ListMatches()
        {
            Console.Write("Tournament ID: ");
            var id = Input.ReadInt();

            if (!_db.Tournaments.Any(x => x.Id == id)) return HandleError("There is no tournament with given ID.");

            var tournament = _db.Tournaments.Where(x => x.Id == id).First();
            var matches = _db.GetRound(tournament.RoundIds.Last())!.MatchIds.Select(x => _db.GetMatch(x)).Where(x => x != null).ToList();
            matches.ForEach(x =>
            {
                var p1 = _db.GetPlayer(x!.Player1Id)?.Nickname ?? "*not found*";
                var p2 = _db.GetPlayer(x!.Player2Id)?.Nickname ?? "*not found*";

                Console.Write($"Match with ID {x!.Id}: {p1}-{p2}");
                Console.WriteLine(x.Finished ? $"; {x.Result!.Item1}-{x.Result.Item2}" : "");
            });
            return true;
        }

        public bool Export()
        {
            if (!CheckTournament(out var t)) return false;

            var output = ExportHelper(t!);

            Console.WriteLine(output);
            return true;
        }

        public bool ExportToFile()
        {
            if (!CheckTournament(out var t)) return false;

            var filePath = Input.ReadString("Path to a file: ");

            if (filePath == null) return HandleError("Nothing written!");

            var retries = 0;
            while (File.Exists(filePath))
            {
                Console.WriteLine("File already exist. Do you want to rewrite it? (Y/n)");
                var response = Console.ReadLine();
                if (response != null && response.Equals("n", StringComparison.OrdinalIgnoreCase)) return true;
                if (response != null && response.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        return HandleError(ex.Message);
                    }
                }
                retries++;
                if (retries == 3) return true;
            }

            StreamWriter myStream;
            try
            {
                myStream = new StreamWriter(filePath);
            }
            catch (Exception ex)
            {
                return HandleError(ex.Message);
            }

            using (var bar = new ProgressBar())
            {
                var output = ExportHelper(t!);
                Thread.Sleep(1000);
                bar.Report(0.5);
                try
                {
                    myStream.Write(output);
                    myStream.Close();
                }
                catch (Exception e)
                {
                    return HandleError(e.Message);
                }
                Thread.Sleep(1000);
                bar.Report(1.0);
                Thread.Sleep(1000);
            }
            return true;
        }

        public bool Import()
        {
            Console.WriteLine("Path to file:");
            var path = Console.ReadLine();
            var counter = 0;
            if (File.Exists(path))
            {
                using (StreamReader inputFile = new StreamReader(path))
                {
                    var line = inputFile.ReadLine();
                    while (line != null)
                    {
                        if (CheckNickname(line))
                        {
                            _db.AddPlayer(line);
                            counter++;
                        }
                        line = inputFile.ReadLine();
                    }
                    Console.WriteLine($"Imported {counter} valid players.");
                    return true;
                }
            }
            return HandleError("The file does not exist!");
        }

        public bool StartTournament()
        {
            Console.WriteLine("Description for tournament:");
            var desc = Console.ReadLine();
            if (desc == null || desc.Length == 0) return HandleError("Provided invalid input for description.");

            Console.WriteLine("IDs/Nicknames of players separated by space:");
            var players = Console.ReadLine()?.Split(' ').ToList();
            if (players == null || players.Count == 0) return HandleError("Provided invalid input for players.");

            var foundPlayers = _db.Players.Where(x => players.Contains(x.Nickname) || players.Contains(x.Id.ToString())).ToList();

            if (foundPlayers.Count != players.Count)
            {
                var notFound = players.Where(x => !foundPlayers.Select(y => y.Nickname).Contains(x)
                                       && !foundPlayers.Select(y => y.Id.ToString()).Contains(x))
                    .Aggregate(new StringBuilder(),
                        (current, next) => current.Append($"{(current.Length == 0 ? "" : ", ")}{next}"))
                    .ToString();
                Console.WriteLine($"There are no players with ID(s)/Nickname(s):\n{notFound}");
                return false;
            }
            var tournament = new Tournament(IdHelper.GetNextTournamentId, desc, foundPlayers);
            _tournamentHelper.Start(tournament);
            _db.Tournaments.Add(tournament);
            _db.SaveTournaments();
            Console.WriteLine($"Tournament with ID {tournament.Id} started! You can list matches in match menu.");
            return true;
        }

        public bool ListPlayers()
        {
            var players = _db.Players.Where(x => x.Id > 0);
            foreach (var player in players)
            {
                Console.WriteLine(player.ToString());
            }
            return true;
        }

        public bool AddResult()
        {
            var matchId = Input.ReadInt("Match ID: ", 0, int.MaxValue);
            if (!_db.Matches.Any(x => x.Id == matchId)) return HandleError("There is no match with given ID.");

            var p1 = _db.GetPlayer(_db.GetMatch(matchId)!.Player1Id);
            var p2 = _db.GetPlayer(_db.GetMatch(matchId)!.Player2Id);

            var p1score = Input.ReadInt($"{p1!.Nickname} score:", 0, int.MaxValue);
            var p2score = Input.ReadInt($"{p2!.Nickname} score:", 0, int.MaxValue);
            if (p1score == p2score) return HandleError("A match can not end with a draw.");

            HandleAddResult(matchId, p1score, p2score);
            return true;
        }

        public bool ListTournaments()
        {
            _db.Tournaments.ToList().ForEach(x => Console.WriteLine(x));
            return true;
        }

        private bool CheckTournament(out Tournament? t)
        {
            var tId = Input.ReadInt("ID of tournament:", 0, int.MaxValue);

            t = _db.GetTournament(tId);
            if (t == null) return HandleError("There is no tournament with given ID.");
            if (t.RoundIds.Count == 0) return HandleError("WTF?! Contact your mum, there are no rounds in this tournament!!");
            return true;
        }

        private string ExportHelper(Tournament t)
        {
            var tree = GetBinaryTreeFromTournament(t, out var space, out var depth);

            return ExportTree(tree, space, depth);
        }

        private BinaryTree<string> GetBinaryTreeFromTournament(Tournament t, out int space, out int depth)
        {
            List<string> playerStrings = new List<string>();
            var firstRoundMatchIds = _db.GetRound(t.RoundIds.FirstOrDefault())!.MatchIds;
            int treeLeaves = firstRoundMatchIds.Count * 2;
            var expectedLength = treeLeaves * 2 - 1;


            foreach (var id in firstRoundMatchIds)
            {
                var match = _db.GetMatch(id);
                playerStrings.Add(_db.GetPlayer(match!.Player1Id)?.Nickname ?? "*not found*");
                playerStrings.Add(_db.GetPlayer(match!.Player2Id)?.Nickname ?? "*not found*");
            }

            foreach (var id in t.RoundIds)
            {
                var matches = _db.GetRound(id)!.MatchIds.Select(x => _db.GetMatch(x));
                foreach (var match in matches)
                {
                    if (match!.WinnerId != null) playerStrings.Add(_db.GetPlayer((int)match.WinnerId)?.Nickname ?? "*not found*");
                    else playerStrings.Add("");
                }
            }
            var toFill = expectedLength - playerStrings.Count;
            for (int i = 0; i < toFill; i++)
            {
                playerStrings.Add("");
            }
            space = playerStrings.Max(x => x.Length);
            depth = Convert.ToInt32(Math.Log2(treeLeaves));
            playerStrings.Reverse();
            return new BinaryTree<string>(playerStrings);
        }

        private bool HandleAddResult(int matchId, int p1_score, int p2_score)
        {
            var match = _db.GetMatch(matchId);
            if (match == null) return HandleError("There is no match with given ID.");
            if (match.WinnerId != null) return HandleError("This match already has a result.");

            match.WinnerId = (p1_score > p2_score) ? match.Player1Id : match.Player2Id;
            match.Result = new Tuple<int, int>(p1_score, p2_score);

            var p1 = _db.GetPlayer(match.Player1Id);
            var p2 = _db.GetPlayer(match.Player2Id);
            if (p1_score > p2_score)
            {
                PlayerHelper.AddWin(p1!);
                PlayerHelper.AddLose(p2!);
            }
            else
            {
                PlayerHelper.AddWin(p2!);
                PlayerHelper.AddLose(p1!);
            }
            var t = _db.GetTournament(match.TournamentId)!;
            if (_tournamentHelper.ReadyForNextRound(t))
            {
                _tournamentHelper.NextRound(t);
                Console.WriteLine("Next round was generated!");
            }
            if (_tournamentHelper.GetWinner(t, out var winner))
            {
                Console.WriteLine($"Tournament has ended! The winner is {winner!.Nickname}");
            }
            _db.SaveMatches();
            _db.SavePlayers();
            return true;
        }

        private string ExportTree<T>(BinaryTree<T> tree, int space, int depth, float delimeterCoef = 0.5f)
        {
            var sb = new StringBuilder();

            if (tree.Left != null) sb.Append(ExportTree(tree.Left, space, depth - 1, delimeterCoef));

            var delimeterLength = Convert.ToInt32((space + 4) * delimeterCoef);
            var nodeLength = space + 4;

            var lineLength = delimeterLength + nodeLength / 2 - 1;
            var spaceLength = depth * (nodeLength + delimeterLength) - lineLength - 1;

            if (depth > 0) sb.Append(new string(' ', spaceLength));
            if (depth > 0) sb.Append('A');
            if (depth > 0) sb.Append(new string(' ', lineLength));
            sb.AppendLine(new string('-', space + 4));

            if (depth > 0) sb.Append(new string(' ', spaceLength));
            if (depth > 0) sb.Append('|');
            if (depth > 0) sb.Append(new string('-', lineLength));
            sb.AppendLine($"| {tree.Value!.ToString()!.PadRight(space)} |");

            if (depth > 0) sb.Append(new string(' ', spaceLength));
            if (depth > 0) sb.Append('V');
            if (depth > 0) sb.Append(new string(' ', lineLength));
            sb.AppendLine(new string('-', space + 4));

            if (tree.Right != null) sb.Append(ExportTree(tree.Right, space, depth - 1, delimeterCoef));
            return sb.ToString();
        }

        private bool CheckNickname(string? nickname)
        {
            if (nickname == null || nickname.Length == 0)
            {
                Console.WriteLine("Invalid input!");
                return false;
            }
            else if (nickname.All(x => !char.IsLetter(x)))
            {
                Console.WriteLine("Nickname must contain at least one alphabetical character!");
                return false;
            }
            else if (nickname.Any(x => char.IsWhiteSpace(x)))
            {
                Console.WriteLine("Nickname cannot contain white characters!");
                return false;
            }
            else if (_db.Players.Any(x => x.Nickname.Equals(nickname)))
            {
                Console.WriteLine("Player with given nickname already exist!");
                return false;
            }
            return true;
        }

        private bool HandleError(string message)
        {
            Console.WriteLine(message);
            return false;
        }
    }
}
