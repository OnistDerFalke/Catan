using System;
using System.Collections.Generic;
using System.Linq;
using DataStorage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class MiniScoreTableController : MonoBehaviour
    {
        [Header("Slots")][Space(5)]
        [Tooltip("Slots Images")] [SerializeField] private Image[] slotsImages;
        [Tooltip("Slots Values")] [SerializeField] private Text[] slotsValues;
        
        [Header("Slots Dimensions")][Space(5)]
        [Tooltip("Slots Frame")] [SerializeField] private Image slotsFrame;
        [Tooltip("Slots Gap")] [SerializeField] private float slotsGap;
        [Tooltip("Slots Frame Border Gap")] [SerializeField] private float borderGap;
        
        // Start is called before the first frame update
        void Start()
        {
            CreateSlotsView();
            SetSlotsColors();
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePlayersScores();
        }

        private void CreateSlotsView()
        {
            var frameWidth = slotsFrame.rectTransform.rect.width - borderGap;
            var slotWidth = frameWidth / GameManager.PlayersNumber - slotsGap;
            var placementOffset = frameWidth / GameManager.PlayersNumber;
            var placementStart = GameManager.PlayersNumber % 2 == 1
                ? -Mathf.Floor(GameManager.PlayersNumber / 2f) * placementOffset
                : (-(GameManager.PlayersNumber / 2f)+0.5f) * placementOffset;
            
            Debug.Log($"frameWidth: {frameWidth}, slotWidth: {slotWidth}, placementOffset: {placementOffset}, placementStart: {placementStart}");

            for (var i = 0; i < GameManager.PlayersNumber; i++)
            {
                var rectTransformRect = new Vector2(slotWidth, slotsImages[i].rectTransform.rect.height);
                slotsImages[i].rectTransform.sizeDelta = rectTransformRect;

                var pos = slotsImages[i].gameObject.transform.localPosition;
                pos.x = placementStart + i * placementOffset;
                slotsImages[i].gameObject.transform.localPosition = pos;
                
                slotsImages[i].gameObject.SetActive(true);
            }
        }

        private void SetSlotsColors()
        {
            for (var i = 0; i < GameManager.PlayersNumber; i++)
            {
                slotsImages[i].color = GameManager.Players[i].color switch
                {
                    Player.Player.Color.Blue => Color.blue,
                    Player.Player.Color.Red => Color.red,
                    Player.Player.Color.White => Color.white,
                    Player.Player.Color.Yellow => Color.yellow,
                    _ => slotsImages[i].color
                };
            }
        }

        private void UpdatePlayersScores()
        {
            for (var i = 0; i < GameManager.PlayersNumber; i++)
            {
              
                var score = GameManager.Players[i].score;
                var scoreSum = score.GetPoints(Player.Score.PointType.Buildings) + score.GetPoints(Player.Score.PointType.LongestPath) +
                                score.GetPoints(Player.Score.PointType.Knights) + score.GetPoints(Player.Score.PointType.VictoryPoints);
                slotsValues[i].text = scoreSum.ToString();
            }
        }
    }
}
