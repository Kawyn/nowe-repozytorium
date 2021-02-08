using UnityEngine;

public class Wifi : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[2];
    static Vector2[] DIRECTIONS = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    public bool[] outs = new bool[4] { true, true, true, true };
    private GameObject[] hotspots;
    public LayerMask obstacles;
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
        hotspots = GameObject.FindGameObjectsWithTag("Hotspot");
    }

    public void Power(Arguments args)
    {
        if (args.source != Vector2.zero)
        {
            foreach (GameObject h in hotspots)
            {
                h.SendMessage("Power", new Arguments(Vector2.zero, args.control, args.off), SendMessageOptions.DontRequireReceiver);
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (!outs[i])
                continue;

            Collider2D hit = Physics2D.OverlapPoint(transform.position + (Vector3)DIRECTIONS[i] + new Vector3(0.5f, 0.5f), obstacles);
            if (hit)
                hit.gameObject.SendMessage("Power", new Arguments(DIRECTIONS[i], args.control, args.off), SendMessageOptions.DontRequireReceiver);
        }

        spriteRenderer.sprite = sprites[args.off ? 0 : 1];
    }

}
