using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PuzzleGhostBlock : Interactable
{
    Tilemap map;
    public Tile activeTile;
    public Tile inactiveTile;
    Collider2D coll;
    private void Awake()
    {
        base.Awake();
        map = GetComponent<Tilemap>();
        coll = GetComponent<Collider2D>();

    }

    public override void Interact()
    {
        map.SwapTile(activeTile, inactiveTile);
        coll.enabled = false;
    }

    public override void DeInteract()
    {
        map.SwapTile(inactiveTile, activeTile);
        coll.enabled = true;
    }
}
