using System.Linq;
using DataStorage;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Game.Popups
{
    public class EndGamePopupController : MonoBehaviour
    {
        [Header("Table Texts")][Space(5)]
        [Tooltip("Players texts")] [SerializeField] private TextArray[] playersTexts = new TextArray[4];
        
        [Header("Rank Elements")][Space(5)]
        [Tooltip("Rank Images")] [SerializeField] private Image[] rankImages;
        [Tooltip("Rank Names")] [SerializeField] private Text[] rankNames;
        
        [Header("Tab End Game UI")][Space(5)]
        [Tooltip("Summary Button")] [SerializeField] private Button summaryButton;
        [Tooltip("Rank Button")] [SerializeField] private Button rankButton;
        [Tooltip("Summary Tab")] [SerializeField] private GameObject summaryTab;
        [Tooltip("Rank Tab")] [SerializeField] private GameObject rankTab;
        [Tooltip("Summary Button Background")] [SerializeField] private GameObject summaryButtonBackground;
        [Tooltip("Rank Button Background")] [SerializeField] private GameObject rankButtonBackground;
        

        [Header("Flow Control Buttons")][Space(5)]
        [Tooltip("Main Menu Exit Button")] [SerializeField] private Button mainMenuExitButton;
        [Tooltip("Game Exit Button")] [SerializeField] private Button gameExitButton;

        private void Start()
        {
            summaryButton.onClick.AddListener(OnSummaryButton);
            rankButton.onClick.AddListener(OnRankButton);
            mainMenuExitButton.onClick.AddListener(OnMainMenuExitButton);
            gameExitButton.onClick.AddListener(OnGameExitButton);
        }

        private void OnSummaryButton()
        {
            rankButtonBackground.SetActive(false);
            rankTab.SetActive(false);
            
            summaryButtonBackground.SetActive(true);
            summaryTab.SetActive(true);
        }

        private void OnRankButton()
        {
            summaryButtonBackground.SetActive(false);
            summaryTab.SetActive(false);
            
            rankButtonBackground.SetActive(true);
            rankTab.SetActive(true);
        }

        private void OnMainMenuExitButton()
        {
            SceneManager.LoadScene("Scenes/MainMenu", LoadSceneMode.Single);
        }

        private void OnGameExitButton()
        {
            Application.Quit();
        }
        
        private void OnEnable()
        {
            UpdateScoreAndRank();
        }

        /// <summary>
        /// Updates the texts in the summary table and rank images
        /// </summary>
        private void UpdateScoreAndRank()
        {
            //Destiny: Creates list of scores
            var scoresList = new int[GameManager.State.Players.Length];
            for (var i = 0; i < GameManager.State.Players.Length; i++)
            {
                var player = GameManager.State.Players[i];
                var score = player.score;
                scoresList[i] = score.GetPoints(Player.Score.PointType.Buildings) +
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

                if (i != 3)
                {
                    rankImages[i].color = GameManager.State.Players[player.index].color switch
                    {
                        Player.Player.Color.Blue => Color.blue,
                        Player.Player.Color.Red => Color.red,
                        Player.Player.Color.Yellow => Color.yellow,
                        Player.Player.Color.White => Color.white,
                        _ => rankImages[i].color
                    };
                    rankNames[i].text = GameManager.State.Players[player.index].name;
                }

                playersTexts[i].Texts[0].text = player.name;
                playersTexts[i].Texts[1].text = score.GetPoints(Player.Score.PointType.Buildings).ToString();
                playersTexts[i].Texts[2].text = score.GetPoints(Player.Score.PointType.LongestPath).ToString();
                playersTexts[i].Texts[3].text = score.GetPoints(Player.Score.PointType.Knights).ToString();
                playersTexts[i].Texts[4].text = score.GetPoints(Player.Score.PointType.VictoryPoints).ToString();
                playersTexts[i].Texts[5].text = (score.GetPoints(Player.Score.PointType.Buildings) +
                                                 score.GetPoints(Player.Score.PointType.LongestPath) +
                                                 score.GetPoints(Player.Score.PointType.Knights) +
                                                 score.GetPoints(Player.Score.PointType.VictoryPoints)).ToString();
            }
        }
    }
}
