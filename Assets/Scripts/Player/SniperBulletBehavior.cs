using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

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

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, this.transform.right, 25f, HitMask);

        //Debug.Log($"Firing sniper bullet at {this.transform.position} towards {this.transform.right}");
        if (hit.collider != null)
        {
            Debug.Log($"Sniper bullet hit {hit.collider.name} {hit.distance} units away");

            
            SpriteRend.size = new Vector2(hit.distance, .0625f);

            switch (hit.collider.gameObject.tag)
            {
                case "Enemy":

                    Enemy enemy = hit.collider.transform.GetComponent<Enemy>();
                    if(enemy != null) enemy.Health -= Damage + ExtraDamage;

                    break;

                case "Tile":
                    TileBehavior tile = hit.collider.transform.GetComponent<TileBehavior>();
                    if(tile != null) tile.Health -= Damage + ExtraDamage;



                    break;
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
