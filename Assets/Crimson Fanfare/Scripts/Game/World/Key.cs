using FXGuild.CrimFan.Audio;
using FXGuild.CrimFan.Audio.Midi;
using FXGuild.CrimFan.Common;
using FXGuild.CrimFan.Game.Pawn;
using JetBrains.Annotations;
using UnityEngine;

namespace FXGuild.CrimFan.Game.World
{
    public abstract class Key : MonoBehaviour
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
            var key = Tones.IsOnWhiteKeys(i_Pitch.Tone)
                ? obj.AddComponent<WhiteKey>()
                : (Key)obj.AddComponent<BlackKey>();
            key.m_InputSrc = i_Keyboard.MidiListener;
            key.Pitch = i_Pitch;
            return key;
        }

        protected abstract Color DefaultColor();

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

            // Set key color according to velocity
            GetComponent<MeshRenderer>().material.color = m_InputSrc.IsKeyPressed(Pitch)
                ? m_InputSrc.GetHitVelocity(Pitch) * Color.red
                : DefaultColor();
        }

        #endregion
    }

    public sealed class WhiteKey : Key
    {
        #region Methods

        protected override Color DefaultColor()
        {
            return Color.white;
        }

        #endregion
    }

    public sealed class BlackKey : Key
    {
        #region Methods

        protected override Color DefaultColor()
        {
            return Color.black;
        }

        #endregion
    }
}