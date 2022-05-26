using System;
using System.Collections.Generic;
using System.Linq;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    //Destiny: Class for kind of two dimensional array but with serialization
    [Serializable]
    public class TextArray
    {
        public Text[] Texts = new Text[6];
    }
    
    public class ScoreTableController : MonoBehaviour
    {
        [Header("Table")][Space(5)]
        [Tooltip("Score table")] [SerializeField]
        private GameObject scoreTable;
    
        [Header("Table Rows")][Space(5)]
        [Tooltip("Players rows")] [SerializeField]
        private GameObject[] playersRows = new GameObject[4];

        [Header("Table Texts")][Space(5)]
        [Tooltip("Players texts")] [SerializeField]
        private TextArray[] playersTexts = new TextArray[4];
        
        void Start()
        {
            //Destiny: Deactivating table on default and activating as many rows as needed
            scoreTable.SetActive(false);
            for(var i = 0; i<4; i++)
            {
                playersRows[i].SetActive(true);
                if(i == 3 && GameManager.PlayersNumber == 3) 
                    playersRows[i].SetActive(false);
            }
        }
        
        void Update()
        {
            UpdateScore();
            
            if (Input.GetKeyDown(KeyCode.Tab))
                scoreTable.SetActive(true);
            if (Input.GetKeyUp(KeyCode.Tab))
                scoreTable.SetActive(false);
        }

        /// <summary>
        /// Updates the texts in the table
        /// </summary>
        private void UpdateScore()
        {
            //Destiny: Creates list of scores
            var scoresList = new int[GameManager.PlayersNumber];
            for (var i = 0; i < GameManager.PlayersNumber; i++)
            {
                var player = GameManager.Players[i];
                var score = player.score;
                scoresList[i] = score.buildings + score.longestPath + score.knights + score.victoryPoints;
            }

            //Destiny: Getting the descending players score rank
            var sortedIndexArray = scoresList.Select((r, i) => new { Value = r, Index = i })
                .OrderByDescending(t => t.Value)
                .Select(p => p.Index)
                .ToArray();
            
            //Destiny: Setting information about scores in table
            for (var i = 0; i<GameManager.PlayersNumber; i++)
            {
                var player = GameManager.Players[i];
                var score = player.score;
                playersTexts[sortedIndexArray[i]].Texts[0].text = player.name;
                playersTexts[sortedIndexArray[i]].Texts[1].text = score.buildings.ToString();
                playersTexts[sortedIndexArray[i]].Texts[2].text = score.longestPath.ToString();
                playersTexts[sortedIndexArray[i]].Texts[3].text = score.knights.ToString();
                playersTexts[sortedIndexArray[i]].Texts[4].text = score.victoryPoints.ToString();
                playersTexts[sortedIndexArray[i]].Texts[5].text = (score.buildings + score.longestPath + 
                                                                   score.knights + score.victoryPoints).ToString();
            }
        }
    }
}
