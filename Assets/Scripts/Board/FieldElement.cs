using System.Collections.Generic;
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

        //Destiny: List of all neighbour buildings to the field (only for fields elements)
        private List<int> buildingsID;

        //Destiny: List of all neighbour paths to the field (only for fields elements)
        private List<int> pathsID;

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
            transform.GetComponent<NumberOverField.NumberOverField>().SetNumberValue(number);
        }

        //Destiny: Setting neighbours of path type
        public void SetPathsID(List<int> pathsID)
        {
            this.pathsID = pathsID;
        }

        //Destiny: Setting neighbours of buildings type
        public void SetBuildingsID(List<int> buildingsID)
        {
            this.buildingsID = buildingsID;
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