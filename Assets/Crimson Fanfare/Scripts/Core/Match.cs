﻿using FXG.CrimFan.Audio;
using FXG.CrimFan.Common;
using FXG.CrimFan.Config;
using FXG.CrimFan.UI;
using FXG.CrimFan.World;
using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable ConvertPropertyToExpressionBody

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

        #region Compile-time constants

        public const string GAME_OBJ_NAME = "Match";

        #endregion

        #region Properties

        public Team TeamLeft { get; private set; }

        public Team TeamRight { get; private set; }

        public Keyboard Keyboard { get; private set; }

        public KeyboardInputHandler KeyboardInputHandler { get; private set; }

        public bool IsOver
        {
            get { return TeamLeft.IsDefeated || TeamRight.IsDefeated; }
        }

        #endregion

        #region Static methods

        public static Match CreateObject(GameConfig i_Config, GameDriver i_Driver)
        {
            var match = new GameObject(GAME_OBJ_NAME).AddComponent<Match>();
            match.transform.parent = i_Driver.transform;
            match.TeamLeft = Team.CreateObject(HorizontalDir.LEFT, Color.green, match);
            match.TeamRight = Team.CreateObject(HorizontalDir.RIGHT, Color.blue, match);
            match.Keyboard = Keyboard.CreateObject(i_Config.KeyboardConfig, match);
            match.KeyboardInputHandler = KeyboardInputHandler.CreateComponent(match.Keyboard);
            StaticGui.CreateObject(match);
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