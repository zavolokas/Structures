using System;

namespace Zavolokas.Structures
{
    public class AreaRemovedException: Exception
    {    }

    public class MapIsNotInitializedException : Exception
    {
        public override string Message
        {
            get
            {
                Area2DMapBuilder builder;
                return $"Method can not be executed before calling '{nameof(builder.InitNewMap)}' method.";
            }
        }
    }

    public class EmptyAreaException : Exception
    {
    }

    public class NoSourceAreaException : Exception
    { }

    public class MappingHasNoDestPointsException : Exception
    {
    }
}
