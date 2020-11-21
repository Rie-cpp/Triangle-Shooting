using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector2 PlayerMoveLimit = new Vector2(8.4f, 4.3f);

    public static Vector3 ClampPosition(Vector3 position)
    {
        return new Vector3(Mathf.Clamp(position.x, -PlayerMoveLimit.x, PlayerMoveLimit.x), Mathf.Clamp(position.y, -PlayerMoveLimit.y, PlayerMoveLimit.y), 0);
    }
    
    
    public static float Aim(Vector2 from, Vector2 to)
    {
        var dx = to.x - from.x;
        var dy = to.y - from.y;
        var rad = Mathf.Atan2(dy, dx);
        return rad * Mathf.Rad2Deg;
    }

    // 指定された角度(°)をベクトルに変換
    public static Vector3 GetDirection(float angle)
    {
        return new Vector3
        (
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad),
            0
        );
    }
}
