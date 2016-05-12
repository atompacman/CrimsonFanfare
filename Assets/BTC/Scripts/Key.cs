using JetBrains.Annotations;
using UnityEngine;

namespace BTC
{
    public abstract class Key : MonoBehaviour
    {
        #region Properties

        public Pitch Pitch { get; private set; }

        #endregion

        #region Abstract methods

        protected abstract Color DefaultColor();

        #endregion

        #region Static methods

        public static Key Create(Pitch i_Pitch, Vector3 i_Scale, Vector3 i_Pos, Keyboard i_KeyB)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = i_Pitch.ToString();
            Destroy(obj.GetComponent<MeshCollider>());
            obj.transform.localScale = i_Scale;
            obj.transform.position = i_Pos;
            obj.transform.parent = i_KeyB.transform;
            var key = Tones.IsOnWhiteKeys(i_Pitch.Tone)
                ? obj.AddComponent<WhiteKey>()
                : (Key) obj.AddComponent<BlackKey>();
            key.Pitch = i_Pitch;
            return key;
        }

        #endregion

        #region Methods

        [UsedImplicitly]
        private void Update()
        {
            GetComponent<MeshRenderer>().material.color =
                Input.GetKey(KeyCode.A + Pitch.Octave * 12 + (int) Pitch.Tone)
                    ? Color.red
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