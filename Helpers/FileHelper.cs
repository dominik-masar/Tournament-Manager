namespace TournamentManager.Helpers
{
    public static class FileHelper
    {
        public static void CreateFile(string path)
        {
            if (File.Exists(path))
            {
                return;
            }

            using var fs = File.Create(path);
        }
    }
}
