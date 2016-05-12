using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace BTC
{
    public sealed class Keyboard : MonoBehaviour
    {
        #region Private fields

        private List<GameObject> m_Keys;
        private VariablesWatcher m_VarWatcher;

        #endregion

        #region Public fields

        [Watched]
        public Vector3 BlackKeyScale;

        [Watched]
        public Pitch FirstKey;

        [Watched, Range(0f, 0.1f)]
        public float GapWidth;

        [Watched, Range(36, 88)]
        public int NumKeys;

        [Watched]
        public Vector3 WhiteKeyScale;

        #endregion

        #region Methods

        [UsedImplicitly]
        private void Start()
        {
            m_Keys = new List<GameObject>(NumKeys);
            m_VarWatcher = new VariablesWatcher(this);
            Build();
        }

        private void Build()
        {
            // Destroy primitives
            foreach (var key in m_Keys)
            {
                Destroy(key);
            }
            m_Keys.Clear();

            // First key must be white
            if (!Tones.IsOnWhiteKeys(FirstKey.Tone))
            {
                FirstKey.Tone = Tones.NextTone(FirstKey.Tone);
            }
            var tone = FirstKey.Tone;

            // Count number of white keys
            var numWhites = 0;
            for (uint i = 0; i < NumKeys; ++i)
            {
                if (Tones.IsOnWhiteKeys(tone))
                {
                    ++numWhites;
                }
                tone = Tones.NextTone(tone);
            }

            // Last key must be white
            if (!Tones.IsOnWhiteKeys(Tones.PreviousTone(tone)))
            {
                --NumKeys;
            }

            // Compute total keyboard length
            var totalLen = numWhites * (WhiteKeyScale.x + GapWidth) - GapWidth;

            // Create keys
            var whiteNoteCount = 0;
            tone = FirstKey.Tone;
            for (uint i = 0; i < NumKeys; ++i)
            {
                m_Keys.Add(Tones.IsOnWhiteKeys(tone)
                    ? CreateWhiteNote(whiteNoteCount++, totalLen)
                    : CreateBlackNote(whiteNoteCount, totalLen));

                tone = Tones.NextTone(tone);
            }
        }

        private GameObject CreateWhiteNote(int i_Idx, float i_TotalLen)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.GetComponent<MeshRenderer>().material.color = Color.white;
            obj.transform.localScale = WhiteKeyScale;
            var scale = obj.transform.localScale;
            scale.x = WhiteKeyScale.x;
            obj.transform.localScale = scale;
            obj.transform.position = Vector3.right *
                                     ((i_Idx + 0.5f) * (WhiteKeyScale.x + GapWidth) -
                                      i_TotalLen / 2);
            obj.transform.parent = transform;
            return obj;
        }

        private GameObject CreateBlackNote(int i_Idx, float i_TotalLen)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.GetComponent<MeshRenderer>().material.color = Color.black;
            obj.transform.localScale = BlackKeyScale;
            obj.transform.position = new Vector3(
                i_Idx * (WhiteKeyScale.x + GapWidth) - i_TotalLen / 2f,
                (BlackKeyScale.y - WhiteKeyScale.y) / 2,
                (WhiteKeyScale.z - BlackKeyScale.z) / 2);
            obj.transform.parent = transform;
            return obj;
        }

        [UsedImplicitly]
        private void Update()
        {
            // Rebuild keyboard only if a public variable changed
            if (!m_VarWatcher.HasAnyVariableChanged())
            {
                return;
            }

            Debug.Log("[BTC] Rebuilding keyboard map");
            Build();
        }

        #endregion
    }
}