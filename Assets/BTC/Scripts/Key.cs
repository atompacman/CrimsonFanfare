using JetBrains.Annotations;
using UnityEngine;

namespace BTC
{
    public abstract class Key : MonoBehaviour
    {
        #region Nested types

        private MidiInputListener m_InputListener;

        public Pitch Pitch { get; private set; }

        public ButtonState State { get; private set; }

        public float Velocity { get; private set; }

        public enum ButtonState
        {
            IDLE,
            PRESSED,
            HELD,
            RELEASED
        }

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
            key.Pitch = i_Pitch;
            key.State = ButtonState.IDLE;
            key.m_InputListener = i_Keyboard.MidiListener;
            return key;
        }

        protected abstract Color DefaultColor();

        #endregion



        #region Methods

        [UsedImplicitly]
        private void Update()
        {
            // Update button state and velocity
            if (m_InputListener.IsKeyPressed(Pitch))
            {
                State = State == ButtonState.PRESSED || State == ButtonState.HELD
                    ? ButtonState.HELD
                    : ButtonState.PRESSED;
                Velocity = m_InputListener.GetHitVelocity(Pitch);
            }
            else
            {
                State = State == ButtonState.RELEASED || State == ButtonState.IDLE
                    ? ButtonState.IDLE
                    : ButtonState.RELEASED;
            }

            // Create a soldier the first frame the key is pressed
            if (State == ButtonState.PRESSED)
            {
                var pos = transform.position.x + transform.localScale.x * 1.5f;
                NoteSoldier.Create(Direction.RIGHT, pos);
            }

            // Set key color according to velocity
            GetComponent<MeshRenderer>().material.color = State == ButtonState.IDLE
                ? DefaultColor()
                : Velocity * Color.red;
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