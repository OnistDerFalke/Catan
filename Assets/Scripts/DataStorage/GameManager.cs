using System;
using System.Collections.Generic;
using Assets.Scripts.Board.States;
using Assets.Scripts.DataStorage.Managers;
using Board;
using Board.States;
using DataStorage.Managers;
using UnityEngine;
using static Assets.Scripts.Board.States.JunctionState;
using static Board.States.GameState;

namespace DataStorage
{
    [Serializable]
    //Destiny: Storage for all important information about the game
    public static class GameManager
    {        
        //Destiny: Element selected by player right now
        public static SelectedElement Selected = new();
        
        //Destiny: Game data
        public static GameState State = new();

        //Destiny: End-of-game values
        public static bool EndGame;
        public const int PointsEndingGame = 10;
        
        //Destiny: If thief enabled in settings
        public static bool ThiefActive;

        //Destiny: Loaded game data
        public static bool LoadingGame = false;
        public static int LoadSlotNumber;
        
        //Destiny: Cloud details
        public static int TakingPlayer = -1;

        //Destiny: Managers
        public static BuildManager BuildManager = new();
        public static CardsManager CardsManager = new();
        public static LongestPathManager LongestPathManager = new();
        public static PopupManager PopupManager = new();
        public static PortManager PortManager = new();
        public static ResourceManager ResourceManager = new();
        public static TradeManager TradeManager = new();
        public static SoundManager SoundManager = new();
        
        //Destiny: Logs list
        public static List<string> Logs = new();

        /// <summary>
        /// Setting up game manager
        /// </summary>
        /// <param name="modeText">game mode choosed by players</param>
        public static void Setup(string modeText = "")
        {
            if (!LoadingGame)
            {
                State.Setup(modeText);
            }

            BuildManager.Setup();
            CardsManager.Setup();
            PopupManager.Setup();

            EndGame = false;
        }

        /// <summary>
        /// Switches player depdending on actual mode
        /// </summary>
        public static void SwitchPlayer()
        {
            //Destiny: If it's first turn of elements initial distribution in advanced mode and not last player
            //then switch to next player
            if (State.SwitchingGameMode == SwitchingMode.InitialSwitchingFirst && 
                State.CurrentPlayerId != State.Players.Length - 1)
            {
                SwitchToNextPlayer();
            }
            //Destiny: If it's first turn of elements initial distribution in advanced mode and last player
            //then switch to different mode
            else if (State.SwitchingGameMode == SwitchingMode.InitialSwitchingFirst && 
                State.CurrentPlayerId == State.Players.Length - 1)
            {
                State.SwitchingGameMode = SwitchingMode.InitialSwitchingSecond;
            }
            //Destiny: If it's second turn of elements initial distribution in advanced mode and not first player
            //then switch to previous player
            else if (State.SwitchingGameMode == SwitchingMode.InitialSwitchingSecond && State.CurrentPlayerId != 0)
            {
                SwitchToPreviousPlayer();
            }
            //Destiny: If it's second turn of elements initial distribution in advanced mode and first player
            //then switch to different mode
            else if (State.SwitchingGameMode == SwitchingMode.InitialSwitchingSecond && State.CurrentPlayerId == 0)
            {
                State.SwitchingGameMode = SwitchingMode.GameSwitching;
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
                for (var i = 0; i < State.Players.Length; i++)
                {
                    if (State.Players[i].color == color)
                    {
                        return i;
                    }
                }
                throw new Exception();
            }
            catch
            {                
                Debug.LogError($"Player with color given: {color} could not be found.");
                return -1;
            }
        }

        /// <summary>
        /// Gives UI color from player color (enum)
        /// </summary>
        /// <param name="color">Player color (enum) to convert</param>
        /// <returns>Normal UI color</returns>
        public static Color GetColorByPlayerColor(Player.Player.Color color)
        {
            return color switch
            {
                Player.Player.Color.White => Color.white,
                Player.Player.Color.Yellow => Color.yellow,
                Player.Player.Color.Red => Color.red,
                Player.Player.Color.Blue => Color.blue,
                _ => Color.grey
            };
        }

        /// <summary>
        /// Makes appropriate action suitable to thrown dices
        /// </summary>
        public static void HandleThrowingDices()
        {
            if (State.CurrentDiceThrownNumber != 7)
            {
                ResourceManager.UpdatePlayersResources();
            } 
            else
            {
                State.Players[State.CurrentPlayerId].MoveThief();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns>List of the ids of players who are adjacent to given field and has any resource</returns>
        public static List<int> AdjacentPlayerIdToField(int fieldId)
        {
            List<int> adjacentPlayerIds = new();

            //Destiny: For each junctions adjacent to chosen field
            BoardManager.Fields[fieldId].junctionsID.ForEach(delegate(int junctionId) 
            {
                //Destiny: For each junction owned by any player
                if (((JunctionState)BoardManager.Junctions[junctionId].State).type != JunctionType.None)
                {
                    int playerId = BoardManager.Junctions[junctionId].GetOwnerId();

                    if (playerId != State.CurrentPlayerId && !adjacentPlayerIds.Contains(playerId) && 
                        State.Players[playerId].resources.GetResourceNumber() > 0)
                    {
                        adjacentPlayerIds.Add(playerId);
                    }
                }
            });
            adjacentPlayerIds.Sort();

            return adjacentPlayerIds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if current player has at least 10 points</returns>
        public static bool EndGameCondition()
        {
            return State.Players[State.CurrentPlayerId].score.GetPoints() >= PointsEndingGame;
        }

        /// <summary>
        /// Switches current player to the next player
        /// </summary>
        private static void SwitchToNextPlayer()
        {
            State.CurrentPlayerId = (State.CurrentPlayerId + 1) % State.Players.Length;
        }

        /// <summary>
        /// Switches current player to the previous player
        /// </summary>
        private static void SwitchToPreviousPlayer()
        {
            if (State.CurrentPlayerId == 0)
            {
                State.CurrentPlayerId = State.Players.Length - 1;
            }
            else
            {
                State.CurrentPlayerId -= 1;
            }
        }
    }
}