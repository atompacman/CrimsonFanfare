using UnityEngine;

// ReSharper disable ConvertPropertyToExpressionBody

namespace FXG.CrimFan.World.Buildings
{
    public abstract class Building : MonoBehaviour
    {
        #region Properties

        public Key Key { get; set; }

        #endregion

        #region Methods

        public virtual bool CanSpawnUnits()
        {
            return false;
        }

        #endregion
    }
}