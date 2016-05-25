using System.Collections.Generic;
using FXGuild.CrimFan.Audio;
using FXGuild.CrimFan.Audio.Midi;
using FXGuild.CrimFan.Config;
using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable UseNullPropagation

namespace FXGuild.CrimFan.Game.World
{
    public sealed class Keyboard : MonoBehaviour
    {
        #region Private fields

        private List<Key> m_Keys;

        #endregion

        #region Properties

        public KeyboardConfig Config { get; private set; }
        public MidiInputSource MidiListener { get; private set; }
        public Match CurrentMatch { get; private set; }

        #endregion

        #region Static methods

        public static Keyboard Create(KeyboardConfig i_Config, Match i_Match)
        {
            var kb = new GameObject("Keyboard").AddComponent<Keyboard>();
            kb.Config = i_Config;
            kb.CurrentMatch = i_Match;
            return kb;
        }

        #endregion

        #region Methods

        [UsedImplicitly]
        private void Start()
        {
            // Create MIDI input listener
            MidiListener = MidiInputSource.Create(Config.DeviceName, Config.NumKeys, Config.FirstKey);

            // First key must be white
            if (!Tones.IsOnWhiteKeys(Config.FirstKey.Tone))
            {
                Config.FirstKey.Tone = Tones.NextTone(Config.FirstKey.Tone);
            }

            // Count number of white keys
            var tone = Config.FirstKey.Tone;
            var numWhites = 0;
            for (uint i = 0; i < Config.NumKeys; ++i)
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
                --Config.NumKeys;
            }

            // Compute total keyboard length
            var totalLen = numWhites * (Config.WhiteKeyScale.x + Config.GapWidth) - Config.GapWidth;

            // Create keys
            m_Keys = new List<Key>(Config.NumKeys);
            var whiteNoteCount = 0;
            var pitch = Config.FirstKey;
            for (uint i = 0; i < Config.NumKeys; ++i)
            {
                Vector3 scale;
                Vector3 pos;

                if (Tones.IsOnWhiteKeys(pitch.Tone))
                {
                    pos = Vector3.right * ((whiteNoteCount + 0.5f) *
                                           (Config.WhiteKeyScale.x + Config.GapWidth) - totalLen / 2);
                    scale = Config.WhiteKeyScale;
                    whiteNoteCount++;
                }
                else
                {
                    pos = new Vector3(
                        whiteNoteCount * (Config.WhiteKeyScale.x + Config.GapWidth) - totalLen / 2f,
                        (Config.BlackKeyScale.y - Config.WhiteKeyScale.y) / 2,
                        (Config.WhiteKeyScale.z - Config.BlackKeyScale.z) / 2);
                    scale = Config.BlackKeyScale;
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