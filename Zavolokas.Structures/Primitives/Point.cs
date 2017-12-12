namespace Zavolokas.Structures
{
    public struct Point
    {
        public int X;
        public int Y;

        public static Point Empty = new Point(true);
        public bool IsEmpty
        {
            get { return _isEmpty; }
        }

        private readonly bool _isEmpty;

        private Point(bool empty)
        {
            X = 0;
            Y = 0;
            _isEmpty = empty;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
            _isEmpty = false;
        }

        internal Point(Vector2D v)
        {
            X = (int)v.X;
            Y = (int)v.Y;
            _isEmpty = false;
        }

        public override string ToString()
        {
            return $"Point({X}:{Y})";
        }
    }
}
