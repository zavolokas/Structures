namespace Zavolokas.Structures
{
    public class Area2DMapState
    {
        public Area2DState DestAreaState { get; set; }
        public Area2DState SourceAreaState { get; set; }
        public Area2DState OutsideAreaState { get; set; }
        public Area2DState[][] Areas { get; set; }
    }
}