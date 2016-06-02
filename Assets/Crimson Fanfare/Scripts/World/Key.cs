using FXG.CrimFan.Audio;
using FXG.CrimFan.Audio.Midi;
using JetBrains.Annotations;
using UnityEngine;

namespace FXG.CrimFan.World
{
    public sealed class Key : MonoBehaviour
    {
        #region Compile-time constants

        private const float NOTE_SOLDIER_HEIGHT = 0.2f;

        #endregion

        #region Private fields

        private MidiInputSource m_InputSrc;

        private Keyboard m_Keyboard;

        #endregion

        #region Properties

        public Pitch Pitch { get; private set; }

        #endregion

        #region Static methods

        public static Key CreateObject(Pitch i_Pitch, Vector3 i_Scale, Vector3 i_Pos,
            Keyboard i_Keyboard)
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
            // ReSharper disable once UseNullPropagation
            if (m_InputSrc.IsKeyHit(Pitch) && team != null)
            {
                Debug.Assert(Tones.IsOnWhiteKeys(Pitch.Tone));

                var kc = m_Keyboard.Configuration;
                team.Army.AddSoldier(new Vector3(
                    transform.position.x,
                    NOTE_SOLDIER_HEIGHT,
                    kc.WhiteKeyScale.z / 2 -
                    Mathf.Lerp(kc.BlackKeyScale.z, kc.WhiteKeyScale.z, Random.value)));
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