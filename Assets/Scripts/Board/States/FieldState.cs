using System;

namespace Assets.Scripts.Board.States
{
    [Serializable]
    public class FieldState : ElementState
    {
        //Destiny: Types of available fields (only for fields elements)
        public enum FieldType
        {
            Forest,
            Pasture,
            Field,
            Hills,
            Mountains,
            Desert
        }

        //Destiny: Means that thief is on that field
        public bool isThief;

        //Destiny: The number above the field
        public int number;

        //Destiny: The type of the field
        public FieldType type;
    }
}
