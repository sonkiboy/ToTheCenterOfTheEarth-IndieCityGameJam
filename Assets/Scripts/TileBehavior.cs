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
            hp = value;

            if (hp <= 0)
            {
                BreakTile();
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


    public void BreakTile()
    {


        Destroy(gameObject);
    }


    
}
