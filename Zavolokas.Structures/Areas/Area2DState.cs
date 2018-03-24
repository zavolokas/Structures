namespace Zavolokas.Structures
{
    public class Area2DState
    {
        public int[] LastElementInRowAbsoluteIndex { get; set; }
        public int[] RowIndexesTable { get; set; }
        public IntervalState[] RowsData { get; set; }
        public int OffsX { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int ElementsCount { get; set; }
    }

    public class IntervalState
    {
        public int[] IntervalLastElementIndexes { get; set; }
        public int[] TotalSpacesCountBeforeIntervals { get; set; }
        public int[] IntervalLastElementPositions { get; set; }
        public int ElementsCount { get; set; }
        public int LastElementAbsoluteIndex { get; set; }
        public int OffsY { get; set; }
    }
}