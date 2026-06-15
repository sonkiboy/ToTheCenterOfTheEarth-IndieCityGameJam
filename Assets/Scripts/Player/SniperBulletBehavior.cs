using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SniperBulletBehavior : Bullet
{


    public LayerMask HitMask;
    SpriteRenderer SpriteRend;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpriteRend = GetComponent<SpriteRenderer>();
        Fire();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Fire()
    {
        RaycastHit2D[] rayHits = Physics2D.RaycastAll(this.transform.position, this.transform.right, 25f, HitMask);
        Collider2D[] hits = new Collider2D[rayHits.Length];

        for (int i = 0; i < hits.Length; i++)
        {
            hits[i] = rayHits[i].collider;
        }

        Collider2D[] renderedHits = new Collider2D[this.Gun.AdditionalPierceCount + this.Gun.Settings.PierceCount];

        for (int i = 0; i < renderedHits.Length; i++)
        {
            for (int k = 0; k < hits.Length; k++)
            {
                // check to see if hits[k] is not already in rendered hits
                bool check = false;

                for (int j = 0; renderedHits.Length > j; j++)
                {
                    if (renderedHits[j] == hits[k])
                    {
                        //Debug.Log($"Close collider {hits[k]} was already found in rendered array at position {j}");
                        check = true;
                        break;
                    }
                }

                if (check == false)
                {
                    if (renderedHits[i] == null)
                    {
                        //Debug.Log($"No collider in position {i}, setting to {hits[k]}");
                        renderedHits[i] = hits[k];
                        continue;
                    }
                    else
                    {
                        if (Vector2.Distance(this.transform.position, hits[k].transform.position) < Vector2.Distance(this.transform.position, renderedHits[i].transform.position))
                        {
                            //Debug.Log($"Closer collider {hits[k]} found for position {i}");

                            renderedHits[i] = hits[k];

                        }
                    }
                }
            }
            //Debug.Log($"Final collider found for position {i} was {renderedHits[i]}");

        }

        foreach (Collider2D hitCollider in renderedHits)
        {
            //Debug.Log($"Firing sniper bullet at {this.transform.position} towards {this.transform.right}");
            if (hitCollider != null)
            {
                //Debug.Log($"Sniper bullet hit {hitCollider.name} {Vector2.Distance(this.transform.position, hitCollider.transform.position)} units away");




                switch (hitCollider.gameObject.tag)
                {
                    case "Enemy":

                        Enemy enemy = hitCollider.transform.GetComponent<Enemy>();
                        if (enemy != null) enemy.Health -= Damage + ExtraDamage;

                        break;

                    case "Tile":
                        TileBehavior tile = hitCollider.transform.GetComponent<TileBehavior>();
                        if (tile != null) tile.Health -= Damage + ExtraDamage;



                        break;
                }
            }

        }

        if (renderedHits[0] != null)
        {
            for (int i = renderedHits.Length - 1; i >= 0; i--)
            {
                if (renderedHits[i] != null)
                {
                    Debug.Log($"distance calculated as {Vector2.Distance(this.transform.position, renderedHits[i].transform.position)}");
                    SpriteRend.size = new Vector2(Vector2.Distance(this.transform.position, renderedHits[i].transform.position), .0625f);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("Sniper Bullet missed");
            SpriteRend.size = new Vector2 (25f, .0625f);
        }




        StartCoroutine(Lifetime());
    }


}
