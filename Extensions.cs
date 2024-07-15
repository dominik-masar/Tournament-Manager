namespace TournamentManager
{
    static class Extensions
    {
        // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            var meh = new Random();
            while (n > 1)
            {
                n--;
                int k = meh.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}
