using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerTileMod : MonoBehaviour
{

    public float TileInfectionTime = 3f;
    public int TileCheckRate = 6;
    public float InfectionRadius = 4;
    public GameObject RootPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(InfectionRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator InfectionRoutine()
    {
        Collider2D[] foundColliders;
        List<GameObject> foundTiles;
        GameObject targetTile;
        RootedTileMod foundRootMod;


        // start a loop so this routine lasts as long as the component is alive
        while (true)
        {
            targetTile = null;
            foundRootMod = null;
            foundTiles = new List<GameObject>();

            // get all tiles in the surrounding area and process the into the foundTiles 
            foundColliders = Physics2D.OverlapCircleAll(this.transform.position, InfectionRadius, LayerMask.GetMask("Tiles"));
            foreach (Collider2D collider in foundColliders)
            {
                foundTiles.Add(collider.gameObject);
            }

            if (foundTiles.Contains(this.gameObject))
            {
                foundTiles.Remove(this.gameObject);
            }

            // evaluaition while loop that continues until we find find a valid target within the found colliders
            while (true)
            {
                // loop through and find the closest tile to this flower tile
                foreach (GameObject tile in foundTiles)
                {
                    if (targetTile == null)
                    {
                        targetTile = tile;
                    }
                    else
                    {
                        if (tile != null)
                        {
                            if (Vector3.Distance(this.transform.position, tile.transform.position) < Vector3.Distance(this.transform.position, targetTile.transform.position))
                            {
                                targetTile = tile;
                            }
                        }
                    }

                }

                if(targetTile != null)
                {
                    // see if the closest object already has a rooted component
                    foundRootMod = targetTile.GetComponent<RootedTileMod>();

                    // if it does, remove it from the list and loop again to find the next closest object
                    if (foundRootMod != null)
                    {
                        foundTiles.Remove(targetTile);
                        targetTile = null;
                    }
                    // if there is no rooted component, then this becomes the target tile
                    else
                    {
                        //Debug.Log($"Target tile confirmed as {targetTile.name}");
                        break;
                    }
                    yield return null;
                }

                yield return null;
            }

            // wait for the buffer time and check every number of times to see if the tile either was destroyed, or got a rooted componend on it
            for (int i = 0; i < TileCheckRate; i++)
            {
                if (targetTile == null || targetTile.GetComponent<RootedTileMod>() != null)
                {
                    //Debug.Log($"Root Check aborted");

                    break;
                }
                yield return new WaitForSeconds(TileInfectionTime / (float)TileCheckRate);
            }

            // after the time, check one more time if the tile is there and has no rooted. If true then add rooted component and loop back to start of routine
            if (targetTile != null && targetTile.GetComponent<RootedTileMod>() == null)
            {
                RootedTileMod mod = targetTile.AddComponent<RootedTileMod>();
                mod.rootPrefab = RootPrefab;
                //Debug.Log($"Root successful from {targetTile.name}");

            }
            yield return null;

        }

    }
}
