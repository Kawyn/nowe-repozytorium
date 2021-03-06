﻿using UnityEngine;

public class Battery : MonoBehaviour
{
    static Vector2[] DIRECTIONS = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    public bool[] outs = new bool[4] { true, true, true, true };
    public LayerMask obstacles;
    public void Refresh()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!outs[i])
                continue;

            Collider2D hit = Physics2D.OverlapPoint(transform.position + (Vector3)DIRECTIONS[i] + new Vector3(0.5f, 0.5f), obstacles);
            if (hit)
                hit.gameObject.SendMessage("Power", new Arguments(DIRECTIONS[i], false, false), SendMessageOptions.DontRequireReceiver);
        }
    }
}
