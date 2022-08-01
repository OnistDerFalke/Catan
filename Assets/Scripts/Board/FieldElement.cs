using Assets.Scripts.Board.States;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Board.States.FieldState;
using static Player.Resources;

namespace Board
{
    public class FieldElement : BoardElement
    {
        //Destiny: List of all neighbour junctions to the field (only for fields elements)
        public List<int> junctionsID;

        //Destiny: List of all neighbour paths to the field (only for fields elements)
        public List<int> pathsID;

        //Destiny: Thief figure that shows over the field when there is a thief on it
        [Tooltip("Thief figure")] [SerializeField]
        public GameObject thiefFigure;

        //Destiny: The type of the field
        [Header("Type of the field")] [Space(5)]
        [Tooltip("Type of the field")] [SerializeField]
        public FieldType type;

        public FieldElement()
        {
            State = new FieldState();
        }

        public void SetState(FieldState state)
        {
            ((FieldState)State).id = state.id;
            ((FieldState)State).isThief = state.isThief;
            ((FieldState)State).number = state.number;
            ((FieldState)State).type = state.type;
            type = state.type;
        }

        /// <summary>
        /// Setting number over the field
        /// </summary>
        /// <param name="n">Number over the field to set</param>
        public void SetNumberAndApply(int n)
        {
            ((FieldState)State).number = n;
            if(!((FieldState)State).isThief)
            {
                transform.GetComponent<NumberOverField.NumberOverField>().SetNumberValue(((FieldState)State).number);
            }
            else
            {
                transform.GetComponent<NumberOverField.NumberOverField>().SetNumberValue(0);
            }
        }

        /// <summary>
        /// Setting neighbors of path type
        /// </summary>
        /// <param name="pathsID">List of neighbors of path type to set</param>
        public void SetPathsID(List<int> pathsID)
        {
            this.pathsID = pathsID;
        }

        /// <summary>
        /// Setting neighbors of junction type
        /// </summary>
        /// <param name="junctionsID">List of neighbors of junction type to set</param>
        public void SetJunctionsID(List<int> junctionsID)
        {
            this.junctionsID = junctionsID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Info about the type in FieldType format</returns>
        public FieldType GetTypeInfo()
        {
            return type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if the thief is over given field</returns>
        public bool IfThief()
        {
            return ((FieldState)State).isThief;
        }

        /// <summary>
        /// Sets new value of the variable that represents the presence of a thief
        /// </summary>
        /// <param name="isThief">new value of the presence of a thief</param>
        public void SetThief(bool isThief)
        {
            ((FieldState)State).isThief = isThief;
            SetNumberAndApply(((FieldState)State).number);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns>Type of resource from the given field</returns>
        public ResourceType GetResourceType()
        {
            switch (((FieldState)State).type)
            {
                case FieldType.Forest:
                    return ResourceType.Wood;
                case FieldType.Hills:
                    return ResourceType.Clay;
                case FieldType.Pasture:
                    return ResourceType.Wool;
                case FieldType.Mountains:
                    return ResourceType.Iron;
                case FieldType.Field:
                    return ResourceType.Wheat;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Number over the field</returns>
        public int GetNumber()
        {
            return ((FieldState)State).number;
        }

        void Awake()
        {
            boardElementType = BoardElementType.Field;
            ((FieldState)State).isThief = false;
        }

        void Update()
        {
            //Destiny: If thief is on the field, activate the figure
            thiefFigure.SetActive(((FieldState)State).isThief);
        }
    }
}