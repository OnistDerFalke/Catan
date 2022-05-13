using UnityEngine;

namespace Board
{
    public class FieldElement : BoardElement
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

        //Destiny: Setting number over the field
        public void SetNumberAndApply(int n)
        {
            number = n;
            transform.GetComponent<NumberOverField>().SetNumberValue(number);
        }

        //Destiny: Returns info about the type in FieldType format
        public FieldType GetTypeInfo()
        {
            return type;
        }
        void Start()
        {
            isThief = false;
        }
    }
}