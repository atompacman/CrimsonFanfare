using FXG.CrimFan.Audio;
using FXG.CrimFan.Common;
using FXG.CrimFan.Config;
using FXG.CrimFan.UI;
using FXG.CrimFan.World;
using JetBrains.Annotations;
using UnityEngine;

namespace FXG.CrimFan.Core
{
    public sealed class Match : MonoBehaviour
    {
        #region Nested types

        public enum Ownership
        {
            TEAM_LEFT,
            TEAM_RIGHT,
            NEUTRAL
        }

        #endregion

        #region Properties

        public Team TeamLeft { get; private set; }

        public Team TeamRight { get; private set; }

        public Keyboard Keyboard { get; private set; }

        #endregion

        #region Static methods

        public static Match CreateComponent(GameConfig i_Config, GameDriver i_Driver)
        {
            var match = i_Driver.gameObject.AddComponent<Match>();
            match.TeamLeft = Team.CreateObject(HorizontalDir.LEFT, Color.green, match);
            match.TeamRight = Team.CreateObject(HorizontalDir.RIGHT, Color.blue, match);
            match.Keyboard = Keyboard.CreateObject(i_Config.KeyboardConfig, match);
            KeyboardInputHandler.CreateComponent(match.Keyboard);
            return match;
        }

        #endregion

        #region Methods

        public Ownership GetKeyOwnership(Pitch i_Pitch)
        {
            var idx = i_Pitch.ToMidi() - Keyboard.Configuration.FirstKey.ToMidi();
            return idx < TeamLeft.TerritorySize
                ? Ownership.TEAM_LEFT
                : idx >= Keyboard.Configuration.NumKeys - TeamRight.TerritorySize
                    ? Ownership.TEAM_RIGHT
                    : Ownership.NEUTRAL;
        }

        public Team GetTeam(HorizontalDir i_Side)
        {
            return i_Side == HorizontalDir.LEFT ? TeamLeft : TeamRight;
        }

        [CanBeNull]
        public Team GetTeam(Pitch i_Key)
        {
            var ownership = GetKeyOwnership(i_Key);
            return ownership == Ownership.NEUTRAL
                ? null
                : ownership == Ownership.TEAM_LEFT
                    ? TeamLeft
                    : TeamRight;
        }

        #endregion
    }
}