using FXGuild.CrimFan.Common;
using JetBrains.Annotations;
using UnityEngine;

namespace FXGuild.CrimFan.Game.Pawn
{
    public sealed class NoteSoldier : MonoBehaviour
    {
        #region Runtime constants

        private static readonly GameObject PREFAB;

        #endregion

        #region Constructors

        static NoteSoldier()
        {
            PREFAB = Resources.Load<GameObject>("Prefabs/NoteSoldier");
        }

        #endregion

        #region Properties

        public HorizontalDir Direction { get; private set; }
        public float Range { get; private set; }

        #endregion

        #region Static methods

        public static NoteSoldier Create(HorizontalDir i_Direction, float i_Position, float i_Range)
        {
            var obj = Instantiate(PREFAB.gameObject);
            var soldier = obj.GetComponent<NoteSoldier>();
            soldier.Direction = i_Direction;
            soldier.Range = i_Range;
            soldier.transform.position = Vector3.right * i_Position;
            return soldier;
        }

        #endregion

        #region Methods

        [UsedImplicitly]
        private void Update()
        {
            if (Direction == HorizontalDir.LEFT)
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