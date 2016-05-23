using FXGuild.CrimFan.Audio;
using FXGuild.CrimFan.Audio.Midi;
using FXGuild.CrimFan.Common;
using FXGuild.CrimFan.Game.Pawn;
using JetBrains.Annotations;
using UnityEngine;

namespace FXGuild.CrimFan.Game.World
{
    public sealed class Key : MonoBehaviour
    {
        #region Nested types

        private MidiInputSource m_InputSrc;

        public Pitch Pitch { get; private set; }

        #endregion

        #region Abstract methods

        public static Key Create(Pitch i_Pitch, Vector3 i_Scale, Vector3 i_Pos, Keyboard i_Keyboard)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = i_Pitch.ToString();
            Destroy(obj.GetComponent<MeshCollider>());
            obj.transform.localScale = i_Scale;
            obj.transform.position = i_Pos;
            obj.transform.parent = i_Keyboard.transform;
            var key = obj.AddComponent<Key>();
            key.m_InputSrc = i_Keyboard.MidiListener;
            key.Pitch = i_Pitch;
            return key;
        }

        #endregion

        #region Methods

        [UsedImplicitly]
        private void Update()
        {
            // Create a soldier the first frame the key is pressed
            if (m_InputSrc.IsKeyHit(Pitch))
            {
                var pos = transform.position.x + transform.localScale.x * 1.5f;
                NoteSoldier.Create(HorizontalDir.RIGHT, pos);
            }

            // Set key color
            Color color;
            if (m_InputSrc.IsKeyPressed(Pitch))
            {
                color = Color.red * m_InputSrc.GetHitVelocity(Pitch);
            }
            else
            {
                var ownership = Match.CurrentMatch.GetKeyOwnership(Pitch);

                if (ownership == Match.KeyOwnership.Neutral)
                {
                    color = Tones.IsOnWhiteKeys(Pitch.Tone) ? Color.white : Color.black;
                }
                else
                {
                    color = ownership == Match.KeyOwnership.TeamLeft 
                        ? Match.CurrentMatch.TeamLeft.Color
                        : Match.CurrentMatch.TeamRight.Color;
                    if (!Tones.IsOnWhiteKeys(Pitch.Tone))
                    {
                        color /= 4;
                    }
                }
            }

            GetComponent<MeshRenderer>().material.color = color;
        }

        #endregion
    }
}