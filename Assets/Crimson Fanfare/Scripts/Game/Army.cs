using System.Collections.Generic;
using FXGuild.CrimFan.Common;
using FXGuild.CrimFan.Game.Pawn;
using JetBrains.Annotations;

namespace FXGuild.CrimFan.Game
{
    public sealed class Army
    {
        #region Private fields

        private readonly List<NoteSoldier> m_Soldiers;

        private readonly HorizontalDir m_Side;

        #endregion

        #region Constructors

        public Army(HorizontalDir i_Side)
        {
            m_Soldiers = new List<NoteSoldier>();
            m_Side = i_Side;

            ResetFrontLine();
        }

        #endregion

        #region Properties

        public float FrontLinePosition { get; private set; }

        public NoteSoldier FrontLineSoldier { get; private set; }

        #endregion

        #region Methods

        public void AddSoldier(float i_Pos)
        {
            m_Soldiers.Add(NoteSoldier.Create(Utils.OppositeDir(m_Side), i_Pos, 0.1f));
        }

        private void ResetFrontLine()
        {
            FrontLinePosition = m_Side == HorizontalDir.LEFT
                ? float.NegativeInfinity
                : float.PositiveInfinity;
            FrontLineSoldier = null;
        }

        [UsedImplicitly]
        private void Update()
        {
            // Update front line info
            ResetFrontLine();
            foreach (var soldier in m_Soldiers)
            {
                var pos = soldier.transform.position.x;
                if ((m_Side == HorizontalDir.LEFT && pos > FrontLinePosition)
                    || (m_Side == HorizontalDir.RIGHT && pos < FrontLinePosition))
                {
                    FrontLinePosition = pos;
                    FrontLineSoldier = soldier;
                }
            }
        }

        #endregion
    }
}