using FXG.CrimFan.Core;
using FXG.CrimFan.World;

namespace FXG.CrimFan.UI.State
{
    public abstract class MenuState
    {
        #region Constructors

        protected MenuState(Team i_Team)
        {
            Team = i_Team;
        }

        #endregion

        #region Properties

        public Team Team { get; private set; }

        #endregion

        #region Methods

        public virtual void InitializeKey(Key i_Key, bool i_IsOnTerritory)
        {
        }

        public virtual void OnKeyboardKeyHit(Key i_Key, bool i_IsOnTerritory)
        {
        }

        public virtual void OnKeyboardKeyPressed(Key i_Key, bool i_IsOnTerritory)
        {
        }

        public virtual void OnKeyboardKeyReleased(Key i_Key, bool i_IsOnTerritory)
        {
        }

        #endregion
    }
}