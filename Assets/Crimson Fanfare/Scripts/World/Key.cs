using FXG.CrimFan.Audio;
using UnityEngine;

// ReSharper disable ConvertPropertyToExpressionBody

namespace FXG.CrimFan.World
{
    public sealed class Key : MonoBehaviour
    {
        #region Properties

        public Pitch Pitch { get; private set; }

        public bool IsWhiteKey
        {
            get { return Tones.IsOnWhiteKeys(Pitch.Tone); }
        }

        public Color DefaultColor
        {
            get { return IsWhiteKey ? Color.white : Color.black; }
        }

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
            key.Pitch = i_Pitch;
            key.SetColor(key.DefaultColor);
            return key;
        }

        #endregion

        #region Methods

        public void SetColor(Color i_Color)
        {
            GetComponent<MeshRenderer>().material.color = i_Color;
        }

        #endregion
    }
}