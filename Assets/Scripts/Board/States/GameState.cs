using System;

namespace Board.States
{
    [Serializable]
    public class GameState
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

        //Destiny: Players data
        public Player.Player[] Players;
        public int CurrentPlayerId;

        //Destiny: Current thrown dice number
        public int CurrentDiceThrownNumber;

        //Destiny: Game and user modes
        public CatanMode Mode;
        public SwitchingMode SwitchingGameMode;
        public MovingMode MovingUserMode;
        public BasicMovingMode BasicMovingUserMode;

        public void SetState(GameState state)
        {
            Players = state.Players;
            CurrentPlayerId = state.CurrentPlayerId;
            CurrentDiceThrownNumber = state.CurrentDiceThrownNumber;
            Mode = state.Mode;
            SwitchingGameMode = state.SwitchingGameMode;
            MovingUserMode = state.MovingUserMode;
            BasicMovingUserMode = state.BasicMovingUserMode;
        }

        /// <summary>
        /// Setting up game manager basic information based on popups inputs
        /// </summary>
        /// <param name="modeText">game mode choosed by players</param>
        public void Setup(string modeText)
        {
            CurrentPlayerId = 0;
            CurrentDiceThrownNumber = 0;

            Mode = modeText == "PODSTAWOWY" ? CatanMode.Basic : CatanMode.Advanced;
            SwitchingGameMode = Mode == CatanMode.Basic ? SwitchingMode.GameSwitching : SwitchingMode.InitialSwitchingFirst;
            MovingUserMode = Mode == CatanMode.Basic ? MovingMode.ThrowDice : MovingMode.BuildVillage;

            BasicMovingUserMode = Mode == CatanMode.Basic ? BasicMovingMode.TradePhase : BasicMovingMode.Normal;
        }
    }
}
