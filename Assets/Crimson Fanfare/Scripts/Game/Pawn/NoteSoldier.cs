﻿using System.Collections.Generic;
using FXGuild.CrimFan.Common;
using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable ConvertPropertyToExpressionBody

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

        #region Compile-time constants

        private const string PREFAB_NAME = "NoteSoldier";

        #endregion

        #region Runtime constants

        private static readonly GameObject PREFAB;

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

            // CreateComponent health bar
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
                UpdateFireState();
            }
            else
            {
                UpdateMovingState();
            }
        }

        private void UpdateFireState()
        {
            CurrentState = State.FIRING;
            var enemies = new List<NoteSoldier>(Army.EnemyArmy.FrontLine.Soldiers);
            var enemy = enemies[Random.Range(0, enemies.Count)];
            if (!enemy.IsDead)
            {
                enemy.Health -= 0.03f;
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