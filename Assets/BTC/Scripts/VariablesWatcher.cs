using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BTC
{
    public sealed class VariablesWatcher
    {
        #region Private fields

        private readonly object[] m_PrevValues;

        private readonly object m_Watched;
        private readonly List<FieldInfo> m_WatchedFields;

        #endregion

        #region Constructors

        public VariablesWatcher(object i_Watched)
        {
            m_Watched = i_Watched;
            m_WatchedFields = new List<FieldInfo>();

            foreach (var member in m_Watched.GetType().GetMembers()
                .Where(i_Member => i_Member is FieldInfo &&
                                   i_Member.GetCustomAttributes(typeof(WatchedAttribute), false)
                                       .Length == 1))
            {
                m_WatchedFields.Add(member as FieldInfo);
            }

            m_PrevValues = new object[m_WatchedFields.Count];
        }

        #endregion

        #region Methods

        public bool HasAnyVariableChanged()
        {
            var hasChanged = false;

            for (var i = 0; i < m_WatchedFields.Count; ++i)
            {
                var currVal = m_WatchedFields[i].GetValue(m_Watched);
                if (currVal == null || m_PrevValues[i] == null)
                {
                    hasChanged |= currVal != m_PrevValues[i];
                }
                else
                {
                    hasChanged |= !currVal.Equals(m_PrevValues[i]);
                }
                if (currVal is ICloneable)
                {
                    m_PrevValues[i] = (currVal as ICloneable).Clone();
                }
                else
                {
                    m_PrevValues[i] = currVal;
                }
            }

            return hasChanged;
        }

        #endregion
    }

    public sealed class WatchedAttribute : Attribute
    {
    }
}