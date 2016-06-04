using FXG.CrimFan.Common;
using FXG.CrimFan.Core;
using FXG.CrimFan.Pawn;
using FXG.CrimFan.World;
using UnityEngine;

namespace FXG.CrimFan.UI.State
{
    public sealed class StandardState : MenuState
    {
        #region Compile-time constants

        private const float RED_FADE_OUT_RATE = 1f;

        #endregion

        #region Constructors

        public StandardState(Team i_Team = null) :
            base(i_Team)
        {
        }

        #endregion

        #region Methods
        
        public override void OnKeyboardKeyIdle(Key i_Key, bool i_IsOnTerritory)
        {
            UpdateKeyColor(i_Key, i_IsOnTerritory, true);
        }

        public override void OnKeyboardKeyHit(Key i_Key, bool i_IsOnTerritory)
        {
            UpdateKeyColor(i_Key, i_IsOnTerritory, false);
        }

        public override bool OnKeyboardKeyReleased(Key i_Key, bool i_IsOnTerritory)
        {
            if (!i_IsOnTerritory)
            {
                return false;
            }

            if (i_Key.IsWhiteKey)
            {
                var kc = Team.Match.Keyboard.Configuration;

                Team.Army.AddSoldier(new Vector3(
                    i_Key.transform.position.x,
                    NoteSoldier.HEIGHT,
                    kc.WhiteKeyScale.z / 2 -
                    Mathf.Lerp(kc.BlackKeyScale.z, kc.WhiteKeyScale.z, Random.value)));
                return false;
            }

            ChangeState(new BuildingCreationConfirmationState(i_Key));
            return true;
        }

        private void UpdateKeyColor(Key i_Key, bool i_IsOnTerritory, bool i_FadeOut)
        {
            var baseColor = i_IsOnTerritory
                ? i_Key.IsWhiteKey ? Team.Color : Team.Color * 0.25f
                : i_Key.DefaultColor;

            var velocity = MidiSource.GetHitVelocity(i_Key.Pitch);
            var etsr = i_FadeOut ? MidiSource.GetElapsedTimeSinceRelease(i_Key.Pitch) : 0;
            var factor = Mathf.Clamp01(1 - velocity + etsr / RED_FADE_OUT_RATE);

            i_Key.SetColor(Color.Lerp(Team.Color.GetInverse(), baseColor, factor));
        }

        #endregion
    }
}