using System.Collections.Generic;
using FXG.CrimFan.Audio;
using FXG.CrimFan.Audio.Midi;
using FXG.CrimFan.Config;
using FXG.CrimFan.Core;
using FXG.CrimFan.Pawn;
using FXG.CrimFan.World.Buildings;
using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable UseNullPropagation

namespace FXG.CrimFan.World
{
    public sealed class Keyboard : MonoBehaviour
    {
        #region Private fields

        private List<Key> m_Keys;

        #endregion

        #region Properties

        public MidiInputSource MidiSource { get; private set; }

        public Match Match { get; private set; }

        public KeyboardConfig Configuration { get; private set; }

        #endregion

        #region Static methods

        public static Keyboard CreateObject(KeyboardConfig i_Config, Match i_Match)
        {
            var kb = new GameObject("Keyboard").AddComponent<Keyboard>();
            kb.transform.parent = i_Match.transform;
            kb.Configuration = i_Config;
            kb.Match = i_Match;

            // Create MIDI input source
            kb.MidiSource = MidiInputSource.CreateComponent(kb);

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
            var totalLen = numWhites * (i_Config.WhiteKeyScale.x + i_Config.GapWidth) -
                           i_Config.GapWidth;

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
                                           (i_Config.WhiteKeyScale.x + i_Config.GapWidth) -
                                           totalLen / 2);
                    scale = i_Config.WhiteKeyScale;
                    whiteNoteCount++;
                }
                else
                {
                    pos = new Vector3(
                        whiteNoteCount * (i_Config.WhiteKeyScale.x + i_Config.GapWidth) -
                        totalLen / 2f,
                        (i_Config.BlackKeyScale.y - i_Config.WhiteKeyScale.y) / 2,
                        (i_Config.WhiteKeyScale.z - i_Config.BlackKeyScale.z) / 2);
                    scale = i_Config.BlackKeyScale;
                }

                kb.m_Keys.Add(Key.CreateObject(pitch, scale, pos, kb));

                pitch = Pitches.NextPitch(pitch);
            }

            return kb;
        }

        private static void StartCooldownIfPossible(Barrack i_Barrack)
        {
            if (i_Barrack != null && i_Barrack.CanSpawnUnits())
            {
                i_Barrack.StartCooldown();
            }
        }

        #endregion

        #region Methods

        public void CreateUnit(Pitch i_Key)
        {
            Debug.Assert(i_Key.IsOnWhiteKeys);

            var team = Match.GetTeam(i_Key);

            if (team == null)
            {
                Debug.LogError("Null team");
                return;
            }

            var prev = Pitches.PreviousPitch(i_Key);
            var next = Pitches.NextPitch(i_Key);
            var prevBuild = IsKeyInsideBounds(prev) ? GetKey(prev).Building : null;
            var nextBuild = IsKeyInsideBounds(next) ? GetKey(next).Building : null;

            if ((prevBuild == null || !prevBuild.CanSpawnUnits()) &&
                (nextBuild == null || !nextBuild.CanSpawnUnits()))
            {
                return;
            }

            team.Army.AddSoldier(new Vector3(
                GetKey(i_Key).transform.position.x,
                NoteSoldier.HEIGHT,
                Configuration.WhiteKeyScale.z / 2 -
                Mathf.Lerp(Configuration.BlackKeyScale.z, Configuration.WhiteKeyScale.z,
                    Random.value)));

            StartCooldownIfPossible((Barrack) prevBuild);
            StartCooldownIfPossible((Barrack) nextBuild);
        }

        public Pitch GetPitch(int i_KeyId)
        {
            return m_Keys[i_KeyId].Pitch;
        }

        public Key GetKey(Pitch i_Pitch)
        {
            return m_Keys[ToIndex(i_Pitch)];
        }

        public bool IsKeyInsideBounds(Pitch i_Key)
        {
            return ToIndex(i_Key) >= 0 && ToIndex(i_Key) < Configuration.NumKeys;
        }

        public bool IsWithinLimits(NoteSoldier i_Unit)
        {
            var posX = i_Unit.transform.position.x;
            var first = m_Keys[0].transform;
            var last = m_Keys[m_Keys.Count - 1].transform;
            return posX > first.position.x - first.localScale.x &&
                   posX < last.position.x + last.localScale.x;
        }

        private int ToIndex(Pitch i_Pitch)
        {
            return i_Pitch.ToMidi() - Configuration.FirstKey.ToMidi();
        }
        
        #endregion
    }
}