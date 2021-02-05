﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plug : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[2];
    public List<Vector2Int> history = new List<Vector2Int>();
    public List<Wire> wires = new List<Wire>();

    public GameManager gameManager;
    public GameObject wirePrefab;

    public LayerMask obstacles;

    GameObject socket;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            Move(Vector2Int.up);
        else if (Input.GetKeyDown(KeyCode.A))
            Move(Vector2Int.left);
        else if (Input.GetKeyDown(KeyCode.S))
            Move(Vector2Int.down);
        else if (Input.GetKeyDown(KeyCode.D))
            Move(Vector2Int.right);
    }

    private void Move(Vector2Int direction)
    {
        if (socket)
        {
            int dx = (int)(socket.transform.position.x - transform.position.x);
            int dy = (int)(socket.transform.position.y - transform.position.y);


            // Są w te same - połącz
            if ((dx != 0 && direction.x == dx) || (dy != 0 && direction.y == dy)) 
                socket.SendMessage("Power", new Arguments(Vector2.zero, true, false));
            else if ((dx != 0 && direction.x == -dx) || (dy != 0 && direction.y == -dy))
                Undo();

            return;
        }

        if (history.Count != 0)
        {
            if (direction == -history[history.Count - 1])
            {
                Undo();
                return;
            }
        }

        if (gameManager.length == 0)
            return;
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;

        int nx = x + direction.x;
        int ny = y + direction.y;

        Collider2D hit = Physics2D.OverlapPoint(transform.position + (Vector3Int)direction, obstacles);

        if (hit)
        {
            // Ściana
            if (hit.transform.CompareTag("Wall"))
                return;

            // Kable
            else if (hit.transform.CompareTag("Wire"))
            {
                Wire owire = hit.transform.GetComponent<Wire>();

                if (direction.x != 0)
                {
                    if (owire.outs[1] || owire.outs[3])
                        return;
                }
                else if (direction.y != 0)
                {
                    if (owire.outs[0] || owire.outs[2])
                        return;
                }
            }

            // Gniazdka
            else if (hit.transform.CompareTag("Socket"))
            {
           
                return;
            }
        }



        // Kolejne sprawdzenia...
        Wire nwire = Instantiate(wirePrefab, transform.position, Quaternion.identity).GetComponent<Wire>();

        nwire.outs[Wire.DirectionToIndex(direction)] = true;

        if (history.Count != 0)
            nwire.outs[Wire.DirectionToIndex(-history[history.Count - 1])] = true;
        else
            nwire.outs[Wire.DirectionToIndex(  gameManager.enterance -gameManager.plugPosition)] = true;
        nwire.Refresh();

        transform.position = new Vector3(nx, ny, 0);

        // Kierunek wtyczki
        SetRotation(direction);

        wires.Add(nwire);
        history.Add(direction);

        gameManager.length--;
        gameManager.Refresh();


        // Podłączanie do sieci!
        hit = Physics2D.OverlapPoint(transform.position + (Vector3Int)direction, obstacles);

        if (hit)
        {
            if (hit.transform.CompareTag("Socket"))
            {
                socket = hit.transform.gameObject;
                socket.SendMessage("Power", new Arguments(Vector2.zero, false, false));
                spriteRenderer.sprite = sprites[1];

            }
        }
    }

    private void Undo()
    {
        if (history.Count <= 0)
            return;

        Vector2Int direction = history[history.Count - 1];

        int x = (int)transform.position.x;
        int y = (int)transform.position.y;

        int nx = x - direction.x;
        int ny = y - direction.y;

        transform.position = new Vector3(nx, ny, 0);


        if (history.Count >= 2)
            SetRotation(history[history.Count - 2]);
        else
            SetRotation(gameManager.plugPosition - gameManager.enterance);

        Destroy(wires[wires.Count - 1].gameObject);

        if (socket)
        {
            socket.SendMessage("Power", new Arguments(Vector2.zero, false, true));
            socket = null;
            spriteRenderer.sprite = sprites[0];
        }
        wires.RemoveAt(history.Count - 1);
        history.RemoveAt(history.Count - 1);
        gameManager.length++;
        gameManager.Refresh();

    }

    public void SetRotation(Vector2Int direction)
    {
        int rotation = 90;

        if (direction.x != 0)
        {
            if (direction.x == -1)
                rotation = 180;
            else if (direction.x == 1)
                rotation = 0;
        }
        else if (direction.y != 0)
        {
            if (direction.y == -1)
                rotation = 270;
        }
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}