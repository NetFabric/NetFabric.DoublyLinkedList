namespace NetFabric
{
    public sealed partial class DoubleLinkedList<T>
    {
        public sealed class Node
        {
            public DoubleLinkedList<T> List { get; internal set; }

            public Node Next { get; internal set; }

            public Node Previous { get; internal set; }

            public T Value { get; set; }

            internal void Invalidate()
            {
                List = null;
                Next = null;
                Previous = null;
            }
        }
    }
}
