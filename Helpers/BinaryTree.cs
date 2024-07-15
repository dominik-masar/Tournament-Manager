namespace TournamentManager.Helpers
{
    public class BinaryTree<T>
    {
        public T Value { get; set; }
        public BinaryTree<T>? Left { get; set; }
        public BinaryTree<T>? Right { get; set; }

        public BinaryTree(IEnumerable<T> values) : this(values, 0) { }

        private BinaryTree(IEnumerable<T> values, int index)
        {
            Value = values.ElementAt(index);
            Load(values, index);
        }

        public void Load(IEnumerable<T> values, int index)
        {
            if (index * 2 + 1 < values.Count())
            {
                Left = new BinaryTree<T>(values, index * 2 + 1);
            }
            if (index * 2 + 2 < values.Count())
            {
                Right = new BinaryTree<T>(values, index * 2 + 2);
            }
        }
    }
}
