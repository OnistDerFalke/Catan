using UnityEngine;

namespace Board
{
    public class FieldElement : BoardElement
    {
        //Destiny: Types of available fields (only for fields elements)
        private enum FieldType
        {
            Forest,
            Pasture,
            Field,
            Hills,
            Mountains,
            Desert
        }

        //Destiny: Array of all neighbour buildings to the field (only for fields elements)
        private int[] buildingsID;
        
        //Destiny: Means that thief is on that field
        private bool isThief;
        
        //Destiny: The number above the field
        private int number;
        
        //Destiny: The type of the field
        [Header("Type of the field")][Space(5)]
        [Tooltip("Type of the field")] [SerializeField]
        private FieldType type;

        void Start()
        {
            isThief = false;
            number = 0;
            
            //Destiny: Updates the number
            transform.GetComponent<NumberOverField>().SetNumberValue(number);
        }
    }
}