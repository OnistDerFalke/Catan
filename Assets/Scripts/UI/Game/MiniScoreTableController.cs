using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static DataStorage.GameManager;

namespace UI.Game
{
    public class MiniScoreTableController : MonoBehaviour
    {
        [Header("Slots")][Space(5)]
        [Tooltip("Slots Images")] [SerializeField] private Image[] slotsImages;
        [Tooltip("Slots Values")] [SerializeField] private Text[] slotsValues;
        
        //Destiny: Frame is used to take width, slot gap is space between slots, border gap is
        //additional for frame visibility
        [Header("Slots Dimensions")][Space(5)]
        [Tooltip("Slots Frame")] [SerializeField] private Image slotsFrame;
        [Tooltip("Slots Gap")] [SerializeField] private float slotsGap;
        [Tooltip("Slots Frame Border Gap")] [SerializeField] private float borderGap;
        
        void Start()
        {
            //Destiny: Placing tiles in the frame and setting it's colors
            CreateSlotsView();
            SetSlotsColors();
        }
        
        void Update()
        {
            //Destiny: Updating scores on tiles
            UpdatePlayersScores();
        }

        /// <summary>
        /// Places tiles in the frame on positions based on player numbers and frame width
        /// </summary>
        private void CreateSlotsView()
        {
            var frameWidth = slotsFrame.rectTransform.rect.width - borderGap;
            var slotWidth = frameWidth / Players.Length - slotsGap;
            var placementOffset = frameWidth / Players.Length;
            var placementStart = Players.Length % 2 == 1 ?
                -Mathf.Floor(Players.Length / 2f) * placementOffset :
                ( -Players.Length / 2f + 0.5f) * placementOffset;
            
            for (var i = 0; i < Players.Length; i++)
            {
                //Destiny: Setting tiles width
                var rectTransformRect = new Vector2(slotWidth, slotsImages[i].rectTransform.rect.height);
                slotsImages[i].rectTransform.sizeDelta = rectTransformRect;

                //Destiny: Setting positions of the tiles
                var pos = slotsImages[i].gameObject.transform.localPosition;
                pos.x = placementStart + i * placementOffset;
                slotsImages[i].gameObject.transform.localPosition = pos;
                
                //Destiny: Showing the tiles
                slotsImages[i].gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Sets colors of the tiles
        /// </summary>
        private void SetSlotsColors()
        {
            for (var i = 0; i < Players.Length; i++)
            {
                slotsImages[i].color = Players[i].color switch
                {
                    Player.Player.Color.Blue => Color.blue,
                    Player.Player.Color.Red => Color.red,
                    Player.Player.Color.White => Color.white,
                    Player.Player.Color.Yellow => Color.yellow,
                    _ => slotsImages[i].color
                };
            }
        }

        /// <summary>
        /// Updates players' scores on tiles
        /// </summary>
        private void UpdatePlayersScores()
        {
            for (var i = 0; i < Players.Length; i++)
            {
              
                var score = Players[i].score;
                var scoreSum = 
                    score.GetPoints(Player.Score.PointType.Buildings) + 
                    score.GetPoints(Player.Score.PointType.LongestPath) +
                    score.GetPoints(Player.Score.PointType.Knights) + 
                    score.GetPoints(Player.Score.PointType.VictoryPoints);
                slotsValues[i].text = scoreSum.ToString();
            }
        }
    }
}
