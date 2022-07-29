using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Game.Managers;
using Board;
using UnityEngine;
using static Board.JunctionElement;

namespace DataStorage
{
    //Destiny: Storage for all important information about the game
    public static class GameManager
    {
        public enum CatanMode
        {
            Basic,
            Advanced
        }

        public enum SwitchingMode
        {
            InitialSwitchingFirst,
            InitialSwitchingSecond,
            GameSwitching
        }

        public enum MovingMode
        {
            Normal,
            ThrowDice,
            OnePathForFree,
            TwoPathsForFree,
            MovingThief,
            BuildPath,
            BuildVillage,
            EndTurn
        }

        public enum BasicMovingMode
        {
            Normal,
            TradePhase,
            BuildPhase
        }
        
        //Destiny: Element selected by player right now
        public static SelectedElement Selected = new();

        //Destiny: Players data
        public static Player.Player[] Players;
        public static int CurrentPlayer;
        
        //Destiny: Current thrown dice number
        public static int CurrentDiceThrownNumber;
        
        //Destiny: Game and user modes
        public static CatanMode Mode;
        public static SwitchingMode SwitchingGameMode;
        public static MovingMode MovingUserMode;
        public static BasicMovingMode BasicMovingUserMode;

        //Destiny: End-of-game values
        public static bool EndGame;
        public const int PointsEndingGame = 10;

        //Destiny: Managers
        public static BuildManager BuildManager = new();
        public static CardsManager CardsManager = new();
        public static LongestPathManager LongestPathManager = new();
        public static PopupManager PopupManager = new();
        public static ResourceManager ResourceManager = new();
        public static TradeManager TradeManager = new();

        /// <summary>
        /// Switches current player to the next player
        /// </summary>
        public static void SwitchToNextPlayer()
        {
            CurrentPlayer = (CurrentPlayer + 1) % Players.Length;
        }

        /// <summary>
        /// Switches current player to the previous player
        /// </summary>
        public static void SwitchToPreviousPlayer()
        {
            if (CurrentPlayer == 0) 
                CurrentPlayer = Players.Length - 1;
            else 
                CurrentPlayer -= 1;
        }

        /// <summary>
        /// Switches player depdending on actual mode
        /// </summary>
        public static void SwitchPlayer()
        {
            //Destiny: If it's first turn of elements initial distribution in advanced mode and not last player
            //then switch to next player
            if (SwitchingGameMode == SwitchingMode.InitialSwitchingFirst && CurrentPlayer != Players.Length - 1)
            {
                SwitchToNextPlayer();
            }
            //Destiny: If it's first turn of elements initial distribution in advanced mode and last player
            //then switch to different mode
            else if (SwitchingGameMode == SwitchingMode.InitialSwitchingFirst && CurrentPlayer == Players.Length - 1)
            {
                SwitchingGameMode = SwitchingMode.InitialSwitchingSecond;
            }
            //Destiny: If it's second turn of elements initial distribution in advanced mode and not first player
            //then switch to previous player
            else if (SwitchingGameMode == SwitchingMode.InitialSwitchingSecond && CurrentPlayer != 0)
            {
                SwitchToPreviousPlayer();
            }
            //Destiny: If it's second turn of elements initial distribution in advanced mode and first player
            //then switch to different mode
            else if (SwitchingGameMode == SwitchingMode.InitialSwitchingSecond && CurrentPlayer == 0)
            {
                SwitchingGameMode = SwitchingMode.GameSwitching;
            }
            else
            {
                SwitchToNextPlayer();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="color">Player color</param>
        /// <returns>Player index</returns>
        /// <exception cref="Exception">Player with color given could not be found</exception>
        public static int GetPlayerIdByColor(Player.Player.Color color)
        {
            try
            {
                for (var i = 0; i < Players.Length; i++)
                {
                    if (Players[i].color == color)
                        return i;
                }
                throw new Exception();
            }
            catch
            {
                Debug.LogError("Player with color given could not be found.");
                return -1;
            }
        }

        /// <summary>
        /// Setting up game manager basic information based on popups inputs
        /// </summary>
        /// <param name="modeText">game mode choosed by players</param>
        /// <param name="playerNumber">game mode choosed by players</param>
        public static void Setup(string modeText)
        {
            Mode = modeText == "PODSTAWOWY" ? CatanMode.Basic : CatanMode.Advanced;
            SwitchingGameMode = Mode == CatanMode.Basic ? 
                SwitchingMode.GameSwitching : SwitchingMode.InitialSwitchingFirst;
            MovingUserMode = Mode == CatanMode.Basic ?
                MovingMode.ThrowDice : MovingMode.BuildVillage;

            //TODO: if trade is not possible turn into build phase
            BasicMovingUserMode = Mode == CatanMode.Basic ? 
                BasicMovingMode.TradePhase : BasicMovingMode.Normal;

            BuildManager.Setup();
            CardsManager.Setup();
            PopupManager.Setup();

            EndGame = false;
        }

        public static void SetProperPhase(BasicMovingMode phaseMode = BasicMovingMode.Normal)
        {
            BasicMovingUserMode = Mode == CatanMode.Basic ? phaseMode : BasicMovingMode.Normal;

            if (Mode == CatanMode.Basic && phaseMode == BasicMovingMode.TradePhase)
            {
                //TODO: if trade is not possible turn into build phase
                //if()
                //BasicMovingUserMode = BasicMovingMode.BuildPhase;
            }
        }

        /// <summary>
        /// Makes appropriate action suitable to thrown dices
        /// </summary>
        public static void HandleThrowingDices()
        {
            if (CurrentDiceThrownNumber != 7)
            {
                ResourceManager.UpdatePlayersResources();
            } 
            else
            {
                Players[CurrentPlayer].MoveThief(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns>List of the ids of players who are adjacent to given field</returns>
        public static List<int> AdjacentPlayerIdToField(int fieldId)
        {
            List<int> adjacentPlayerIds = new();

            //Destiny: For each junctions adjacent to chosen field
            BoardManager.Fields[fieldId].junctionsID.ForEach(delegate(int junctionId) {
                //Destiny: For each junction owned by any player
                if (BoardManager.Junctions[junctionId].type != JunctionType.None)
                {
                    int playerId = BoardManager.Junctions[junctionId].GetOwnerId();
                    if (playerId != CurrentPlayer && !adjacentPlayerIds.Contains(playerId))
                        adjacentPlayerIds.Add(playerId);
                }
            });
            adjacentPlayerIds.Sort();

            return adjacentPlayerIds;
        }
    }
}