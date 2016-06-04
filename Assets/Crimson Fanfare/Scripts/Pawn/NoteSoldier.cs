using System.Linq;
using FXG.CrimFan.Common;
using FXG.CrimFan.Core;
using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable SuggestBaseTypeForParameter

// ReSharper disable ConvertPropertyToExpressionBody

namespace FXG.CrimFan.Pawn
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

        #region Compile-time constants

        public const float HEIGHT = 0.2f;

        private const string PREFAB_NAME = "NoteSoldier";

        #endregion

        #region Runtime constants

        private static readonly GameObject PREFAB;

        #endregion

        #region Private fields

        [CanBeNull]
        private NoteSoldier m_Target;

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

        public bool IsDead
        {
            get { return Mathf.Approximately(Health, 0); }
        }

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
            soldier.Army = i_Army;
            soldier.Range = 0.5f + Random.value * 0.3f;
            soldier.InitialHealth = Random.Range(2, 10);
            soldier.Health = soldier.InitialHealth;
            soldier.FireRate = Random.value * 1f;
            soldier.CurrentState = State.MOVING;
            soldier.m_Target = null;

            // Face good direction
            if (soldier.Army.Side == HorizontalDir.RIGHT)
            {
                var scale = soldier.transform.localScale;
                scale.x *= -1;
                soldier.transform.localScale = scale;
            }

            // CreateComponent health bar
            HealthBar.CreateObject(soldier);

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

            // If we must find a new target
            if (m_Target == null || m_Target.IsDead || !IsInRange(m_Target))
            {
                m_Target = (from enemy in Army.Team.EnemyTeam.Army.GetSoldiers()
                    orderby DistanceTo(enemy) ascending
                    where !enemy.IsDead && IsInRange(enemy)
                    select enemy).FirstOrDefault();
            }

            if (m_Target == null)
            {
                UpdateMovingState();
            }
            else
            {
                UpdateFireState();
            }
        }

        private float DistanceTo(NoteSoldier i_Enemy)
        {
            return Mathf.Abs(i_Enemy.transform.position.x - transform.position.x);
        }

        private bool IsInRange(NoteSoldier i_Enemy)
        {
            return DistanceTo(i_Enemy) < Range;
        }

        private void UpdateFireState()
        {
            CurrentState = State.FIRING;
            if (m_Target != null && !m_Target.IsDead)
            {
                m_Target.Health -= 0.03f;
            }
        }

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
        }

        #endregion
    }
}