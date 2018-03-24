using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zavolokas.Structures
{
    /// <summary>
    /// Represents an area in 2D space. Allows to 
    /// - enumerate trough each its point uniformally.
    /// Invariant:
    /// - ElementsCount represenats the amount of points in the area(which has a positive value or 0).
    /// - IsEmpty indicaes that there is no Points in the area.
    /// - Indexer returns a corresponding point when it gets an index as an argument or an index when it gets a point.
    /// </summary>
    public class Area2D
    {
        /// <summary>
        /// The empty area.
        /// </summary>
        public static readonly Area2D Empty = new Area2D(0, 0, 0, 0);

        /// <summary>
        /// Contains absolute indexes of the last element in the row.
        /// For the following matrix:
        /// 
        /// 0,0,0,0,0,0,0,0,0
        /// 0,1,1,0,1,1,0,0,1
        /// 1,1,1,1,0,0,1,1,1
        /// 0,1,1,0,0,0,1,1,0
        /// 0,0,0,0,0,0,0,0,0
        /// 0,0,0,0,1,1,1,0,0
        /// 0,0,0,0,0,0,0,0,0
        /// 
        /// It will contain: 4, 11, 15, 18
        /// </summary>
        private readonly int[] _lastElementInRowAbsoluteIndex;

        /// <summary>
        /// Contains indexes of the data rows.
        /// For the following matrix:
        /// 
        /// 0,0,0,0,0,0,0,0,0
        /// 0,1,1,0,1,1,0,0,1
        /// 1,1,1,1,0,0,1,1,1
        /// 0,1,1,0,0,0,1,1,0
        /// 0,0,0,0,0,0,0,0,0
        /// 0,0,0,0,1,1,1,0,0
        /// 0,0,0,0,0,0,0,0,0
        /// 
        /// It will contain: 0, 1, 2, 4
        /// First three go together than one empty row and than one more.
        /// That is reflected in the indexes.
        /// </summary>
        private readonly int[] _rowIndexesTable;

        private readonly IntervalsData[] _rowsData;
        //private readonly Dictionary<int,int> _rowLocGlobIndexMap = new Dictionary<int, int>();

        /// <summary>
        /// Horisontal offset of the area.
        /// For the following matrix initialized in position (3,1):
        /// 
        /// 0,0,0,0,0,0,0,0,0
        /// 0,1,1,0,1,1,0,0,1
        /// 1,1,1,1,0,0,1,1,1
        /// 0,1,1,0,0,0,1,1,0
        /// 0,0,0,0,0,0,0,0,0
        /// 0,0,0,0,1,1,1,0,0
        /// 0,0,0,0,0,0,0,0,0
        /// 
        /// For row #1 will be: 3
        /// For row #2 will be: 3
        /// For row #5 will be: 3
        /// </summary>
        private readonly int _offsX;

        public static Area2D Create(int x, int y, int width, int height)
        {
            return new Area2D(x, y, width, height);
        }

        public static Area2D Create(int x, int y, byte[][] markup)
        {
            return new Area2D(x, y, markup);
        }

        public static Area2D RestoreFrom(Area2DState state)
        {
            return new Area2D(state);
        }

        internal Area2D(int x, int y, int width, int height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentException();

            Bound = new Rectangle(x, y, width, height);
            _lastElementInRowAbsoluteIndex = new int[height];
            _rowsData = new IntervalsData[height];
            _rowIndexesTable = new int[height];
            _offsX = x;

            for (var i = 0; i < height; i++)
            {
                var row = new IntervalsData
                {
                    OffsY = i,
                    ElementsCount = width,
                    LastElementAbsoluteIndex = i * width + (width - 1),
                    TotalSpacesCountBeforeIntervals = new[] { 0 },
                    IntervalLastElementPositions = new[] { width - 1 },
                    IntervalLastElementIndexes = new[] { width - 1 }

                };
                _lastElementInRowAbsoluteIndex[i] = row.LastElementAbsoluteIndex;
                _rowIndexesTable[i] = i;
                _rowsData[i] = row;
                //_rowLocGlobIndexMap.Add(row.OffsY,i);
            }

            ElementsCount = width * height;
        }

        internal Area2D(int x, int y, byte[][] markup)
        {
            if (markup == null)
                throw new ArgumentNullException(nameof(markup));

            const byte INTERVAL = 1;
            const byte SPACES = 2;

            var absElementIndex = 0;

            var dataRows = new List<IntervalsData>();
            var rowIndexes = new List<int>(markup.Length);
            var lastElementInRowAbsoluteIndex = new List<int>(markup.Length);
            _offsX = x;

            var xl = markup[0].Length;
            var xr = 0;
            var yt = markup.Length;
            var yb = 0;

            for (var j = 0; j < markup.Length; j++)
            {
                var row = markup[j];
                var firstElementPos = -1;
                var state = SPACES;
                var capacity = (int)(markup.Length * 0.4);
                var intervals = new List<int>(capacity);
                var spaces = new List<int>(capacity) { 0 };
                var elementCount = 0;
                for (var i = 0; i < row.Length; i++)
                {
                    var element = row[i];

                    if (firstElementPos == -1 && element != 0)
                    {
                        firstElementPos = i;
                    }

                    if (element == 1)
                    {
                        elementCount++;
                        absElementIndex++;

                        if (xl > i) xl = i;
                        if (xr < i) xr = i;
                        if (yt > j) yt = j;
                        if (yb < j) yb = j;
                    }

                    if (element == 1 && state != INTERVAL)
                    {
                        intervals.Add(i);
                        state = INTERVAL;
                    }
                    else if (element == 0 && state != SPACES)
                    {
                        var spacesAmount = spaces.Count > 0 ? spaces[spaces.Count - 1] : 0;
                        spaces.Add(spacesAmount + 1);
                        state = SPACES;
                    }
                    else if (element == 1)
                    {
                        intervals[intervals.Count - 1]++;
                    }
                    else
                    {
                        spaces[spaces.Count - 1]++;
                    }
                }

                if (elementCount > 0)
                {
                    var ilei = new int[intervals.Count];
                    for (var i = 0; i < ilei.Length; i++)
                    {
                        ilei[i] = intervals[i] - spaces[i];
                    }

                    var datarow = new IntervalsData
                    {
                        LastElementAbsoluteIndex = absElementIndex - 1,
                        ElementsCount = elementCount,
                        OffsY = j - yt,
                        IntervalLastElementPositions = intervals.ToArray(),
                        TotalSpacesCountBeforeIntervals = spaces.ToArray(),
                        IntervalLastElementIndexes = ilei,
                    };
                    dataRows.Add(datarow);
                    //_rowLocGlobIndexMap.Add(datarow.OffsY, dataRows.Count-1);

                    rowIndexes.Add(j);
                    lastElementInRowAbsoluteIndex.Add(absElementIndex - 1);
                }
            }

            _lastElementInRowAbsoluteIndex = lastElementInRowAbsoluteIndex.ToArray();
            Bound = new Rectangle(xl + x, yt + y, xr - xl + 1, yb - yt + 1);
            _rowIndexesTable = rowIndexes.Select((yy) => yy - yt).ToArray();
            _rowsData = dataRows.ToArray();
            ElementsCount = absElementIndex;
        }

        private Area2D(Point position, IntervalsData[] rowsData)
        {
            this._rowsData = rowsData;
            _offsX = position.X;
            _lastElementInRowAbsoluteIndex = new int[_rowsData.Length];
            _rowIndexesTable = new int[_rowsData.Length];

            int left = int.MaxValue;
            int right = int.MinValue;

            for (int i = 0; i < _rowsData.Length; i++)
            {
                var row = _rowsData[i];
                var lastPos = row.IntervalLastElementPositions[row.IntervalLastElementPositions.Length - 1];
                var firstPos = row.TotalSpacesCountBeforeIntervals[0];
                if (right < lastPos) right = lastPos;
                if (left > firstPos) left = firstPos;

                ElementsCount += row.ElementsCount;
                _rowsData[i].LastElementAbsoluteIndex = ElementsCount - 1;
                _lastElementInRowAbsoluteIndex[i] = ElementsCount - 1;
                _rowIndexesTable[i] = row.OffsY;
                //_rowLocGlobIndexMap.Add(row.OffsY, i);
            }

            int width = right - left + 1;
            int height = _rowsData[_rowsData.Length - 1].OffsY + 1;

            Bound = new Rectangle(left + position.X, position.Y, width, height);
        }

        internal Area2D(Area2DState state)
        {
            Bound = new Rectangle(state.X, state.Y, state.Width, state.Height);
            _lastElementInRowAbsoluteIndex = state.LastElementInRowAbsoluteIndex;
            _offsX = state.OffsX;
            _rowIndexesTable = state.RowIndexesTable;
            _rowsData = state.RowsData.Select(intervalState => new IntervalsData(intervalState)).ToArray();
            ElementsCount = state.ElementsCount;
        }

        public Area2DState GetState()
        {
            return new Area2DState
            {
                LastElementInRowAbsoluteIndex = _lastElementInRowAbsoluteIndex,
                OffsX = _offsX,
                RowIndexesTable = _rowIndexesTable,
                X = Bound.X,
                Y = Bound.Y,
                Width = Bound.Width,
                Height = Bound.Height,
                RowsData = _rowsData.Select(rd => rd.GetState()).ToArray(),
                ElementsCount = ElementsCount
            };
        }

        /// <summary>
        /// Gets the count of the points that the area contains.
        /// Contract:
        /// - can not have a negative value
        /// </summary>
        /// <value>
        /// The points count.
        /// </value>
        public int ElementsCount { get; private set; }

        /// <summary>
        /// Gets a <see cref="Point"/> that the area contains at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The <see cref="Point"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">When the index less than zero and equal or greater than the ElementsCount.</exception>
        public Point this[int index]
        {
            get
            {
                if (index < 0 || ElementsCount <= index)
                    throw new IndexOutOfRangeException(nameof(index));

                // _lastElementInRowAbsoluteIndex contains indexes of last element in the row.
                // Using _lastElementInRowAbsoluteIndex we find the needed row.
                int rowIndex = FindIntervalIndex(_lastElementInRowAbsoluteIndex, index);
                var row = _rowsData[rowIndex];

                // Calculate the index of the element in the row(only)
                var rowFirstElAbsIndex = row.LastElementAbsoluteIndex - (row.ElementsCount - 1);
                var rowElementIndex = index - rowFirstElAbsIndex;

                // The index of the element in the row could be the needed X coordinate,
                // but there could be spaces between the elements as well. That is why we
                // need to find out the amount of spaces our element.
                // In order to do that we can use the SpacesAmount table if we know the
                // index of the elemnts interval within the row where our index relates to index.
                var rowIntervalIndex = FindIntervalIndex(row.IntervalLastElementIndexes, rowElementIndex);
                var x = rowElementIndex + row.TotalSpacesCountBeforeIntervals[rowIntervalIndex] + _offsX;
                int y = row.OffsY + Bound.Y;
                return new Point(x, y);
            }
        }

        /// <summary>
        /// Populates provided array with the indexes of the points at the corresponding/mapped area.
        /// </summary>
        /// <param name="mappedPointIndexes">The mapped point indexes.</param>
        /// <param name="mappedAreaWidth">Width of the corresponding/mapped area.</param>
        /// <param name="fillFromEnd">if set to <c>true</c> fills the indexes started from the end.</param>
        public void FillMappedPointsIndexes(int[] mappedPointIndexes, int mappedAreaWidth, bool fillFromEnd = false)
        {
            if (mappedPointIndexes.Length != ElementsCount)
                throw new NotImplementedException("not implemented FillMappedPointsIndexes func when the size of array (for filled points) not equal to the area ElementsCount");

            var pointsAmount = ElementsCount;
            const int NotDividableMinAmountElements = 80;

            // Decide on how many partitions we should divade the processing
            // of the elements.
            var partsCount = pointsAmount > NotDividableMinAmountElements
                ? Environment.ProcessorCount
                : 1;

            var partSize = (int)(pointsAmount / partsCount);

            Parallel.For(0, partsCount, partIndex =>
            {
                var firstPointIndex = partIndex * partSize;
                var lastPointIndex = firstPointIndex + partSize - 1;
                if (partIndex == partsCount - 1) lastPointIndex = pointsAmount - 1;
                if (lastPointIndex > pointsAmount) lastPointIndex = pointsAmount - 1;

                for (var pointIndex = firstPointIndex; pointIndex <= lastPointIndex; pointIndex++)
                {
                    int index = fillFromEnd ? ElementsCount - pointIndex - 1 : pointIndex;
                    // _lastElementInRowAbsoluteIndex contains indexes of last element in the row.
                    // Using _lastElementInRowAbsoluteIndex we find the needed row.
                    int rowIndex = FindIntervalIndex(_lastElementInRowAbsoluteIndex, index);
                    var row = _rowsData[rowIndex];

                    // Calculate the index of the element in the row(only)
                    var rowFirstElAbsIndex = row.LastElementAbsoluteIndex - (row.ElementsCount - 1);
                    var rowElementIndex = index - rowFirstElAbsIndex;

                    // The index of the element in the row could be the needed X coordinate,
                    // but there could be spaces between the elements as well. That is why we
                    // need to find out the amount of spaces our element.
                    // In order to do that we can use the SpacesAmount table if we know the
                    // index of the elemnts interval within the row where our index relates to index.
                    var rowIntervalIndex = FindIntervalIndex(row.IntervalLastElementIndexes, rowElementIndex);
                    var x = rowElementIndex + row.TotalSpacesCountBeforeIntervals[rowIntervalIndex] + _offsX;
                    int y = row.OffsY + Bound.Y;

                    mappedPointIndexes[index] = y * mappedAreaWidth + x;
                }
            });
        }

        /// <summary>
        /// Gets an index of the specified point if the area contains it, otherwise it returns -1.
        /// </summary>
        /// <value>
        /// The <see cref="System.Int32"/>.
        /// </value>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public int GetPointIndex(Point point)
        {
            return GetPointIndex(point.X, point.Y);
        }

        /// <summary>
        /// Gets an index of the specified point if the area contains it, otherwise it returns -1.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        /// <value>
        /// The <see cref="System.Int32" />.
        /// </value>
        public int GetPointIndex(int x, int y)
        {
            // ensure that the point lies within the bounds of the area, otherwise return index -1
            if (x < Bound.X || (Bound.X + Bound.Width) < x ||
                y < Bound.Y || (Bound.Y + Bound.Height) < y)
                return -1;

            // We store only significant rows(that contain elements) of the area, that is why
            // we need to convert the coordinate of the point to the area space.
            var pointY = y - Bound.Y;

            // Get the row data by index of the row in the area coordinate space. 
            // Row doesn't contain elements when it is not found in the _rowIndexesTable.
            var rowIndex = Array.BinarySearch(_rowIndexesTable, pointY);
            if (rowIndex < 0)
                return -1;
            var row = _rowsData[rowIndex];

            // The offs X is the offset where the area was placed, so
            // the information before this offset is not saved in the area.
            // But the point.X coordinate is the coordinate in the whole area space,
            // that is why we transform it to our area of interest.
            var pointX = x - _offsX;

            // We need to ensure that the point resides in one of the intervals of elements.
            // For that matter we obtain the index of the possible interval and establish its
            // start(ifepos) and end(ilepos) positions in the area row coordinate space.
            // ifepos (Interval First Element Position) - position of the first element in the interval in the row space (starting from 0)
            // ilepos (Interval Last Element Position) - position of the last element in the interval in the row space (starting from 0)
            var intervalIndex = FindIntervalIndex(row.IntervalLastElementPositions, pointX);
            var prevElementsCout = intervalIndex > 0 ? row.IntervalLastElementIndexes[intervalIndex - 1] + 1 : 0;
            var ifepos = row.TotalSpacesCountBeforeIntervals[intervalIndex] + prevElementsCout;
            var ilepos = row.IntervalLastElementPositions[intervalIndex];

            // Check whether the pointX lies within the interval bounds.
            if (pointX < ifepos || ilepos < pointX)
                return -1;

            // It does. So now we need to calculate the absolute index of 
            // the element at which refers the point.
            // What is the index of the point within the interval? 
            var intervalPointIndex = pointX - ifepos;
            var rowFirstElAbsIndex = row.LastElementAbsoluteIndex - (row.ElementsCount - 1);
            var index = rowFirstElAbsIndex + prevElementsCout + intervalPointIndex;
            return index;
        }

        /// <summary>
        /// Gets the points enumerator.
        /// </summary>
        /// <value>
        /// The points enumerator.
        /// </value>
        public IEnumerable<Point> Points
        {
            get
            {
                for (int i = 0; i < ElementsCount; i++)
                {
                    yield return this[i];
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether there are no points in the area.
        /// </summary>
        /// <value>
        ///   <c>true</c> if there are no points; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty
        {
            get { return ElementsCount == 0; }
        }

        /// <summary>
        /// Gets the boundary of the area.
        /// </summary>
        /// <value>
        /// The boundary.
        /// </value>
        public Rectangle Bound { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"Area[B:{Bound.X}, {Bound.Y}, {Bound.Width}, {Bound.Height}; P: {ElementsCount}]";
        }

        /// <summary>
        /// Determines whether the area instance is same as the specified area.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <returns>
        ///   <c>true</c> if area is same as the specified area; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">area</exception>
        public bool IsSameAs(Area2D area)
        {
            if (area == null)
                throw new ArgumentNullException(nameof(area));

            if (IsEmpty && IsEmpty == area.IsEmpty)
                return true;

            if (ElementsCount != area.ElementsCount)
                return false;

            if (Bound.X != area.Bound.X ||
                Bound.Y != area.Bound.Y ||
                Bound.Width != area.Bound.Width ||
                Bound.Height != area.Bound.Height)
                return false;

            if (_lastElementInRowAbsoluteIndex.Length != area._lastElementInRowAbsoluteIndex.Length)
                return false;

            for (int i = 0; i < _lastElementInRowAbsoluteIndex.Length; i++)
            {
                if (_lastElementInRowAbsoluteIndex[i] != area._lastElementInRowAbsoluteIndex[i])
                    return false;
            }

            if (_rowIndexesTable.Length != area._rowIndexesTable.Length)
                return false;

            for (int i = 0; i < _rowIndexesTable.Length; i++)
            {
                if (_rowIndexesTable[i] != area._rowIndexesTable[i])
                    return false;
            }

            if (_rowsData.Length != area._rowsData.Length)
                return false;

            for (int i = 0; i < _rowsData.Length; i++)
            {
                var row1 = _rowsData[i];
                var row2 = area._rowsData[i];

                if (row1.ElementsCount != row2.ElementsCount)
                    return false;

                if (row1.LastElementAbsoluteIndex != row2.LastElementAbsoluteIndex)
                    return false;

                if (row1.TotalSpacesCountBeforeIntervals.Length != row2.TotalSpacesCountBeforeIntervals.Length)
                {
                    int shortestLength = row1.TotalSpacesCountBeforeIntervals.Length < row2.TotalSpacesCountBeforeIntervals.Length
                        ? row1.TotalSpacesCountBeforeIntervals.Length
                        : row2.TotalSpacesCountBeforeIntervals.Length;

                    for (int j = 0; j < shortestLength; j++)
                    {
                        if (row1.TotalSpacesCountBeforeIntervals[j] != row2.TotalSpacesCountBeforeIntervals[j])
                            return false;
                    }
                }

                if (row1.IntervalLastElementIndexes.Length != row2.IntervalLastElementIndexes.Length)
                {
                    for (int j = 0; j < row1.IntervalLastElementIndexes.Length; j++)
                    {
                        if (row1.IntervalLastElementIndexes[j] != row2.IntervalLastElementIndexes[j])
                            return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Joins area with the specified area B.
        /// </summary>
        /// <param name="areaB">The areaB.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public Area2D Join(Area2D areaB)
        {
            if (areaB == null)
                throw new ArgumentNullException(nameof(areaB));

            var areaA = this;

            if (areaA.IsEmpty && areaB.IsEmpty)
                return Empty;

            if (areaB.IsEmpty)
                return areaA;

            if (areaA.IsEmpty)
                return areaB;

            // Get area of interest. 
            int ay1 = areaA.Bound.Y;
            int by1 = areaB.Bound.Y;
            int ay2 = areaA.Bound.Y + areaA.Bound.Height - 1;
            int by2 = areaB.Bound.Y + areaB.Bound.Height - 1;
            var areasYInterval = new Interval
            {
                Start = ay1 < by1 ? ay1 : by1,
                End = ay2 > by2 ? ay2 : by2
            };

            // Find out - how data rows from the both areas area correspond to each other
            var correspondDataRowIndexes = new List<Tuple<int?, int?>>();
            #region Build DataRow correspondance
            int aFirstRowAbsIndex = areaA.Bound.Y;
            int bFirstRowAbsIndex = areaB.Bound.Y;
            int aDataRowIndex = 0;
            int bDataRowIndex = 0;

            for (int absRowPos = areasYInterval.Start; absRowPos <= areasYInterval.End; absRowPos++)
            {
                int aAbsDataRowIndex = areaA._rowsData[aDataRowIndex].OffsY + aFirstRowAbsIndex;
                int bAbsDataRowIndex = areaB._rowsData[bDataRowIndex].OffsY + bFirstRowAbsIndex;

                // we are interested only in the data rows that present iether in the area A or B
                if (absRowPos == aAbsDataRowIndex || absRowPos == bAbsDataRowIndex)
                {
                    // by default we assume that we don't have a correspondent data row in the second area
                    int? aCorrespondentDataRowIndex = null;
                    int? bCorrespondentDataRowIndex = null;

                    if (absRowPos == aAbsDataRowIndex)
                    {
                        aCorrespondentDataRowIndex = aDataRowIndex;
                        if (aDataRowIndex < areaA._rowsData.Length - 1)
                            aDataRowIndex++;
                    }

                    if (absRowPos == bAbsDataRowIndex)
                    {
                        bCorrespondentDataRowIndex = bDataRowIndex;
                        if (bDataRowIndex < areaB._rowsData.Length - 1)
                            bDataRowIndex++;
                    }

                    if (aCorrespondentDataRowIndex != null || bCorrespondentDataRowIndex != null)
                    {
                        var correspondense = new Tuple<int?, int?>(aCorrespondentDataRowIndex, bCorrespondentDataRowIndex);
                        correspondDataRowIndexes.Add(correspondense);
                    }
                }
            }
            #endregion

            // Now, since we know the data rows of interest and data 
            // rows that we should combine with correspondent rows,
            // we going to do that

            // Here we will track the X pos for the new result area.
            int mostLeftPosition = int.MaxValue;

            // Data rows we will turn into intervals first to perform 'join' easily.
            // Perform row by row 'join' operation and store all the result intervals
            // per row to create a new result area later.
            var intervalRows = new List<List<Interval>>(correspondDataRowIndexes.Count);
            #region Calculate the intervals for each row
            for (int rowIndex = 0; rowIndex < correspondDataRowIndexes.Count; rowIndex++)
            {
                List<Interval> intervalsC;
                var t = correspondDataRowIndexes[rowIndex];

                // Perform Join only when both data rows are present from
                // the both area A and B.
                if (t.Item1.HasValue && t.Item2.HasValue)
                {
                    var rowA = areaA._rowsData[t.Item1.Value];
                    var rowB = areaB._rowsData[t.Item2.Value];

                    // Since all the intervals a positive and only Area offset can move
                    // intervals to a negative dimensionm we pass _offsX to align the intervals
                    // respectivelly to 0 position. So we will get intervals that start at negative zone
                    // as well. This we will need to take into account when create a result Area.
                    var intervalsA = rowA.GetIntervals(areaA._offsX);
                    var intervalsB = rowB.GetIntervals(areaB._offsX);
                    intervalsC = new List<Interval>(100);

                    #region Join ( intervalsC = intervalsA + intervalsB)
                    //throw new NotImplementedException();
                    int aindex = 0;
                    int bindex = 0;
                    var currentInterval = Interval.Empty;

                    while (aindex < intervalsA.Length || bindex < intervalsB.Length)
                    {
                        // we will join the closest interval from the to interval sets to the current
                        Interval aInterval = aindex < intervalsA.Length ? intervalsA[aindex] : Interval.Empty;
                        Interval bInterval = bindex < intervalsB.Length ? intervalsB[bindex] : Interval.Empty;
                        Interval closestInterval;
                        if (!(aInterval.IsEmpty || bInterval.IsEmpty))
                        {
                            if (aInterval.Start < bInterval.Start)
                            {
                                closestInterval = aInterval;
                                aindex++;
                            }
                            else
                            {
                                closestInterval = bInterval;
                                bindex++;
                            }
                        }
                        else
                        {
                            if (!aInterval.IsEmpty)
                            {
                                closestInterval = aInterval;
                                aindex++;
                            }
                            else
                            {
                                closestInterval = bInterval;
                                bindex++;
                            }
                        }

                        var joinresult = currentInterval.Join(closestInterval);
                        if (joinresult.Length > 1) intervalsC.Add(joinresult[0]);
                        currentInterval = joinresult[joinresult.Length - 1];
                    }
                    intervalsC.Add(currentInterval);
                    #endregion
                }
                else
                {
                    // No correspondent data rows -> result is intervals from an area that
                    // has a data row at this position.
                    var row = t.Item1.HasValue ? areaA._rowsData[t.Item1.Value] : areaB._rowsData[t.Item2.Value];
                    var offsX = t.Item1.HasValue ? areaA._offsX : areaB._offsX;
                    var intervals = row.GetIntervals(offsX);
                    intervalsC = intervals.ToList();
                }

                // search for the most left position in the current intervals.
                for (int i = 0; i < intervalsC.Count; i++)
                {
                    var intervalC = intervalsC[i];
                    if (intervalC.Start < mostLeftPosition) mostLeftPosition = intervalC.Start;
                }
                intervalRows.Add(intervalsC);
            }
            #endregion

            int? ypos = null;
            var cIntervalsDatas = new List<IntervalsData>(areaA.Bound.Height);
            #region Convert intervals to Data rows
            for (var rowIndex = 0; rowIndex < correspondDataRowIndexes.Count; rowIndex++)
            {
                var t = correspondDataRowIndexes[rowIndex];
                int absRowIndex;
                if (t.Item1.HasValue)
                {
                    var rowA = areaA._rowsData[t.Item1.Value];
                    absRowIndex = rowA.OffsY + areaA.Bound.Y;
                }
                else
                {
                    var rowB = areaB._rowsData[t.Item2.Value];
                    absRowIndex = rowB.OffsY + areaB.Bound.Y;
                }
                var cIntervals = intervalRows[rowIndex];

                if (cIntervals.Count > 0)
                {
                    if (ypos == null)
                    {
                        ypos = absRowIndex;
                    }

                    var adjustedIntervals = new List<Interval>(cIntervals.Count);
                    for (int i = 0; i < cIntervals.Count; i++)
                    {
                        var interval = cIntervals[i];
                        adjustedIntervals.Add(new Interval
                        {
                            Start = interval.Start - mostLeftPosition,
                            End = interval.End - mostLeftPosition
                        });
                    }

                    var rowData = new IntervalsData(absRowIndex - ypos.Value, adjustedIntervals.ToArray());
                    cIntervalsDatas.Add(rowData);
                }
            }
            #endregion

            Area2D result = Empty;
            if (cIntervalsDatas.Count > 0)
            {
                //correct position.
                var position = new Point(mostLeftPosition, ypos.Value);
                result = new Area2D(position, cIntervalsDatas.ToArray());
            }
            return result;
        }

        /// <summary>
        /// Returns an area which is a result of substraction of a specified area from the original.
        /// </summary>
        /// <param name="areaB">The area b.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        /// <remarks>
        /// empty - area = empty
        /// area - empty = area
        /// area1 - area2 = area1 when area1 * area2 = empty
        /// area1 - area2 = area3 when area1 * area2 = !empty
        /// area1 - area2 = empty when area1 = area2
        /// area1 - area2 = empty when area2 includes area1
        /// </remarks>
        public Area2D Substract(Area2D areaB)
        {
            if (areaB == null)
                throw new ArgumentNullException(nameof(areaB));

            var areaA = this;

            if (areaB.IsEmpty)
                return areaA;

            if (areaA.IsEmpty)
                return Empty;

            // Find out wether the areas intersect. 
            var aAreaXInterval = new Interval { Start = areaA.Bound.X, End = areaA.Bound.X + areaA.Bound.Width - 1 };
            var aAreaYInterval = new Interval { Start = areaA.Bound.Y, End = areaA.Bound.Y + areaA.Bound.Height - 1 };

            var bAreaXInterval = new Interval { Start = areaB.Bound.X, End = areaB.Bound.X + areaB.Bound.Width - 1 };
            var bAreaYInterval = new Interval { Start = areaB.Bound.Y, End = areaB.Bound.Y + areaB.Bound.Height - 1 };

            // If not - return the original area.
            if (aAreaXInterval.Intersect(bAreaXInterval).IsEmpty ||
                aAreaYInterval.Intersect(bAreaYInterval).IsEmpty)
                return areaA;

            // Find out - which data rows from the second area correspond to each data row of the first one
            var correspondDataRowIndexes = new List<Tuple<int, int?>>();
            #region Build DataRow correspondance
            int a1FirstRowAbsIndex = areaA.Bound.Y;
            int a2FirstRowAbsIndex = areaB.Bound.Y;
            int a1DataRowIndex = 0;
            int a2DataRowIndex = 0;

            for (int absRowPos = aAreaYInterval.Start; absRowPos <= aAreaYInterval.End; absRowPos++)
            {
                // we are interested only in the data rows that present in the first area
                if (absRowPos == areaA._rowsData[a1DataRowIndex].OffsY + a1FirstRowAbsIndex)
                {
                    // we should move the second area index to an actual position
                    // since it can be far behind the current row (absRowPos)
                    while (areaB._rowsData.Length > a2DataRowIndex &&
                           absRowPos > areaB._rowsData[a2DataRowIndex].OffsY + a2FirstRowAbsIndex)
                    {
                        a2DataRowIndex++;
                    }

                    // by default we assume that we don't have a correspondent data row in the second area
                    int? a2CorrespondentDataRowIndex = null;

                    // now we are going to check wether the current data rows correspond
                    if (areaB._rowsData.Length > a2DataRowIndex && absRowPos == areaB._rowsData[a2DataRowIndex].OffsY + a2FirstRowAbsIndex)
                    {
                        a2CorrespondentDataRowIndex = a2DataRowIndex;
                        a2DataRowIndex++;
                    }

                    Tuple<int, int?> correspondense = new Tuple<int, int?>(a1DataRowIndex, a2CorrespondentDataRowIndex);
                    correspondDataRowIndexes.Add(correspondense);
                    a1DataRowIndex++;
                }
            }
            #endregion

            // Now, since we know the data rows of interest and data 
            // rows from which we should extract correspondent rows from other area,
            // we going to do that

            // Here we will track the X pos for the new result area.
            int mostLeftPosition = int.MaxValue;

            // Data rows we will turn into intervals first to perform 'substract' easily.
            // Perform row by row 'substract' operation and store all the result intervals
            // per row to create a new result area later.
            var intervalRows = new List<List<Interval>>(correspondDataRowIndexes.Count);
            #region Calculate the intervals for each row
            for (int rowIndex = 0; rowIndex < correspondDataRowIndexes.Count; rowIndex++)
            {
                List<Interval> intervalsC;

                var t = correspondDataRowIndexes[rowIndex];
                var rowA = areaA._rowsData[t.Item1];

                // Since all the intervals a positive and only Area offset can move
                // intervals to a negative dimensionm we pass _offsX to align the intervals
                // respectivelly to 0 position. So we will get intervals that start at negative zone
                // as well. This we will need to take into account when create a result Area.
                var intervalsA = rowA.GetIntervals(areaA._offsX);

                // Perform substraction only when the current data row from
                // area A has a correspondent data row in the area B.
                if (t.Item2.HasValue)
                {
                    var rowB = areaB._rowsData[t.Item2.Value];
                    var intervalsB = rowB.GetIntervals(areaB._offsX);
                    intervalsC = new List<Interval>(100);

                    #region Substract ( intervalsC = intervalsA - intervalsB)
                    var firstBIntervalIndex = 0;
                    for (var aIntervalIndex = 0; aIntervalIndex < intervalsA.Length; aIntervalIndex++)
                    {
                        var aInterval = intervalsA[aIntervalIndex];
                        var intervalProcessed = false;
                        for (var bIntervalsIndex = firstBIntervalIndex; bIntervalsIndex < intervalsB.Length; bIntervalsIndex++)
                        {
                            var bInterval = intervalsB[bIntervalsIndex];
#if DEBUG
                            // there shouldn't be any ampty interval
                            if (aInterval.IsEmpty || bInterval.IsEmpty)
                            {
                                throw new Exception("interval can not be empty!");
                            }
#endif

                            // L =======
                            // R            ========
                            // add aInterval to the result and break when bInterval is far after the aInterval
                            if (aInterval.End < bInterval.Start)
                            {
                                intervalsC.Add(aInterval);
                                intervalProcessed = true;
                                break;
                            }

                            // L            ========
                            // R =======
                            //increase firstBIntervalIndex and continue when bInterval is far before the aInterval
                            if (bInterval.End < aInterval.Start)
                            {
                                firstBIntervalIndex++;
                                continue;
                            }

                            // L =======
                            // R    ========
                            // The result of substraction can not be reached by the rest of B intervals, 
                            // that is why we put it to the result intervals and break;
                            if (aInterval.Start < bInterval.Start &&
                                (bInterval.Start <= aInterval.End && aInterval.End <= bInterval.End))
                            {
                                var cInterval = aInterval.Substract(bInterval)[0];
                                intervalsC.Add(cInterval);
                                intervalProcessed = true;
                                break;
                            }

                            // L    ========
                            // R ======
                            // The result can be still affected by the other B intervals,
                            // we make it as current and proceed.
                            if (bInterval.Start <= aInterval.Start &&
                                (aInterval.Start <= bInterval.End && bInterval.End < aInterval.End))
                            {
                                aInterval = aInterval.Substract(bInterval)[0];
                                continue;
                            }

                            // L =====  
                            // R  ===  
                            // two intervals will be the result of substracion
                            // The first one can not be reached by the rest of B intervals, 
                            // that is why we put it to the results.
                            // The second one can be still affected by the other B intervals,
                            // we make it as current and proceed.
                            //var intervals = aInterval.Substract(bInterval);
                            if (aInterval.Start < bInterval.Start && bInterval.End < aInterval.End)
                            {
                                var res = aInterval.Substract(bInterval);
                                intervalsC.Add(res[0]);
                                aInterval = res[1];
                                continue;
                            }

                            // L  ===   ===
                            // R =====  ===
                            // the result of substraction in this case is empty interval
                            // so we have nothing to add - simply break.
                            intervalProcessed = true;
                            break;
                        }

                        if (!intervalProcessed) intervalsC.Add(aInterval);
                    }
                    #endregion
                }
                else
                {
                    // No correspondent data rows -> result is intervals from area A.
                    intervalsC = intervalsA.ToList();
                }

                // search for the most left position in the current intervals.
                for (int i = 0; i < intervalsC.Count; i++)
                {
                    var intervalC = intervalsC[i];
                    if (intervalC.Start < mostLeftPosition) mostLeftPosition = intervalC.Start;
                }
                intervalRows.Add(intervalsC);
            }
            #endregion

            int? ypos = null;
            var cIntervalsDatas = new List<IntervalsData>(areaA.Bound.Height);
            #region Convert intervals to Data rows
            for (var rowIndex = 0; rowIndex < correspondDataRowIndexes.Count; rowIndex++)
            {
                var t = correspondDataRowIndexes[rowIndex];
                var rowA = areaA._rowsData[t.Item1];
                var absRowIndex = rowA.OffsY + areaA.Bound.Y;
                var cIntervals = intervalRows[rowIndex];

                if (cIntervals.Count > 0)
                {
                    if (ypos == null)
                    {
                        ypos = absRowIndex;
                    }

                    var adjustedIntervals = new List<Interval>(cIntervals.Count);
                    for (int i = 0; i < cIntervals.Count; i++)
                    {
                        var interval = cIntervals[i];
                        adjustedIntervals.Add(new Interval
                        {
                            Start = interval.Start - mostLeftPosition,
                            End = interval.End - mostLeftPosition
                        });
                    }

                    var rowData = new IntervalsData(absRowIndex - ypos.Value, adjustedIntervals.ToArray());
                    cIntervalsDatas.Add(rowData);
                }
            }
            #endregion

            Area2D result = Empty;
            if (cIntervalsDatas.Count > 0)
            {
                //correct position.
                var position = new Point(mostLeftPosition, ypos.Value);
                result = new Area2D(position, cIntervalsDatas.ToArray());
            }
            return result;
        }

        /// <summary>
        /// Returns an intersection of the two areas.
        /// </summary>
        /// <param name="area">The area b.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        /// <remarks>
        /// area * empty = empty
        /// empty * area = empty
        /// empty * empty = empty
        /// area1 * area2 = empty when no common elements
        /// area1 * area2 = area3 when have common elements
        /// </remarks>
        public Area2D Intersect(Area2D area)
        {
            var areaA = this;
            var areaB = area;

            if (areaB == null)
                throw new ArgumentNullException(nameof(areaB));

            if (areaB.IsEmpty || areaA.IsEmpty)
                return Area2D.Empty;

            if ((areaA.Bound.X < areaB.Bound.X && (areaA.Bound.X + areaA.Bound.Width - 1) < areaB.Bound.X) ||
                (areaB.Bound.X < areaA.Bound.X && (areaB.Bound.X + areaB.Bound.Width - 1) < areaA.Bound.X) ||
                (areaA.Bound.Y < areaB.Bound.Y && (areaA.Bound.Y + areaA.Bound.Height - 1) < areaB.Bound.Y) ||
                (areaB.Bound.Y < areaA.Bound.Y && (areaB.Bound.Y + areaB.Bound.Height - 1) < areaA.Bound.Y))
                return Empty;

            List<IntervalsData> cIntervalsDatas = new List<IntervalsData>(areaA.Bound.Height + areaB.Bound.Height - 1);

            //find out - which rows from both areas correspond to each other?
            var a1 = new Interval { Start = areaA.Bound.Y, End = areaA.Bound.Y + areaA.Bound.Height - 1 };
            var a2 = new Interval { Start = areaB.Bound.Y, End = areaB.Bound.Y + areaB.Bound.Height - 1 };
            var intRows = a1.Intersect(a2);
            var correspondRowIndexes = new List<Tuple<int, int, int>>();

            int a1FirstRowIndex = intRows.Start - areaA.Bound.Y;
            int a2FirstRowIndex = intRows.Start - areaB.Bound.Y;

            int a1RowIndex = FindIntervalIndex(areaA._rowIndexesTable, a1FirstRowIndex);
            int a2RowIndex = FindIntervalIndex(areaB._rowIndexesTable, a2FirstRowIndex);
            //int a1RowIndex = areaA._rowLocGlobIndexMap[a1FirstRowIndex];
            //int a2RowIndex = areaB._rowLocGlobIndexMap[a2FirstRowIndex];

            for (int absRowPos = intRows.Start, i = 0; absRowPos <= intRows.End; absRowPos++, i++)
            {
                if (areaA._rowsData[a1RowIndex].OffsY - a1FirstRowIndex == i && areaB._rowsData[a2RowIndex].OffsY - a2FirstRowIndex == i)
                {
                    correspondRowIndexes.Add(new Tuple<int, int, int>(a1RowIndex, a2RowIndex, i));
                }

                if (areaA._rowsData[a1RowIndex].OffsY - a1FirstRowIndex == i)
                {
                    a1RowIndex++;
                }
                if (areaB._rowsData[a2RowIndex].OffsY - a2FirstRowIndex == i)
                {
                    a2RowIndex++;
                }
            }

            int mostLeftPosition = int.MaxValue;
            var intervalRows = new List<List<Interval>>(correspondRowIndexes.Count);
            for (int rowIndex = 0; rowIndex < correspondRowIndexes.Count; rowIndex++)
            {
                var t = correspondRowIndexes[rowIndex];
                var rowA = areaA._rowsData[t.Item1];
                var rowB = areaB._rowsData[t.Item2];

                // Since all the intervals a positive and only Area offset can move
                // intervals to a negative dimensionm we pass _offsX to align the intervals
                // respectivelly to 0 position. So we will get intervals that start at negative zone
                // as well. This we will need to take into account when create a result Area.
                var intervalsA = rowA.GetIntervals(areaA._offsX);
                var intervalsB = rowB.GetIntervals(areaB._offsX);

                var intervalsC = new List<Interval>(100);
                for (int aIntervalIndex = 0; aIntervalIndex < intervalsA.Length; aIntervalIndex++)
                {
                    var aInterval = intervalsA[aIntervalIndex];
                    for (int bIntervalsIndex = 0; bIntervalsIndex < intervalsB.Length; bIntervalsIndex++)
                    {
                        var bInterval = intervalsB[bIntervalsIndex];
                        var cInterval = aInterval.Intersect(bInterval);
                        if (!cInterval.IsEmpty)
                        {
                            intervalsC.Add(cInterval);
                        }
                    }
                }

                for (int i = 0; i < intervalsC.Count; i++)
                {
                    var intervalC = intervalsC[i];
                    if (intervalC.Start < mostLeftPosition) mostLeftPosition = intervalC.Start;
                }
                intervalRows.Add(intervalsC);
            }

            int? ypos = null;

            for (int rowIndex = 0; rowIndex < correspondRowIndexes.Count; rowIndex++)
            {
                var t = correspondRowIndexes[rowIndex];
                var rowA = areaA._rowsData[t.Item1];
                var absRowIndex = rowA.OffsY + areaA.Bound.Y;
                var cIntervals = intervalRows[rowIndex];

                if (cIntervals.Count > 0)
                {
                    if (ypos == null)
                    {
                        ypos = absRowIndex;
                    }

                    var adjustedIntervals = new List<Interval>(cIntervals.Count);
                    for (int i = 0; i < cIntervals.Count; i++)
                    {
                        var interval = cIntervals[i];
                        adjustedIntervals.Add(new Interval()
                        {
                            Start = interval.Start - mostLeftPosition,
                            End = interval.End - mostLeftPosition
                        });
                    }

                    var rowData = new IntervalsData(absRowIndex - ypos.Value, adjustedIntervals.ToArray());
                    cIntervalsDatas.Add(rowData);
                }
            }

            Area2D result = Empty;
            if (cIntervalsDatas.Count > 0)
            {
                //correct position.
                var position = new Point(mostLeftPosition, ypos.Value);
                result = new Area2D(position, cIntervalsDatas.ToArray());
            }
            return result;
        }

        public Area2D Translate(int offsetX, int offsetY)
        {
            int left = int.MaxValue;
            for (int i = 0; i < _rowsData.Length; i++)
            {
                var row = _rowsData[i];
                var firstPos = row.TotalSpacesCountBeforeIntervals[0];
                if (left > firstPos) left = firstPos;
            }

            var position = new Point(Bound.X + offsetX - left, Bound.Y + offsetY);
            return new Area2D(position, this._rowsData);
        }

        //private static int BinSearch(int[] a, int key)
        //{
        //    int lo = 0, hi = a.Length - 1;
        //    while (lo <= hi)
        //    {
        //        int mid = lo + (hi - lo)/2;
        //        if (key < a[mid]) hi = mid - 1;
        //        else if (key > a[mid])lo = mid + 1;
        //        else return mid;
        //    }
        //    return -1;
        //}

        private static int FindIntervalIndex(int[] a, int key)
        {
            int lo = 0, hi = a.Length - 1;
            while (lo <= hi)
            {
                int mid = lo + (hi - lo) / 2;
                if (key == a[mid] ||
                    (a[mid] > key && ((mid > 0 && a[mid - 1] < key) || mid == 0))
                    )
                    return mid;
                else if (key < a[mid]) hi = mid - 1;
                else if (key > a[mid]) lo = mid + 1;
                //else return mid;
            }
            return a.Length - 1;
        }

        //private static int FindIntervalIndex(int[] intervals, int item)
        //{
        //    int i;
        //    for (i = 0; i < intervals.Length; i++)
        //    {
        //        if (intervals[i] >= item)
        //        {
        //            return i;
        //        }
        //    }
        //    return intervals.Length - 1;
        //}

        private struct IntervalsData
        {
            public IntervalsData(int offsy, IList<Interval> intervals)
            {
                OffsY = offsy;
                ElementsCount = 0;

                TotalSpacesCountBeforeIntervals = new int[intervals.Count];
                IntervalLastElementPositions = new int[intervals.Count];
                IntervalLastElementIndexes = new int[intervals.Count];

                Interval interval;
                for (var i = 0; i < intervals.Count; i++)
                {
                    interval = intervals[i];
#if DEBUG
                    if (interval.Start < 0 || interval.Start > interval.End)
                        throw new ArgumentException($"Not allowed interval: {interval}");
#endif

                    var spaces = interval.Start - ElementsCount;
                    ElementsCount += interval.End - interval.Start + 1;
                    TotalSpacesCountBeforeIntervals[i] = spaces;
                    IntervalLastElementPositions[i] = interval.End;
                    IntervalLastElementIndexes[i] = interval.End - spaces;
                }

                LastElementAbsoluteIndex = ElementsCount - 1;
            }

            public IntervalsData(IntervalState state)
            {
                ElementsCount = state.ElementsCount;
                IntervalLastElementIndexes = state.IntervalLastElementIndexes;
                IntervalLastElementPositions = state.IntervalLastElementPositions;
                LastElementAbsoluteIndex = state.LastElementAbsoluteIndex;
                OffsY = state.OffsY;
                TotalSpacesCountBeforeIntervals = state.TotalSpacesCountBeforeIntervals;
            }

            public IntervalState GetState()
            {
                return new IntervalState
                {
                    ElementsCount = ElementsCount,
                    IntervalLastElementIndexes = IntervalLastElementIndexes,
                    IntervalLastElementPositions = IntervalLastElementPositions,
                    LastElementAbsoluteIndex = LastElementAbsoluteIndex,
                    OffsY = OffsY,
                    TotalSpacesCountBeforeIntervals = TotalSpacesCountBeforeIntervals
                };
            }

            /// <summary>
            /// Position of the row in the bounds area.
            /// For the following matrix:
            /// 
            /// 0,0,0,0,0,0,0,0,0
            /// 0,1,1,0,1,1,0,0,1
            /// 1,1,1,1,0,0,1,1,1
            /// 0,1,1,0,0,0,1,1,0
            /// 0,0,0,0,0,0,0,0,0
            /// 0,0,0,0,1,1,1,0,0
            /// 0,0,0,0,0,0,0,0,0
            /// 
            /// For row #1 will be: 0
            /// For row #2 will be: 1
            /// For row #5 will be: 4
            /// </summary>
            public int OffsY;

            /// <summary>
            /// The last element absolute index
            /// </summary>
            public int LastElementAbsoluteIndex;

            /// <summary>
            /// The count of elements in the intervals.
            /// </summary>
            public int ElementsCount;

            /// <summary>
            /// Array of positions of the last elements in the intervals.
            /// For the following matrix:
            /// 
            /// 0,0,0,0,0,0,0,0,0
            /// 0,1,1,0,1,1,0,0,1
            /// 1,1,1,1,0,0,1,1,1
            /// 0,1,1,0,0,0,1,1,0
            /// 0,0,0,0,0,0,0,0,0
            /// 0,0,0,0,1,1,1,0,0
            /// 0,0,0,0,0,0,0,0,0
            /// 
            /// For row #0 will contain: 2, 5, 8
            /// For row #1 will contain: 3, 8
            /// For row #4 will contain: 6
            /// </summary>
            public int[] IntervalLastElementPositions;

            /// <summary>
            /// Array of the total spaces in the row before the interval.
            /// For the following matrix:
            /// 
            /// 0,0,0,0,0,0,0,0,0
            /// 0,1,1,0,1,1,0,0,1
            /// 1,1,1,1,0,0,1,1,1
            /// 0,1,1,0,0,0,1,1,0
            /// 0,0,0,0,0,0,0,0,0
            /// 0,0,0,0,1,1,1,0,0
            /// 0,0,0,0,0,0,0,0,0
            /// 
            /// For row #0 will contain: 1, 2, 4
            /// For row #1 will contain: 0, 2
            /// For row #4 will contain: 4
            /// </summary>
            public int[] TotalSpacesCountBeforeIntervals;

            /// <summary>
            /// ilei (Interval Last Element Indexes) indexes of last elements in the interval in the row space (starting from 0)
            /// It is difference between IntervalLastElementPositions - TotalSpacesCountBeforeIntervals
            /// 
            /// For the following matrix:
            /// 
            /// 0,0,0,0,0,0,0,0,0
            /// 0,1,1,0,1,1,0,0,1
            /// 1,1,1,1,0,0,1,1,1
            /// 0,1,1,0,0,0,1,1,0
            /// 0,0,0,0,0,0,0,0,0
            /// 0,0,0,0,1,1,1,0,0
            /// 0,0,0,0,0,0,0,0,0
            /// 
            /// For row #0 will contain: 1, 3, 4
            /// For row #1 will contain: 3, 6
            /// For row #4 will contain: 2
            /// </summary>
            public int[] IntervalLastElementIndexes;

            internal Interval[] GetIntervals(int offs)
            {
                Interval[] intervals = new Interval[IntervalLastElementPositions.Length];
                Interval interval = new Interval();
                for (int i = 0; i < IntervalLastElementPositions.Length; i++)
                {
                    interval.End = IntervalLastElementPositions[i] + offs;
                    interval.Start = TotalSpacesCountBeforeIntervals[i] + offs + (i <= 0 ? 0 : IntervalLastElementIndexes[i - 1] + 1);
                    intervals[i] = interval;
                }

                return intervals;
            }
        }
    }
}