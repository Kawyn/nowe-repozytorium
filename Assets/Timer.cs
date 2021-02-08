using UnityEngine;

public class Timer : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[4];
    static Vector2[] DIRECTIONS = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    public int time = 3;
    public int startTime;
    public bool[] outs = new bool[4];
    SpriteRenderer spriteRenderer;
    public LayerMask obstacles;
    private bool on;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Power(Arguments args)
    {
        on = true;
        time = startTime;
        spriteRenderer.sprite = sprites[3];
    }

    public void Refresh()
    {

       
        if (on && time == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                if (!outs[i])
                    continue;

                Collider2D hit = Physics2D.OverlapPoint(transform.position + (Vector3)DIRECTIONS[i] + new Vector3(0.5f, 0.5f), obstacles);

                if (hit)
                    hit.transform.gameObject.SendMessage("Power", new Arguments(DIRECTIONS[i], false, transform), SendMessageOptions.DontRequireReceiver);
            }
        } 
        if (!on)
            return;

        time--;

        if (time == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                if (!outs[i])
                    continue;

                Collider2D hit = Physics2D.OverlapPoint(transform.position + (Vector3)DIRECTIONS[i] + new Vector3(0.5f, 0.5f), obstacles);

                if (hit)
                    hit.transform.gameObject.SendMessage("Power", new Arguments(DIRECTIONS[i], false, false), SendMessageOptions.DontRequireReceiver);
            }
        }

        if (time >= 0)
            spriteRenderer.sprite = sprites[time];
    }
}
