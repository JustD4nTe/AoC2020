namespace AoC2020.Day23
{
    public class Cup
    {
        public int Value { get; set; }
        public Cup Next { get; set; }
        public Cup Previous { get; set; }

        public Cup(int value)
        {
            Value = value;
        }

        public Cup(int value, Cup previous) : this(value)
        {
            Previous = previous;
            Previous.Next = this;
        }
    }
}