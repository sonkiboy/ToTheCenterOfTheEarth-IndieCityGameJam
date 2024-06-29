using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    #region Obj and Components

    SpriteRenderer spriteRenderer;


    #endregion


    public TileConfig Config;


    int hp = 10;
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


            else if (hp <= (float)Config.Health * .3 && Config.BrokenOverlays[0] != null)
            {
                if (spriteRenderer.sprite != Config.BrokenOverlays[0])
                {
                    spriteRenderer.sprite = Config.BrokenOverlays[0];
                }
                
            }
            else if (hp <= (float)Config.Health * .75 && Config.BrokenOverlays[1] != null)
            {
                if (spriteRenderer.sprite != Config.BrokenOverlays[1])
                {
                    spriteRenderer.sprite = Config.BrokenOverlays[1];
                }
            }



        }
    }


    // Start is called before the first frame update
    void Start()
    {
        hp = Config.Health;
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
