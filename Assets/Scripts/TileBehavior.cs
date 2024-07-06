using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    #region Obj and Components

    SpriteRenderer spriteRenderer;
    SpriteRenderer OverlayRenderer;


    #endregion


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
                    StartCoroutine(BreakTile());
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


    public IEnumerator BreakTile()
    {
        if (Config.ExplodeOnBreak)
        {
            Explode();
        }

        if (Config.TreasureDrop > 0)
        {
            GameManager.Instance.CurrentTreasure += Config.TreasureDrop;
        }

        yield return new WaitForEndOfFrame();

        Destroy(gameObject);
    }

    void Explode()
    {
        Debug.Log("Exploding");

        Collider2D[] foundColliders = Physics2D.OverlapCircleAll(transform.position , Config.ExplosionRange,Config.LayersDected);

        foreach (Collider2D collider in foundColliders)
        {

            if (collider.gameObject.tag == "Player")
            {
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    Vector2 direction = ((Vector2)transform.position - (Vector2)collider.transform.position);

                    Vector2 force = -direction.normalized * Config.BlastForce / direction.magnitude;

                    Debug.Log($"Force read as {force} and Direction as {direction}");

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
    }


    
}
