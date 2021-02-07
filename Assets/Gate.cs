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
            powered = args.off;


        }
        if (open)
        {
            if (!horizontal)
            {
                // +X
                Collider2D hit = Physics2D.OverlapPoint(transform.position + Vector3.right + new Vector3(0.5f, 0.5f), obstacles);

                if (hit)
                    hit.transform.gameObject.SendMessage("Power", new Arguments(Vector3.right, false, powered), SendMessageOptions.DontRequireReceiver);

                // -X
                hit = Physics2D.OverlapPoint(transform.position + Vector3.left + new Vector3(0.5f, 0.5f), obstacles);

                if (hit)
                    hit.transform.gameObject.SendMessage("Power", new Arguments(Vector3.left, false, powered), SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                // +Y
                Collider2D hit = Physics2D.OverlapPoint(transform.position + Vector3.up + new Vector3(0.5f, 0.5f), obstacles);

                if (hit)
                    hit.transform.gameObject.SendMessage("Power", new Arguments(Vector3.up, false, powered), SendMessageOptions.DontRequireReceiver);

                // -Y
                hit = Physics2D.OverlapPoint(transform.position + Vector3.down + new Vector3(0.5f, 0.5f), obstacles);

                if (hit)
                    hit.transform.gameObject.SendMessage("Power", new Arguments(Vector3.down, false, powered), SendMessageOptions.DontRequireReceiver);
            }
        
        }
        if(!open)
        {
            if (!horizontal)
            {
                Collider2D hit = Physics2D.OverlapPoint(transform.position + Vector3.right + new Vector3(0.5f, 0.5f), obstacles);

                if (hit)
                    hit.transform.gameObject.SendMessage("Power", new Arguments(Vector3.right, false, powered), SendMessageOptions.DontRequireReceiver);

                hit = Physics2D.OverlapPoint(transform.position + Vector3.left + new Vector3(0.5f, 0.5f), obstacles);

                if (hit)
                    hit.transform.gameObject.SendMessage("Power", new Arguments(Vector3.left, false, powered), SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                // +Y
                Collider2D hit = Physics2D.OverlapPoint(transform.position + Vector3.up + new Vector3(0.5f, 0.5f), obstacles);

                if (hit)
                    hit.transform.gameObject.SendMessage("Power", new Arguments(Vector3.up, false, powered), SendMessageOptions.DontRequireReceiver);

                // -Y
                hit = Physics2D.OverlapPoint(transform.position + Vector3.down + new Vector3(0.5f, 0.5f), obstacles);

                if (hit)
                    hit.transform.gameObject.SendMessage("Power", new Arguments(Vector3.down, false, powered), SendMessageOptions.DontRequireReceiver);
            }
        }
        spriteRenderer.sprite = sprites[open ? 1 : 0];
    }
}
