using FXGuild.CrimFan.Common;
using JetBrains.Annotations;
using UnityEngine;

namespace FXGuild.CrimFan.Game.Pawn
{
    public sealed class NoteSoldier : MonoBehaviour
    {
        public enum State
        {
            FIRING,
            MOVING
        }

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

        public Army Army { get; private set; }
        public float Range { get; private set; }
        public State CurrentState { get; private set; }

        #endregion

        #region Static methods

        public static NoteSoldier CreateObject(float i_Position, float i_Range, Army i_Army)
        {
            var obj = Instantiate(PREFAB.gameObject);
            obj.transform.parent = i_Army.transform;
            var soldier = obj.GetComponent<NoteSoldier>();
            soldier.Army = i_Army;
            soldier.Range = i_Range;
            soldier.CurrentState = State.MOVING;

            // Set initial position
            soldier.transform.position = Vector3.right * i_Position;

            // Face good direction
            if (soldier.Army.Side == HorizontalDir.RIGHT)
            {
                var scale = soldier.transform.localScale;
                scale.x *= -1;
                soldier.transform.localScale = scale;
            }

            return soldier;
        }

        #endregion

        #region Methods

        [UsedImplicitly]
        private void Update()
        {
            var enemyArmy = Army.Team.Match.GetEnemyTeamOf(Army.Team).Army;
            
            if (enemyArmy.FrontLine.Exists && Mathf.Abs(enemyArmy.FrontLine.Position - transform.position.x) < Range)
            {
                CurrentState = State.FIRING;
            }
            else
            {
                if (Army.Side == HorizontalDir.RIGHT)
                {
                    transform.position += Vector3.left * 0.01f;
                }
                else
                {
                    transform.position += Vector3.right * 0.01f;
                }
            }
        }

        #endregion
    }
}