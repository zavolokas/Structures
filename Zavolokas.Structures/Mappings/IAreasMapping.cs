using System;

namespace Zavolokas.Structures
{
    public interface IAreasMapping
    {
        /// <summary>
        /// Gets the associated areas in the ascesnding order by the visibility.
        /// </summary>
        /// <value>
        /// The associated areas.
        /// </value>
        Tuple<Area2D, Area2D>[] AssociatedAreasAsc { get; }

        /// <summary>
        /// Gets the source area that includes all the source areas.
        /// </summary>
        /// <value>
        /// The source area.
        /// </value>
        Area2D SourceArea { get; }

        /// <summary>
        /// Gets the dest area that includes all the dest areas.
        /// </summary>
        /// <value>
        /// The dest area.
        /// </value>
        Area2D DestArea { get; }
    }
}