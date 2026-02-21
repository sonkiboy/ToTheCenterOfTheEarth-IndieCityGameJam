using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileBehavior : MonoBehaviour
{
    #region Obj and Components

    SpriteRenderer spriteRenderer;
    SpriteRenderer OverlayRenderer;

    [SerializeField] GameObject explosionFX;

    [SerializeField] GameObject gemDrop;
    [SerializeField] GameObject fuelDrop;

    #endregion


    public event EventHandler<EventArgs> OnTileBreak;

    public TileConfig Config;


    [SerializeField] int hp = 10;
    public int Health
    {
        get { return hp; }
        set
        {
            if (hp > 0)
            {
                hp = value;

                if (hp <= 0)
                {
                    BreakTile(true);
                }


                else if (hp <= (float)Config.Health * .3 && Config.BrokenOverlays.Length >= 1)
                {
                    if (spriteRenderer.sprite != Config.BrokenOverlays[0])
                    {
                        OverlayRenderer.sprite = Config.BrokenOverlays[0];
                    }

                }
                else if (hp <= (float)Config.Health * .75 && Config.BrokenOverlays.Length >= 2)
                {
                    if (spriteRenderer.sprite != Config.BrokenOverlays[1])
                    {
                        OverlayRenderer.sprite = Config.BrokenOverlays[1];
                    }
                }

            }

        }
    }


    // Start is called before the first frame update
    void Start()
    {
        hp = Config.Health;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(transform.Find("Overlay") != null)
        {
            OverlayRenderer = transform.Find("Overlay").GetComponent<SpriteRenderer>();

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BreakTile(bool isBrokenByPlayer)
    {
        StartCoroutine(BreakTileSequence(isBrokenByPlayer));
    }
    IEnumerator BreakTileSequence(bool isBrokenByPlayer)
    {
        // call the event 
        if(OnTileBreak != null)
        {
            OnTileBreak.Invoke(this,EventArgs.Empty);
        }

        // if there are break particles assigned, spawn them
        if (Config.breakParticles != null)
        {
            Instantiate(Config.breakParticles, this.transform.position + (Vector3.one * .5f), Quaternion.identity);
        }

        // these are events to happen if this this tile was only broken by the player
        if (isBrokenByPlayer)
        {
            // if the config of this tile dicates it explodes on destruction, call the Explode() method
            if (Config.ExplodeOnBreak)
            {
                Explode();
            }

            // if the config of the tile has gems to be dropped, then call DropObject(gemDrop) for each gem to be dropped
            if (Config.GemsDropped > 0)
            {
                for (int i = 0; i < Config.GemsDropped; i++)
                {
                    DropObject(gemDrop);
                }
            }

            // if the config of the tile has Fuel to be dropped, then call DropObject(fuelDrop) for each gem to be dropped
            if (Config.FuelDrop > 0)
            {
                for (int i = 0; i < Config.FuelDrop; i++)
                {
                    DropObject(fuelDrop);

                }
            }

            

            // if the conifg spawns a power up, pick a random power up from the config list
            if (Config.SpawnedPowerUps.Length > 0)
            {
                int random = Random.Range(0, Config.SpawnedPowerUps.Length);

                GameObject powerUp = Config.SpawnedPowerUps[random];

                Instantiate(powerUp, this.transform.position + (Vector3.one * .5f), powerUp.transform.rotation);

            }
        }

        // if the config has enemies to spawn, spawn all the enemies in the config list
        if (Config.SpawnedEnemies.Length > 0)
        {
            foreach (GameObject enemy in Config.SpawnedEnemies)
            {
                Instantiate(enemy, this.transform.position + (Vector3.one * .5f), enemy.transform.rotation);
            }
        }

        // play the break tile sound effect
        GameManager.Instance.SoundManager.PlaySoundOnObject(Config.BreakSound,this.gameObject);

        // buffer to make sure everything above happens before the tile object is destroyed
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    // called when a tile breaks and the config has ExplodeOnBreak turned On
    void Explode()
    {
        // Create a circle boundary at the position of the tile with demensions and collision based on the config
        Collider2D[] foundColliders = Physics2D.OverlapCircleAll(transform.position , Config.ExplosionRange,Config.LayersDected);

        // foreach collider found in the circle bounds, check if it is a Tile, Player, Enemy, or Pickup to interact with
        foreach (Collider2D collider in foundColliders)
        {

            if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "PickUp")
            {
                Rigidbody2D rb = collider.attachedRigidbody;

                if (rb != null)
                {
                    Vector2 direction = ((Vector2)transform.position - (Vector2)collider.transform.position);

                    Vector2 force = -direction.normalized * Config.BlastForce / direction.magnitude;

                    //Debug.Log($"Force read as {force} and Direction as {direction}");

                    rb.AddForce(force,ForceMode2D.Impulse);
                }
            }
            else if (collider.gameObject.tag == "Tile")
            {

                float distance = (this.transform.position - collider.transform.position).magnitude;
                int totalDamage = Mathf.RoundToInt((float)Config.ExplosionDamage * (Config.ExplosionRange - distance) / Config.ExplosionRange);

                collider.GetComponent<TileBehavior>().Health -= totalDamage;
            }
        }

        // create the explosion visual effect
        Instantiate(explosionFX, (Vector2)this.transform.position + (Vector2.one * .5f), explosionFX.transform.rotation);

    }

    void DropObject(GameObject drop)
    {
        Vector2 offset = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        float rotationOffset = Random.Range(0, 360);

        Instantiate(drop, transform.position + (Vector3)offset, Quaternion.Euler(Vector3.forward * rotationOffset));
    }
    
}
