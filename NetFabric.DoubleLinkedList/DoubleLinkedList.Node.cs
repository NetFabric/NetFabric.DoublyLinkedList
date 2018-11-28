namespace NetFabric
{
    public sealed partial class DoubleLinkedList<T>
    {
        public sealed class Node
        {
            public DoubleLinkedList<T> List { get; private set; }

            public Node Next { get; internal set; }

            public Node Previous { get; internal set; }

            public T Value { get; set; }

            internal Node(DoubleLinkedList<T> list)
            {
                List = list;
            }

            internal void Invalidate()
            {
                List = null;
                Next = null;
                Previous = null;
            }
        }
    }
}
