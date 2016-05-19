using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable UseNullPropagation

namespace BTC
{
    public sealed class Keyboard : MonoBehaviour
    {
        #region Private fields

        // Used in Update() to prevent rebuilding the keyboard when a watched field is changed
        // during build
        private bool m_HadJustBeenBuilt;

        private List<Key> m_Keys;

        private VariablesWatcher m_VarWatcher;

        #endregion

        #region Public fields

        [Watched]
        public Vector3 BlackKeyScale;

        [Watched]
        public string DeviceName;

        [Watched]
        public Pitch FirstKey;

        [Watched, Range(0f, 0.1f)]
        public float GapWidth;

        [Watched, Range(36, 88)]
        public int NumKeys;

        [Watched]
        public Vector3 WhiteKeyScale;

        #endregion

        #region Properties

        public MidiInputListener MidiListener { get; private set; }

        #endregion

        #region Methods

        [UsedImplicitly]
        private void Start()
        {
            m_Keys = new List<Key>(NumKeys);
            m_VarWatcher = new VariablesWatcher(this);
            Build();
        }

        private void Build()
        {
            m_HadJustBeenBuilt = true;

            // Create MIDI input listener
            if (MidiListener != null)
            {
                MidiListener.Dispose();
            }
            MidiListener = new MidiInputListener(DeviceName, NumKeys, FirstKey);

            // Destroy primitives
            foreach (var key in m_Keys)
            {
                Destroy(key.gameObject);
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
            var pitch = FirstKey;
            for (uint i = 0; i < NumKeys; ++i)
            {
                if (Tones.IsOnWhiteKeys(pitch.Tone))
                {
                    var pos = Vector3.right *
                              ((whiteNoteCount + 0.5f) * (WhiteKeyScale.x + GapWidth) - totalLen / 2);
                    m_Keys.Add(Key.Create(pitch, WhiteKeyScale, pos, this));
                    whiteNoteCount++;
                }
                else
                {
                    var pos = new Vector3(
                        whiteNoteCount * (WhiteKeyScale.x + GapWidth) - totalLen / 2f,
                        (BlackKeyScale.y - WhiteKeyScale.y) / 2,
                        (WhiteKeyScale.z - BlackKeyScale.z) / 2);
                    m_Keys.Add(Key.Create(pitch, BlackKeyScale, pos, this));
                }
                pitch = Pitches.NextPitch(pitch);
            }
        }

        [UsedImplicitly]
        private void Update()
        {
            // Rebuild keyboard only if a public variable changed
            if (m_VarWatcher.HasAnyVariableChanged() && !m_HadJustBeenBuilt)
            {
                Debug.Log("[BTC] Rebuilding keyboard map");
                Build();
            }
            else
            {
                m_HadJustBeenBuilt = false;
            }
        }

        [UsedImplicitly]
        private void OnDestroy()
        {
            if (MidiListener != null)
            {
                MidiListener.Dispose();
            }
        }

        #endregion
    }
}