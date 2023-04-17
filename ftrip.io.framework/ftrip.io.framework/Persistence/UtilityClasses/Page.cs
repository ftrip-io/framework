namespace ftrip.io.framework.Persistence.UtilityClasses
{
    public class Page
    {
        public int Number { get; set; }
        public int Size { get; set; }

        public Page()
        {
        }

        public Page(int number, int size)
        {
            Number = number;
            Size = size;
        }

        public static Page Max
        {
            get => new Page
            {
                Size = int.MaxValue,
                Number = 1
            };
        }
    }
}