using System.Collections.Generic;
using FXG.CrimFan.Common;
using FXG.CrimFan.Pawn;
using UnityEngine;

// ReSharper disable ConvertPropertyToExpressionBody

namespace FXG.CrimFan.Core
{
    public sealed class Army : MonoBehaviour
    {
        #region Private fields

        private List<NoteSoldier> m_Soldiers;

        #endregion

        #region Properties

        public Team Team { get; private set; }

        public FrontLine FrontLine { get; private set; }

        public HorizontalDir Side
        {
            get { return Team.Side; }
        }

        #endregion

        #region Static methods

        public static Army CreateComponent(Team i_Team)
        {
            var army = i_Team.gameObject.AddComponent<Army>();
            army.m_Soldiers = new List<NoteSoldier>();
            army.Team = i_Team;
            army.FrontLine = FrontLine.CreateComponent(army);
            return army;
        }

        #endregion

        #region Methods

        public void AddSoldier(Vector3 i_Pos)
        {
            m_Soldiers.Add(NoteSoldier.CreateObject(i_Pos, this));
        }

        public void RemoveSoldier(NoteSoldier i_NoteSoldier)
        {
            m_Soldiers.Remove(i_NoteSoldier);
            Destroy(i_NoteSoldier.gameObject);
        }

        public int GetNumSoldiers()
        {
            return m_Soldiers.Count;
        }

        public IEnumerable<NoteSoldier> GetSoldiers()
        {
            return m_Soldiers;
        }

        #endregion
    }
}