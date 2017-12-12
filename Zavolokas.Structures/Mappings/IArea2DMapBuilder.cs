namespace Zavolokas.Structures
{
    public interface IArea2DMapBuilder
    {
        /// <summary>
        /// Adds the associated areas.
        /// </summary>
        /// <param name="areaOnDest">The area on dest.</param>
        /// <param name="areaOnSource">The area on source.</param>
        /// <remarks>
        /// - Can not be executed before InitNewMap method.
        /// - Both areaOnDest and areaOnSource can not be empty.
        /// </remarks>
        IArea2DMapBuilder AddAssociatedAreas(Area2D areaOnDest, Area2D areaOnSource);

        /// <summary>
        /// Builds the mapping.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// - Can not be executed before InitNewMap method.
        /// </remarks>
        Area2DMap Build();

        /// <summary>
        /// Initializes the new map.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <remarks>
        /// - Requires not null IPointToAreaMapping.
        /// - Requires that mapping has a number of dest points.
        /// - Requires that at least one dest point points to not empty source area.
        /// - Discards all the previous settings made by other methods.
        /// </remarks>
        IArea2DMapBuilder InitNewMap(IAreasMapping mapping);

        /// <summary>
        /// Initializes a new instance of the <see cref="Area2DMapBuilder" /> class.
        /// </summary>
        /// <param name="destArea">The dest area.</param>
        /// <param name="sourceArea">The source area.</param>
        /// <remarks>
        /// - Dest area can not be empty.
        /// - Source area can not be empty.
        /// - Discards all the previous settings made by other methods.
        /// </remarks>
        IArea2DMapBuilder InitNewMap(Area2D destArea, Area2D sourceArea);

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
        IArea2DMapBuilder ReduceDestArea(Area2D reduceArea, bool allowEmptyDest = false);

        /// <summary>
        /// Sets the ignored sourced area.
        /// </summary>
        /// <param name="ignoreArea">The ignore area.</param>
        /// <remarks>
        /// - Can not be executed before InitNewMap method.
        /// - Requires that the ignoreArea is not empty.
        /// - Requires that the ignoreArea not equal or include whole source area.
        /// </remarks>
        IArea2DMapBuilder SetIgnoredSourcedArea(Area2D ignoreArea);

        /// <summary>
        /// Adds all the associated areas from the mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <remarks>
        ///  - Can not be executed before InitNewMap method.
        /// </remarks>
        /// <returns></returns>
        IArea2DMapBuilder AddMapping(IAreasMapping mapping);
    }
}