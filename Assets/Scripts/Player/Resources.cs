using Board;

namespace Player
{
    public class Resources
    {
        //Destiny: Types of resources
        public enum ResourceType
        {
            Wood,
            Clay,
            Wool,
            Iron,
            Wheat
        }

        public int woodNumber;
        public int clayNumber;
        public int woolNumber;
        public int ironNumber;
        public int wheatNumber;

        public Resources()
        {
            woodNumber = 0;
            clayNumber = 0;
            woolNumber = 0;
            ironNumber = 0;
            wheatNumber = 0;
        }

        public void AddResources(FieldElement.FieldType fieldType, int number = 1)
        {
            switch(fieldType)
            {
                case FieldElement.FieldType.Forest:
                    woodNumber += number;
                    break;
                case FieldElement.FieldType.Hills:
                    clayNumber += number;
                    break;
                case FieldElement.FieldType.Pasture:
                    woolNumber += number;
                    break;
                case FieldElement.FieldType.Mountains:
                    ironNumber += number;
                    break;
                case FieldElement.FieldType.Field:
                    wheatNumber += number;
                    break;
                default:
                    break;
            }
        }

        public void RemoveResources(ResourceType fieldType, int number)
        {
            switch (fieldType)
            {
                case ResourceType.Wood:
                    woodNumber -= number;
                    break;
                case ResourceType.Clay:
                    clayNumber -= number;
                    break;
                case ResourceType.Wool:
                    woolNumber -= number;
                    break;
                case ResourceType.Iron:
                    ironNumber -= number;
                    break;
                case ResourceType.Wheat:
                    wheatNumber -= number;
                    break;
                default:
                    break;
            }
        }
    }
}