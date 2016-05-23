using FXGuild.CrimFan.Common;
using JetBrains.Annotations;
using UnityEngine;

namespace FXGuild.CrimFan.Game.Pawn
{
    public sealed class NoteSoldier : MonoBehaviour
    {
        private static readonly GameObject PREFAB;

        public HorizontalDir Direction { get; private set; }

        #region Methods

        static NoteSoldier()
        {
            PREFAB = Resources.Load<GameObject>("Prefabs/NoteSoldier");
        }

        public static NoteSoldier Create(HorizontalDir i_Direction, float i_Position)
        {
            var obj = Instantiate(PREFAB.gameObject);
            var soldier = obj.GetComponent<NoteSoldier>();
            soldier.Direction = i_Direction;
            soldier.transform.position = Vector3.right * i_Position;
            return soldier;
        }
    
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