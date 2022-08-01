using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    //Destiny: Keeps information about player's score
    public class Score
    {
        //Destiny: Types of points
        public enum PointType
        {
            Buildings,
            Knights,
            LongestPath,
            VictoryPoints,
            None
        }

        [SerializeField] private int buildings;
        [SerializeField] private int knights;
        [SerializeField] private int longestPath;
        [SerializeField] private int victoryPoints;

        public Score()
        {
            buildings = 0;
            knights = 0;
            longestPath = 0;
            victoryPoints = 0;
        }

        public int GetPoints(PointType type)
        {
            switch (type)
            {
                case PointType.Buildings:
                    return buildings;
                case PointType.Knights:
                    return knights;
                case PointType.LongestPath:
                    return longestPath;
                case PointType.VictoryPoints:
                    return victoryPoints;
            }

            return buildings + knights + longestPath + victoryPoints;
        }

        public void AddPoints(PointType type)
        {
            switch(type)
            {
                case PointType.Buildings:
                    buildings++;
                    break;
                case PointType.Knights:
                    knights = 2;
                    break;
                case PointType.LongestPath:
                    longestPath = 2;
                    break;
                case PointType.VictoryPoints:
                    victoryPoints++;
                    break;
            }
        }

        public void RemovePoints(PointType type)
        {
            switch (type)
            {
                case PointType.Knights:
                    knights = 0;
                    break;
                case PointType.LongestPath:
                    longestPath = 0;
                    break;
            }
        }
    }
}