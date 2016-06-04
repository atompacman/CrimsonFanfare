﻿using JetBrains.Annotations;
using System.Collections.Generic;
using FXG.CrimFan.Common;
using FXG.CrimFan.Pawn;
using UnityEngine;

// ReSharper disable LoopCanBePartlyConvertedToQuery

// ReSharper disable ConvertPropertyToExpressionBody

namespace FXG.CrimFan.Core
{
    public sealed class FrontLine : MonoBehaviour
    {
        #region Private fields

        private List<NoteSoldier> m_SoldiersOnFront;

        #endregion

        #region Properties

        public float Position { get; private set; }

        public Army Army { get; private set; }

        public IEnumerable<NoteSoldier> Soldiers
        {
            get { return m_SoldiersOnFront; }
        }

        public bool Exists
        {
            get { return m_SoldiersOnFront.Count != 0; }
        }

        #endregion

        #region Static methods

        public static FrontLine CreateComponent(Army i_Army)
        {
            var line = i_Army.gameObject.AddComponent<FrontLine>();
            line.Position = 0;
            line.m_SoldiersOnFront = new List<NoteSoldier>();
            line.Army = i_Army;
            return line;
        }

        #endregion

        #region Methods

        [UsedImplicitly]
        private void Update()
        {
            m_SoldiersOnFront.Clear();

            if (Army.GetNumSoldiers() == 0)
            {
                return;
            }

            // Update front line position
            Position = Army.Side == HorizontalDir.LEFT
                ? float.NegativeInfinity
                : float.PositiveInfinity;

            foreach (var soldier in Army.GetSoldiers())
            {
                var pos = soldier.transform.position.x;

                if (Army.Side == HorizontalDir.LEFT && pos > Position ||
                    Army.Side == HorizontalDir.RIGHT && pos < Position)
                {
                    Position = pos;
                }
            }

            // Update list of soldiers on front line
            foreach (var soldier in Army.GetSoldiers())
            {
                if (Mathf.Approximately(soldier.transform.position.x, Position))
                {
                    m_SoldiersOnFront.Add(soldier);
                }
            }
        }

        #endregion
    }
}