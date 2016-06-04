using FXG.CrimFan.Audio;
using FXG.CrimFan.World.Buildings;
using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable ConvertPropertyToExpressionBody

namespace FXG.CrimFan.World
{
    public sealed class Key : MonoBehaviour
    {
        #region Properties

        public Pitch Pitch { get; private set; }

        [CanBeNull]
        public Building Building { get; private set; }

        public bool IsWhiteKey
        {
            get { return Tones.IsOnWhiteKeys(Pitch.Tone); }
        }

        public Color DefaultColor
        {
            get { return IsWhiteKey ? Color.white : Color.black; }
        }

        public Color Color
        {
            get { return GetComponent<MeshRenderer>().material.color; }
            set { GetComponent<MeshRenderer>().material.color = value; }
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
            key.Building = null;
            key.Color = key.DefaultColor;
            return key;
        }

        #endregion

        #region Methods

        public void SetBuilding<T>() where T : Building
        {
            Debug.Assert(Building == null);
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.parent = transform;
            obj.transform.localScale = new Vector3(1, 0.15f, 0.8f);
            obj.transform.position = transform.position +
                                     Vector3.up *
                                     (transform.localScale.y + obj.transform.lossyScale.y) / 2;

            Building = obj.AddComponent<T>();
            if (Building == null)
            {
                Debug.LogError("Could not create building");
                return;
            }
            Building.Key = this;
        }

        #endregion
    }
}