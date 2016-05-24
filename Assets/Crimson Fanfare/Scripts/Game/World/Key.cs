using FXGuild.CrimFan.Audio;
using FXGuild.CrimFan.Audio.Midi;
using JetBrains.Annotations;
using UnityEngine;

namespace FXGuild.CrimFan.Game.World
{
    public sealed class Key : MonoBehaviour
    {
        #region Nested types

        private MidiInputSource m_InputSrc;

        public Pitch Pitch { get; private set; }

        private Keyboard m_Keyboard;

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
            key.m_Keyboard = i_Keyboard;
            return key;
        }

        #endregion

        #region Methods

        [UsedImplicitly]
        private void Update()
        {
            // Get team associated with this key
            var team = m_Keyboard.CurrentMatch.GetTeam(Pitch);

            // If the key belongs to a team, create a soldier the first frame the key is pressed
            if (m_InputSrc.IsKeyHit(Pitch) && team != null)
            {
                team.Army.AddSoldier(transform.position.x + transform.localScale.x * 1.5f);
            }

            // Set key color
            Color color;
            if (m_InputSrc.IsKeyPressed(Pitch))
            {
                color = Color.red * m_InputSrc.GetHitVelocity(Pitch);
            }
            else
            {
                if (team == null)
                {
                    color = Tones.IsOnWhiteKeys(Pitch.Tone) ? Color.white : Color.black;
                }
                else
                {
                    color = team.Color;
                    if (!Tones.IsOnWhiteKeys(Pitch.Tone))
                    {
                        color *= 0.25f;
                    }
                }
            }

            GetComponent<MeshRenderer>().material.color = color;
        }

        #endregion
    }
}