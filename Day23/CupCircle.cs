namespace AoC2020.Day23
{
    public class CupCircle
    {
        public Cup Head { get; set; }
        public Cup Tail { get; set; }

        private readonly Cup[] _arr;

        public CupCircle(int firstValue)
        {
            _arr = new Cup[1_000_000];

            _arr[firstValue - 1] = new(firstValue);
            Head = _arr[firstValue - 1];

            Head.Previous = Tail;
            Head.Next = Tail;

            Tail = Head;

            Tail.Previous = Head;
            Tail.Next = Head;
        }

        public void Insert(int value)
        {
            _arr[value - 1] = new(value, Tail);

            Tail.Next = _arr[value - 1];
            Tail = Tail.Next;

            Head.Previous = Tail;
            Tail.Next = Head;
        }

        public Cup Find(int value) => _arr[value - 1];
    }
}