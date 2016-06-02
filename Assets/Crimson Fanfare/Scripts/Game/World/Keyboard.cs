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

        public MidiInputSource MidiListener { get; private set; }
        public Match CurrentMatch { get; private set; }
        public KeyboardConfig Configuration { get; private set; }

        #endregion

        #region Static methods

        public static Keyboard CreateObject(KeyboardConfig i_Config, Match i_Match)
        {
            var kb = new GameObject("Keyboard").AddComponent<Keyboard>();
            kb.transform.parent = i_Match.transform;
            kb.Configuration = i_Config;
            kb.CurrentMatch = i_Match;

            // CreateComponent MIDI input listener
            kb.MidiListener = MidiInputSource.CreateComponent(i_Config.DeviceName, i_Config.NumKeys, i_Config.FirstKey, kb.gameObject);

            // First key must be white
            if (!Tones.IsOnWhiteKeys(i_Config.FirstKey.Tone))
            {
                i_Config.FirstKey.Tone = Tones.NextTone(i_Config.FirstKey.Tone);
            }

            // Count number of white keys
            var tone = i_Config.FirstKey.Tone;
            var numWhites = 0;
            for (uint i = 0; i < i_Config.NumKeys; ++i)
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
                --i_Config.NumKeys;
            }

            // Compute total keyboard length
            var totalLen = numWhites * (i_Config.WhiteKeyScale.x + i_Config.GapWidth) - i_Config.GapWidth;

            // CreateObject keys
            kb.m_Keys = new List<Key>(i_Config.NumKeys);
            var whiteNoteCount = 0;
            var pitch = i_Config.FirstKey;
            for (uint i = 0; i < i_Config.NumKeys; ++i)
            {
                Vector3 scale;
                Vector3 pos;

                if (Tones.IsOnWhiteKeys(pitch.Tone))
                {
                    pos = Vector3.right * ((whiteNoteCount + 0.5f) *
                                           (i_Config.WhiteKeyScale.x + i_Config.GapWidth) - totalLen / 2);
                    scale = i_Config.WhiteKeyScale;
                    whiteNoteCount++;
                }
                else
                {
                    pos = new Vector3(
                        whiteNoteCount * (i_Config.WhiteKeyScale.x + i_Config.GapWidth) - totalLen / 2f,
                        (i_Config.BlackKeyScale.y - i_Config.WhiteKeyScale.y) / 2,
                        (i_Config.WhiteKeyScale.z - i_Config.BlackKeyScale.z) / 2);
                    scale = i_Config.BlackKeyScale;
                }

                kb.m_Keys.Add(Key.CreateObject(pitch, scale, pos, kb));

                pitch = Pitches.NextPitch(pitch);
            }

            return kb;
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