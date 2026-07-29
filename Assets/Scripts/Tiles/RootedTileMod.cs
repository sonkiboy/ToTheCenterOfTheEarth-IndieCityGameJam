using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class RootedTileMod : MonoBehaviour
{

    TileBehavior tile;
    GameObject rootOverlay;
    public GameObject rootPrefab;

    private float destroyDelayTime = 1.5f;

    public int TileHealthBonus = 20;
    private int originalTileHealth;

    private void Awake()
    {
        tile = GetComponent<TileBehavior>();
        originalTileHealth = tile.Health;


    }

    private void Start()
    {
        tile.Health = tile.Health + TileHealthBonus;
        tile.OnTileHealthChange += OnRootDamaged;
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        rootOverlay = Instantiate(rootPrefab, boxCollider.bounds.center,quaternion.identity);
        rootOverlay.transform.localScale = boxCollider.size;
        

    }

    void OnRootDamaged(object sender, EventArgs e)
    {
        if(tile.Health <= originalTileHealth)
        {
            // unsub the event so this only ever triggers once
            tile.OnTileHealthChange -= OnRootDamaged;

            // destory the overlay sprite
            Destroy(rootOverlay);

            // start the destroy sequence
            StartCoroutine(RootBreakDelay(destroyDelayTime));
        }
    }

    // destroys this component after the passed in delay
    IEnumerator RootBreakDelay(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this);
    }
}
