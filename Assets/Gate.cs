using UnityEngine;

public class Gate : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[2];

    public bool horizontal = true;
    public bool open = false;
    public bool powered = false;  
    public LayerMask obstacles;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    public void Power(Arguments args)
    {

        if (open)
        {
            if (!horizontal && args.source.y == 0)
            {
                // +X
                Collider2D hit = Physics2D.OverlapPoint(transform.position + Vector3.right + new Vector3(0.5f, 0.5f), obstacles);

                if (hit)
                    hit.transform.gameObject.SendMessage("Power", new Arguments(Vector3.right, false, args.off), SendMessageOptions.DontRequireReceiver);
                
                // -X
                hit = Physics2D.OverlapPoint(transform.position + Vector3.left + new Vector3(0.5f, 0.5f), obstacles);

                if (hit)
                    hit.transform.gameObject.SendMessage("Power", new Arguments(Vector3.left, false, args.off), SendMessageOptions.DontRequireReceiver);
            }
            else if(horizontal && args.source.x == 0)
            {
                // +Y
                Collider2D hit = Physics2D.OverlapPoint(transform.position + Vector3.up + new Vector3(0.5f, 0.5f), obstacles);

                if (hit)
                    hit.transform.gameObject.SendMessage("Power", new Arguments(Vector3.up, false, args.off), SendMessageOptions.DontRequireReceiver);

                // -Y
                hit = Physics2D.OverlapPoint(transform.position + Vector3.down + new Vector3(0.5f, 0.5f), obstacles);

                if (hit)
                    hit.transform.gameObject.SendMessage("Power", new Arguments(Vector3.down, false, args.off), SendMessageOptions.DontRequireReceiver);
            }
        
        }
        if (args.source.x != 0 && horizontal)
        {
            open = !args.off;
        }
        else if (args.source.y != 0 && !horizontal)
        {
            open = !args.off;
        }
        else
        {
            powered = !args.off;
        }

        spriteRenderer.sprite = sprites[open ? 0 : 1];
    }
}
