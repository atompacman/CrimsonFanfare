using BTC;
using JetBrains.Annotations;
using UnityEngine;

public sealed class NoteSoldier : MonoBehaviour
{
    private static readonly GameObject PREFAB;

    public Direction Direction { get; private set; }

    #region Methods

    static NoteSoldier()
    {
        PREFAB = Resources.Load<GameObject>("Prefabs/NoteSoldier");
    }

    public static NoteSoldier Create(Direction i_Direction, float i_Position)
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
        if (Direction == Direction.LEFT)
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