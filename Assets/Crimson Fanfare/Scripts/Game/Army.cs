using FXGuild.CrimFan.Common;
using FXGuild.CrimFan.Game.Pawn;
using System.Collections.Generic;

namespace FXGuild.CrimFan.Game
{
    public sealed class Army
    {
        private readonly List<NoteSoldier> m_Soldiers;

        public float FrontLinePosition { get; private set; }

        public NoteSoldier FrontLineSoldier { get; private set; }

        private HorizontalDir m_Side;

        #region Constructors

        public Army(HorizontalDir i_Side)
        {
            m_Soldiers = new List<NoteSoldier>();
            m_Side = i_Side;

            ResetFrontLine();
        }

        private void ResetFrontLine()
        {
            FrontLinePosition = m_Side == HorizontalDir.LEFT ? float.NegativeInfinity : float.PositiveInfinity;
            FrontLineSoldier = null;
        }

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

        public void AddSoldier(float i_Pos)
        {
            m_Soldiers.Add(NoteSoldier.Create(Utils.OppositeDir(m_Side), i_Pos, 0.1f));
        }

        #region Properties

        #endregion
    }
}