using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RootedTileMod : MonoBehaviour
{

    TileBehavior tile;
    GameObject rootOverlay;
    Animator animator;
    public GameObject rootPrefab;

    private float destroyDelayTime = 1.5f;

    public int TileHealthBonus = 20;
    private int originalTileHealth;

    private bool isIgnited = false;
    public float BurnTime = .5f;
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
        animator = rootOverlay.GetComponent<Animator>();

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

    public void IgniteRoot()
    {
        if(isIgnited == false)
        {
            isIgnited = true;

            // start the coroutine
            StartCoroutine(SpreadFire());
        }
    }

    IEnumerator SpreadFire()
    {
        if (animator != null) animator.SetTrigger("Ignite");
        yield return new WaitForSeconds(BurnTime / 2f);

        List<RootedTileMod> foundRoots = new List<RootedTileMod>();
        Collider2D foundTile;
        RootedTileMod foundMod;
        foreach(Vector2 checkDir in SonkUtilities.FourDirections)
        {

            foundTile = Physics2D.OverlapPoint((Vector2)this.transform.position + (((Vector2.one / 2) + checkDir)*tile.GetComponent<BoxCollider2D>().size), LayerMask.GetMask("Tiles"));
            if(foundTile != null)
            {
                foundMod = foundTile.GetComponent<RootedTileMod>();
                if (foundMod != null) foundRoots.Add(foundMod);
            }
            
        }


        foreach(RootedTileMod root in foundRoots)
        {
            if(root != null) root.IgniteRoot();
        }

        yield return new WaitForSeconds(BurnTime / 2f);


        tile.Health = originalTileHealth;   


    }
}
