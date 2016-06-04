using FXG.CrimFan.Audio;
using FXG.CrimFan.Core;
using FXG.CrimFan.Pawn;
using FXG.CrimFan.World;
using UnityEngine;

namespace FXG.CrimFan.UI.State
{
    public sealed class YoloState : MenuState
    {
        #region Constructors

        public YoloState(Team i_Team) :
            base(i_Team)
        {
        }

        #endregion

        #region Methods

        public override void InitializeKey(Key i_Key, bool i_IsOnTerritory)
        {
            OnKeyboardKeyReleased(i_Key, i_IsOnTerritory);
        }

        public override void OnKeyboardKeyHit(Key i_Key, bool i_IsOnTerritory)
        {
            var kb = Team.Match.Keyboard;

            if (i_IsOnTerritory)
            {
                Debug.Assert(i_Key.IsWhiteKey);
                var kc = kb.Configuration;

                Team.Army.AddSoldier(new Vector3(
                    i_Key.transform.position.x,
                    NoteSoldier.HEIGHT,
                    kc.WhiteKeyScale.z / 2 -
                    Mathf.Lerp(kc.BlackKeyScale.z, kc.WhiteKeyScale.z, Random.value)));
            }

            i_Key.SetColor(Color.red * kb.MidiSource.GetHitVelocity(i_Key.Pitch));
        }

        public override void OnKeyboardKeyReleased(Key i_Key, bool i_IsOnTerritory)
        {
            if (i_IsOnTerritory)
            {
                var color = Team.Color;
                if (!i_Key.IsWhiteKey)
                {
                    color *= 0.25f;
                }
                i_Key.SetColor(color);
            }
            else
            {
                i_Key.SetDefaultColor();
            }
        }

        #endregion
    }
}