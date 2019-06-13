namespace Collections.Pair
{
    public class Pair<T, U>
    {
        public T First { get; private set; }
        public U Second { get; private set; }

        public Pair(T first, U second)
        {
            First = first;
            Second = second;
        }
    }

    public static class Pair
    {
        public static Pair<T, U> Create<T, U>(T first, U second)
        {
            return new Pair<T, U>(first, second);
        }
    }
}
