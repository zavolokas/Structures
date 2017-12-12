using System;
using System.Collections.Generic;

namespace Zavolokas.Structures
{
    public class Area2DMapBuilder : IArea2DMapBuilder
    {
        private bool _isInitialized = false;
        private Area2D _sourceArea;
        private Area2D _destArea;
        private Area2D _ignoreArea;
        private readonly List<Tuple<Area2D, Area2D>> _associatedAreas = new List<Tuple<Area2D, Area2D>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Area2DMapBuilder"/> class.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"></exception>
        public Area2DMapBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Area2DMapBuilder" /> class.
        /// </summary>
        /// <param name="destArea">The dest area.</param>
        /// <param name="sourceArea">The source area.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        /// <exception cref="EmptyAreaException"></exception>
        /// <remarks>
        /// - Dest area can not be empty.
        /// - Source area can not be empty.
        /// - Discards all the previous settings made by other methods.
        /// </remarks>
        public IArea2DMapBuilder InitNewMap(Area2D destArea, Area2D sourceArea)
        {
            if (destArea == null)
                throw new ArgumentNullException(nameof(destArea));

            if (sourceArea == null)
                throw new ArgumentNullException(nameof(sourceArea));

            if (destArea.IsEmpty || sourceArea.IsEmpty)
                throw new EmptyAreaException();

            _destArea = destArea;
            _sourceArea = sourceArea;
            _associatedAreas.Clear();

            _isInitialized = true;
            return this;
        }

        /// <summary>
        /// Initializes the new map.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <remarks>
        /// - Requires not null IPointToAreaMapping.
        /// - Requires that mapping has a number of dest points.
        /// - Requires that at least one dest point points to not empty source area.
        /// - Discards all the previous settings made by other methods.
        /// </remarks>
        public IArea2DMapBuilder InitNewMap(IAreasMapping mapping)
        {
            if (mapping == null)
                throw new ArgumentNullException(nameof(mapping));

            var areas = mapping.AssociatedAreasAsc;
            var destArea = Area2D.Empty;
            var sourceArea = Area2D.Empty;

            foreach (var area in areas)
            {
                destArea = destArea.Join(area.Item1);
                sourceArea = sourceArea.Join(area.Item2);
            }

            _destArea = destArea;
            _sourceArea = sourceArea;

            _associatedAreas.Clear();
            _associatedAreas.AddRange(areas);

            _isInitialized = true;

            return this;
        }

        /// <summary>
        /// Sets the ignored sourced area.
        /// </summary>
        /// <param name="ignoreArea">The ignore area.</param>
        /// <exception cref="MapIsNotInitializedException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="EmptyAreaException"></exception>
        /// <exception cref="AreaRemovedException"></exception>
        /// <remarks>
        /// - Can not be executed before InitNewMap method.
        /// - Requires that the ignoreArea is not empty.
        /// - Requires that the ignoreArea not equal or include whole source area.
        /// </remarks>
        public IArea2DMapBuilder SetIgnoredSourcedArea(Area2D ignoreArea)
        {
            if (!_isInitialized)
                throw new MapIsNotInitializedException();

            if (ignoreArea == null)
                throw new ArgumentNullException(nameof(ignoreArea));

            if (ignoreArea.IsEmpty)
                throw new EmptyAreaException();

            var substractedArea = _sourceArea.Substract(ignoreArea);
            if (substractedArea.IsEmpty)
                throw new AreaRemovedException();

            _ignoreArea = ignoreArea;

            return this;
        }

        /// <summary>
        /// Adds all the associated areas from the mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <remarks>
        /// - Can not be executed before InitNewMap method.
        /// </remarks>
        public IArea2DMapBuilder AddMapping(IAreasMapping mapping)
        {
            if (!_isInitialized)
                throw new MapIsNotInitializedException();

            if (mapping == null)
                throw new ArgumentNullException(nameof(mapping));

            var areas = mapping.AssociatedAreasAsc;
            var destArea = _destArea;
            var sourceArea = _sourceArea;

            foreach (var area in areas)
            {
                destArea = destArea.Join(area.Item1);
                sourceArea = sourceArea.Join(area.Item2);
            }

            _destArea = destArea;
            _sourceArea = sourceArea;

            _associatedAreas.AddRange(areas);

            return this;
        }

        /// <summary>
        /// Reduces the current dest area of the mapping. 
        /// It may allow empty dest area in the result mapping what makes it important to check the built mapping afterwards whether the dest area is not empty.
        /// </summary>
        /// <param name="reduceArea">Intersection of this area with the current destination area will be the new destination area.</param>
        /// <param name="allowEmptyDest">By deafult method doesn't allow to pass reduceArea that will result in empty destination area of the result mapping. 
        /// But it can be swithed off implicitly. In that case the client of the class is responsible for handaling of this special case.</param>
        /// <exception cref="MapIsNotInitializedException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="EmptyAreaException"></exception>
        /// <exception cref="AreaRemovedException"></exception>
        /// <remarks>
        /// - Can not be executed before InitNewMap method.
        /// - Requires that the reduceArea is not empty.
        /// - Requires that reduce area in intersection with dest area not results in empty area.
        /// - Multiple call of the method has a cumulutive effect - result dest area should be an intersection of all passed areas and the dest
        /// </remarks>
        public IArea2DMapBuilder ReduceDestArea(Area2D reduceArea, bool allowEmptyDest = false)
        {
            if (!_isInitialized)
                throw new MapIsNotInitializedException();

            if (reduceArea == null)
                throw new ArgumentNullException(nameof(reduceArea));

            if (reduceArea.IsEmpty)
                throw new EmptyAreaException();

            var newDestArea = _destArea.Intersect(reduceArea);
            if (newDestArea.IsEmpty && !allowEmptyDest)
                throw new AreaRemovedException();

            _destArea = newDestArea;

            return this;
        }

        /// <summary>
        /// Adds the associated areas.
        /// </summary>
        /// <param name="areaOnDest">The area on dest.</param>
        /// <param name="areaOnSource">The area on source.</param>
        /// <exception cref="MapIsNotInitializedException"></exception>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        /// <exception cref="EmptyAreaException"></exception>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <remarks>
        /// - Can not be executed before InitNewMap method.
        /// - Both areaOnDest and areaOnSource can not be empty.
        /// </remarks>
        public IArea2DMapBuilder AddAssociatedAreas(Area2D areaOnDest, Area2D areaOnSource)
        {
            if (!_isInitialized)
                throw new MapIsNotInitializedException();

            if (areaOnDest == null)
                throw new ArgumentNullException(nameof(areaOnDest));

            if (areaOnSource == null)
                throw new ArgumentNullException(nameof(areaOnSource));

            if (areaOnDest.IsEmpty || areaOnSource.IsEmpty)
                throw new EmptyAreaException();

            _associatedAreas.Add(new Tuple<Area2D, Area2D>(areaOnDest, areaOnSource));

            return this;
        }

        /// <summary>
        /// Builds the mapping.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MapIsNotInitializedException"></exception>
        /// <remarks>
        /// - Can not be executed before InitNewMap method.
        /// </remarks>
        public Area2DMap Build()
        {
            if (!_isInitialized)
                throw new MapIsNotInitializedException();

            List<Tuple<Area2D, Area2D>> areas = new List<Tuple<Area2D, Area2D>>();

            foreach (var areaPair in _associatedAreas)
            {
                var destArea = areaPair.Item1.Intersect(_destArea);
                if (!destArea.IsEmpty)
                {
                    var sourceArea = areaPair.Item2;
                    if (_ignoreArea != null && !_ignoreArea.IsEmpty)
                    {
                        sourceArea = sourceArea.Substract(_ignoreArea);
                    }
                    if (!sourceArea.IsEmpty)
                    {
                        areas.Add(new Tuple<Area2D, Area2D>(destArea, sourceArea));
                    }
                }
            }

            var srcArea = _sourceArea;

            if (_ignoreArea != null && !_ignoreArea.IsEmpty)
            {
                srcArea = srcArea.Substract(_ignoreArea);
            }
            areas.Insert(0, new Tuple<Area2D, Area2D>(_destArea, srcArea));

            var result = new Area2DMap(areas.ToArray(), Area2D.Empty);
            return result;
        }
    }
}
