using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using FXGuild.CrimFan.Config;
using FXGuild.CrimFan.Audio.Midi;
using FXGuild.CrimFan.Audio;

// ReSharper disable UseNullPropagation

namespace FXGuild.CrimFan.Game.World
{
    public sealed class Keyboard : MonoBehaviour
    {
        #region Private fields

        private KeyboardConfig m_Config;
        private List<Key> m_Keys;
        public MidiInputSource MidiListener { get; private set; }

        #endregion

        #region Methods

        public static Keyboard Create(KeyboardConfig i_Config)
        {
            var kb = new GameObject("Keyboard").AddComponent<Keyboard>();
            kb.m_Config = i_Config;
            return kb;
        }

        [UsedImplicitly]
        private void Start()
        {
            // Create MIDI input listener
            MidiListener = MidiInputSource.Create(m_Config.DeviceName, m_Config.NumKeys, m_Config.FirstKey);

            // First key must be white
            if (!Tones.IsOnWhiteKeys(m_Config.FirstKey.Tone))
            {
                m_Config.FirstKey.Tone = Tones.NextTone(m_Config.FirstKey.Tone);
            }

            // Count number of white keys
            var tone = m_Config.FirstKey.Tone;
            var numWhites = 0;
            for (uint i = 0; i < m_Config.NumKeys; ++i)
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
                --m_Config.NumKeys;
            }
        
            // Compute total keyboard length
            var totalLen = numWhites * (m_Config.WhiteKeyScale.x + m_Config.GapWidth) - m_Config.GapWidth;

            // Create keys
            m_Keys = new List<Key>(m_Config.NumKeys);
            var whiteNoteCount = 0;
            var pitch = m_Config.FirstKey;
            for (uint i = 0; i < m_Config.NumKeys; ++i)
            {
                Vector3 scale;
                Vector3 pos;

                if (Tones.IsOnWhiteKeys(pitch.Tone))
                {
                    pos = Vector3.right * ((whiteNoteCount + 0.5f) * 
                        (m_Config.WhiteKeyScale.x + m_Config.GapWidth) - totalLen / 2);
                    scale = m_Config.WhiteKeyScale;
                    whiteNoteCount++;
                }
                else
                {
                    pos = new Vector3(
                        whiteNoteCount * (m_Config.WhiteKeyScale.x + m_Config.GapWidth) - totalLen / 2f,
                        (m_Config.BlackKeyScale.y - m_Config.WhiteKeyScale.y) / 2,
                        (m_Config.WhiteKeyScale.z - m_Config.BlackKeyScale.z) / 2);
                    scale = m_Config.BlackKeyScale;
                }

                m_Keys.Add(Key.Create(pitch, scale, pos, this));

                pitch = Pitches.NextPitch(pitch);
            }
        }
        
        [UsedImplicitly]
        private void OnDestroy()
        {
            foreach (var key in m_Keys)
            {
                Destroy(key);
            }
        }

        #endregion
    }
}