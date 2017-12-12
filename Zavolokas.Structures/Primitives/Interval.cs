
namespace Zavolokas.Structures
{
    public struct Interval
    {
        public static Interval Empty = new Interval { IsEmpty = true };
        
        public int Start;
        public int End;
        public bool IsEmpty { get; private set; }

        public Interval Intersect(Interval interval)
        {
            if (IsEmpty || interval.IsEmpty) return Empty;
            if (Start > interval.End || End < interval.Start) return Empty;

            int maxStart = Start > interval.Start ? Start : interval.Start;
            int minEnd = End < interval.End ? End : interval.End;

            return new Interval { Start = maxStart, End = minEnd };
        }

        public Interval[] Join(Interval interval)
        {
            if (IsEmpty) return new[] { interval };
            if (interval.IsEmpty) return new[] { this };

            // L =======
            // R    ========
            if (Start < interval.Start && (interval.Start <= End && End <= interval.End))
                return new[] { new Interval() { Start = this.Start, End = interval.End } };

            // L    ========
            // R ======
            if (interval.Start <= Start && (Start <= interval.End && interval.End < End))
                return new[] { new Interval() { Start = interval.Start, End = End } };

            // L =====  ===
            // R  ===   ===
            if (Start <= interval.Start && interval.End <= End)
                return new[] { this };

            // L  ===   ===
            // R =====  ===
            if (interval.Start <= Start && End <= interval.End)
                return new[] { interval };

            if (interval.Start < Start)
                return new[] { interval, this };

            return new[] { this, interval };
        }

        public Interval[] Substract(Interval interval)
        {
            if (IsEmpty) return new[] { Empty };
            if (interval.IsEmpty) return new[] { this };

            // L =======
            // R    ========
            if (Start < interval.Start && (interval.Start <= End && End <= interval.End))
                return new[] { new Interval() { Start = this.Start, End = interval.Start - 1 } };

            // L    ========
            // R ======
            if (interval.Start <= Start && (Start <= interval.End && interval.End < End))
                return new[] { new Interval() { Start = interval.End + 1, End = End } };

            // L ===
            // R ===
            if (interval.Start <= Start && End <= interval.End)
                return new[] { Empty };

            // L ========
            // R   ===
            if (Start < interval.Start && interval.End < End)
                return new[]
                {
                    new Interval() { Start = Start, End = interval.Start-1 },
                    new Interval() { Start = interval.End+1, End = End }
                };

            return new[] { this };
        }

#if DEBUG
        public override string ToString()
        {
            return IsEmpty ? "Empty" : $"[{Start}, {End}]";
        }
#endif
    }
}