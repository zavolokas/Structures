using System;
using System.Collections.Generic;
using System.Linq;

namespace Zavolokas.Structures
{
    public class Area2DMap : IAreasMapping
    {
        private readonly Area2D _destArea;
        private readonly Area2D _sourceArea;
        private readonly Tuple<Area2D, Area2D>[] _areas;
        private readonly Area2D _outsideArea;

        /// <summary>
        /// Initializes a new instance of the <see cref="Area2DMap" /> class.
        /// </summary>
        /// <param name="areas">The array of area tuples which are Dest Area and Source Area.</param>
        /// <param name="outsideArea">The area to map points that area not mapped.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        /// <exception cref="EmptyAreaException"></exception>
        /// <remarks>
        /// - Dest areas and Source areas can not be nither null nor empty.
        /// - In a case when dest areas have common point, the source area of the last dest area is associated with the points.
        /// - Outside area can not be null.
        /// </remarks>
        // NOTE: It is IMPORTANT that the constructor of this class is INTERNAL. This constructor is not allowed to 
        // be called by a client code. Someone should control the allowence of empty areas in the mapping. Now  the 
        // builder class requires from the client to allow it implicitly.
        internal Area2DMap(Tuple<Area2D, Area2D>[] areas, Area2D outsideArea)
        {
            if (areas == null)
                throw new ArgumentNullException(nameof(areas));

            if (outsideArea == null)
                throw new ArgumentNullException(nameof(outsideArea));

            if (areas.Any(a => a.Item2 == null || a.Item1 == null))
                throw new ArgumentNullException();

            //NOTE: The requirement below is no longer valid since the dest can be allowed to get reduced to
            // an empty area by the client of the builder class implicitly and than the client is responsible 
            // for handling of this case.
            //if (areas.Any(a => a.Item2.IsEmpty || a.Item1.IsEmpty))
            //    throw new EmptyAreaException();

            _areas = areas;
            _outsideArea = outsideArea;
            _destArea = areas.Aggregate(Area2D.Empty, (current, t) => current.Join(t.Item1));
            _sourceArea = areas.Aggregate(Area2D.Empty, (current, t) => current.Join(t.Item2));
        }

        /// <summary>
        /// Gets the source area for the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>Associated source Area2D. Or an EmptyArea if there is no mapping for the point.</returns>
        /// <remarks>
        /// </remarks>
        public Area2D GetPointSourceArea(Point point)
        {
            Area2D result = _outsideArea;
            if (_areas.Any(t => t.Item1.GetPointIndex(point) > -1))
            {
                result = _areas.Last(t => t.Item1.GetPointIndex(point) > -1).Item2;
            }
            return result;
        }

        /// <summary>
        /// Gets the associated areas.
        /// </summary>
        /// <value>
        /// The associated areas.
        /// </value>
        Tuple<Area2D, Area2D>[] IAreasMapping.AssociatedAreasAsc
        {
            get
            {
                var areas = new Tuple<Area2D, Area2D>[_areas.Length];
                _areas.CopyTo(areas,0);
                return areas;
            }
        }

        /// <summary>
        /// Gets the source area that includes all the source areas.
        /// </summary>
        /// <value>
        /// The source area.
        /// </value>
        Area2D IAreasMapping.SourceArea
        {
            get { return _sourceArea; }
        }

        /// <summary>
        /// Gets the dest area that includes all the dest areas.
        /// </summary>
        /// <value>
        /// The dest area.
        /// </value>
        Area2D IAreasMapping.DestArea
        {
            get { return _destArea; }
        }

        /// <summary>
        /// Gets the coun of all the unique points in all the dest areas.
        /// </summary>
        /// <value>
        /// The dest elements count.
        /// </value>
        public int DestElementsCount{ get { return _destArea.ElementsCount; } }

        /// <summary>
        /// Gets the dest points.
        /// </summary>
        /// <value>
        /// The dest points.
        /// </value>
        /// <remarks>
        /// Contains all the points from the destination areas without duplications.
        /// Points are ordered from the left to the right and from the top to the bottom.
        /// </remarks>
        public IEnumerable<Point> DestPoints {get { return _destArea.Points; }}

        /// <summary>
        /// Gets the bounds that contain all the dest areas.
        /// </summary>
        /// <value>
        /// The dest bounds.
        /// </value>
        public Rectangle DestBounds { get { return _destArea.Bound; } }

        /// <summary>
        /// Gets the <see cref="Point"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="Point"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        /// <exception cref="System.IndexOutOfRangeException"></exception>
        public Point this[int index]
        {
            get
            {
                if (index < 0 || DestElementsCount <= index)
                    throw new IndexOutOfRangeException();

                return _destArea[index];
            }
        }
    }
}
