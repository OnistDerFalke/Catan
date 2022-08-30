using DataStorage;
using System;
using System.Linq;
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

        [Header("Table Backgrounds")] [Space(5)] 
        [Tooltip("Three Players Background")] [SerializeField] private GameObject background3P;
        [Tooltip("Four Players Background")] [SerializeField] private GameObject background4P;

        [Header("Table Texts")][Space(5)]
        [Tooltip("Players texts")] [SerializeField]
        private TextArray[] playersTexts = new TextArray[4];
        
        [Header("Table Colors")][Space(5)]
        [Tooltip("Players Colors")] [SerializeField] private Image[] playersColors = new Image[4];
        [Tooltip("White Player Color")] [SerializeField] private Color whitePlayerColor;
        [Tooltip("Red Player Color")] [SerializeField] private Color redPlayerColor;
        [Tooltip("Blue Player Color")] [SerializeField] private Color bluePlayerColor;
        [Tooltip("Yellow Player Color")] [SerializeField] private Color yellowPlayerColor;

        void Start()
        {
            //Destiny: Activating background for right number of players (rows number)
            switch (GameManager.State.Players.Length)
            {
                
                case 3:
                    background3P.SetActive(true);
                    break;
                case 4:
                    background4P.SetActive(true);
                    break;
            }
            
            scoreTable.SetActive(false);
            
            //Destiny: Deactivating table on default and activating as many rows as needed
            for(var i = 0; i < 4; i++)
            {
                playersRows[i].SetActive(true);
                if(i == 3 && GameManager.State.Players.Length == 3) 
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
        /// Updates the texts and colors in the table
        /// </summary>
        private void UpdateScore()
        {
            //Destiny: Creates list of scores
            var scoresList = new int[GameManager.State.Players.Length];
            for (var i = 0; i < GameManager.State.Players.Length; i++)
            {
                var player = GameManager.State.Players[i];
                var score = player.score;
                scoresList[i] = 
                    score.GetPoints(Player.Score.PointType.Buildings) + 
                    score.GetPoints(Player.Score.PointType.LongestPath) +
                    score.GetPoints(Player.Score.PointType.Knights) + 
                    score.GetPoints(Player.Score.PointType.VictoryPoints);
            }

            //Destiny: Getting the descending players score rank
            var sortedIndexArray = scoresList.Select((r, i) => new { Value = r, Index = i })
                .OrderByDescending(t => t.Value)
                .Select(p => p.Index)
                .ToArray();

            //Destiny: Setting information about scores in table
            for (var i = 0; i < GameManager.State.Players.Length; i++)
            {
                var player = GameManager.State.Players[sortedIndexArray[i]];
                var score = player.score;
                playersTexts[i].Texts[0].text = player.name;
                playersTexts[i].Texts[1].text = score.GetPoints(Player.Score.PointType.Buildings).ToString();
                playersTexts[i].Texts[2].text = score.GetPoints(Player.Score.PointType.LongestPath).ToString();
                playersTexts[i].Texts[3].text = score.GetPoints(Player.Score.PointType.Knights).ToString();
                playersTexts[i].Texts[4].text = score.GetPoints(Player.Score.PointType.VictoryPoints).ToString();
                playersTexts[i].Texts[5].text = (score.GetPoints(Player.Score.PointType.Buildings) + 
                                                 score.GetPoints(Player.Score.PointType.LongestPath) + 
                                                 score.GetPoints(Player.Score.PointType.Knights) + 
                                                 score.GetPoints(Player.Score.PointType.VictoryPoints)).ToString();
                
                foreach (var text in playersTexts[i].Texts)
                {
                    text.color = player.color switch
                    {
                        Player.Player.Color.Blue => Color.white,
                        Player.Player.Color.Red => Color.white,
                        Player.Player.Color.Yellow => Color.black,
                        Player.Player.Color.White => Color.black,
                        _ => playersColors[GameManager.State.CurrentPlayerId].color
                    };
                }
                
                playersColors[i].color = player.color switch
                {
                    Player.Player.Color.Blue => bluePlayerColor,
                    Player.Player.Color.Red => redPlayerColor,
                    Player.Player.Color.Yellow => yellowPlayerColor,
                    Player.Player.Color.White => whitePlayerColor,
                    _ => playersColors[GameManager.State.CurrentPlayerId].color
                };
            }
        }
    }
}
