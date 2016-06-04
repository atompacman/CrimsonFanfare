using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable ConvertPropertyToExpressionBody

namespace FXG.CrimFan.World.Buildings
{
    public sealed class Barrack : Building
    {
        #region Compile-time constants

        private const float CAN_SPAWN_UNIT_BLINKING_SPEED = 6f;

        private const float COOLDOWN_COLOR_FADE_SPEED = 0.2f;

        private const float COOLDOWN_TIME = 5;

        #endregion

        #region Private fields

        private float m_CooldownStartTime;

        #endregion

        #region Methods

        public override bool CanSpawnUnits()
        {
            return Time.fixedTime - m_CooldownStartTime > COOLDOWN_TIME;
        }

        public void StartCooldown()
        {
            m_CooldownStartTime = Time.fixedTime;
        }

        [UsedImplicitly]
        private void Start()
        {
            m_CooldownStartTime = float.MinValue;
        }

        [UsedImplicitly]
        private void Update()
        {
            Color color;
            if (CanSpawnUnits())
            {
                color = Color.green * (0.5f *
                                       Mathf.Cos(Time.fixedTime * CAN_SPAWN_UNIT_BLINKING_SPEED) +
                                       0.5f);
            }
            else
            {
                color = Color.white *
                        (1 - Mathf.Exp(-(Time.fixedTime - m_CooldownStartTime) *
                                  COOLDOWN_COLOR_FADE_SPEED));
            }
            GetComponent<MeshRenderer>().material.color = color;
        }

        #endregion
    }
}