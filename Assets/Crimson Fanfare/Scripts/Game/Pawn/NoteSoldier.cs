using FXGuild.CrimFan.Common;
using JetBrains.Annotations;
using UnityEngine;

namespace FXGuild.CrimFan.Game.Pawn
{
    public sealed class NoteSoldier : MonoBehaviour
    {
        #region Nested types

        public enum State
        {
            FIRING,
            MOVING
        }

        #endregion

        #region Runtime constants

        private static readonly GameObject PREFAB;

        private const string PREFAB_NAME = "NoteSoldier";

        #endregion

        #region Private fields

        private float m_FiringStartTime;

        #endregion

        #region Constructors

        static NoteSoldier()
        {
            PREFAB = Utils.LoadPrefab(PREFAB_NAME);
        }

        #endregion

        #region Properties

        public Army Army { get; private set; }

        public float Range { get; private set; }

        public float InitialHealth { get; private set; }

        public float Health { get; private set; }

        public float FireRate { get; private set; }

        public State CurrentState { get; private set; }

        #endregion

        #region Static methods

        public static NoteSoldier CreateObject(Vector3 i_Position, Army i_Army)
        {
            var obj = Instantiate(PREFAB.gameObject);
            obj.transform.parent = i_Army.transform;
            obj.transform.position = i_Position;
            var soldier = obj.GetComponent<NoteSoldier>();
            soldier.m_FiringStartTime = 0;
            soldier.Army = i_Army;
            soldier.Range = Random.value * 1f;
            soldier.InitialHealth = Random.Range(2, 10);
            soldier.Health = soldier.InitialHealth;
            soldier.FireRate = Random.value * 1f;
            soldier.CurrentState = State.MOVING;

            // Face good direction
            if (soldier.Army.Side == HorizontalDir.RIGHT)
            {
                var scale = soldier.transform.localScale;
                scale.x *= -1;
                soldier.transform.localScale = scale;
            }

            // Create health bar
            HealthBar.CreateObject(obj);

            return soldier;
        }

        #endregion

        #region Methods

        [UsedImplicitly]
        private void Update()
        {
            // Die when there is no health left
            if (Health <= 0)
            {
                Army.RemoveSoldier(this);
                return;
            }

            var efl = Army.EnemyArmy.FrontLine;
            if (efl.Exists && Mathf.Abs(efl.Position - transform.position.x) < Range)
            {
                //UpdateFireState();
                Health -= 0.1f;
            }
            else
            {
                UpdateMovingState();
            }
        }

        /*
        private void UpdateFireState()
        {
            CurrentState = State.FIRING;

            if (Mathf.Approximately(m_FiringStartTime, 0))
            {
                m_FiringStartTime = Time.fixedTime;
            }

            Army.EnemyArmy.FrontLine.Soldiers
        }
        */

        private void UpdateMovingState()
        {
            CurrentState = State.MOVING;
            if (Army.Side == HorizontalDir.RIGHT)
            {
                transform.position += Vector3.left * 0.01f;
            }
            else
            {
                transform.position += Vector3.right * 0.01f;
            }
            m_FiringStartTime = 0;
        }

        #endregion
    }
}