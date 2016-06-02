using FXG.CrimFan.Common;
using JetBrains.Annotations;
using UnityEngine;

namespace FXG.CrimFan.Pawn
{
    public sealed class HealthBar : MonoBehaviour
    {
        #region Compile-time constants

        private const float MAX_HEIGHT = 0.2f;

        private const string PREFAB_NAME = "HealthBar";

        #endregion

        #region Runtime constants

        private static readonly Vector3 POS = new Vector3(0.1f, 0.1f, 0);

        private static readonly GameObject PREFAB;

        #endregion

        #region Constructors

        static HealthBar()
        {
            PREFAB = Utils.LoadPrefab(PREFAB_NAME);
        }

        #endregion

        #region Static methods

        public static HealthBar CreateObject(GameObject i_NoteSoldier)
        {
            var bar = Instantiate(PREFAB);
            bar.transform.parent = i_NoteSoldier.transform;
            bar.transform.position = i_NoteSoldier.transform.position + POS;
            return bar.GetComponent<HealthBar>();
        }

        #endregion

        #region Methods

        [UsedImplicitly]
        private void Update()
        {
            // Compute current pawn's health ratio
            var soldier = transform.parent.gameObject.GetComponent<NoteSoldier>();
            var healthRatio = soldier.Health / soldier.InitialHealth;

            // Update height
            var lr = GetComponent<LineRenderer>();
            lr.SetPosition(1, Vector3.up * MAX_HEIGHT * healthRatio);

            // Update color
            var color = new Color(1 - healthRatio, healthRatio, 0);
            lr.SetColors(color / 2, color);
        }

        #endregion
    }
}