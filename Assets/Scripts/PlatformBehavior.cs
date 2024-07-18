using System.Collections;
using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    #region Object and Components
    public SpriteRenderer fuelBar;
    SpriteRenderer spriteRenderer;
    BoxCollider2D drillCollider;

    BlockGenerator blockGenerator;

    #endregion

    #region Sounds

    public AK.Wwise.Event AlarmOnSound;
    public AK.Wwise.Event AlarmOffSound;

    public AK.Wwise.Event DrillOnSound;
    public AK.Wwise.Event DrillOffSound;

    #endregion


    public float DecentSpeed = 2;
    public float DrillRate = 2;
    public int DrillDamage = 3;

    private int fuel = 0;
    public int CurrentFuel
    {
        get { return fuel; }
        set
        {
            

            fuel = value;

            if (fuel < 0)
            {
                fuel = 0;
            }

            if(fuel <= 0 && !isDraining)
            {
                fuel = 0;

                AlarmOnSound.Post(gameObject);
                DrillOnSound.Post(gameObject);

                StartCoroutine(DrainPlayer());
            }
            else if(fuel > 0 && isDraining)
            {
                //Debug.Log("fuel gained, stopping drain");

                AlarmOffSound.Post(gameObject);
                DrillOffSound.Post(gameObject);


                isDraining = false;
                StopCoroutine(DrainPlayer());
            }



            if(fuel > MaxFuel)
            {
                fuel = MaxFuel;
            }

            float width = ((float)fuel / (float)MaxFuel) * 2.5f;
            //Debug.Log($"Setting Fuel bar to {width} (Fuel: {fuel} | Max Fuel: {MaxFuel}");
            fuelBar.size = new Vector2(width, fuelBar.size.y);

        }
    }

    public int MaxFuel = 200;
    public float FuelPerSecond = 15f; 

    bool isDecending = false;
    bool isDraining = false;

    // Start is called before the first frame update
    void Start()
    {
        drillCollider = transform.Find("DrillCollider").GetComponent<BoxCollider2D>();
        blockGenerator = GameObject.Find("Cavern").GetComponent<BlockGenerator>();

        CurrentFuel = 100;

        DrillOnSound.Post(gameObject);

        StartCoroutine(Drill());
        StartCoroutine(ConsumeFuel());
    }

    private void OnDisable()
    {
        AlarmOffSound.Post(gameObject);
        AkSoundEngine.StopAll(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            collision.transform.parent = this.gameObject.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            if (gameObject.activeSelf)
            {
                collision.transform.parent = null;
            }
            
        }
    }

    IEnumerator Drill()
    {
        // just padding to help with errors
        yield return new WaitForSeconds(1f);

        while (true)
        {
            if (CurrentFuel > 0)
            {
                Collider2D[] foundCollisions = Physics2D.OverlapBoxAll(drillCollider.bounds.center, drillCollider.size, 0f, ~drillCollider.excludeLayers);

                if (foundCollisions.Length > 0)
                {
                    //Debug.Log("Digging");

                    foreach (Collider2D collision in foundCollisions)
                    {
                        if (collision.gameObject.tag == "Tile")
                        {
                            TileBehavior tileBehavior = collision.GetComponent<TileBehavior>();

                            tileBehavior.Health -= DrillDamage;
                        }
                        else if (collision.gameObject.tag == "Enemy")
                        {
                            IEnemy enemy = collision.gameObject.GetComponent<IEnemy>();

                            enemy.DealDamage(DrillDamage);
                        }
                    }
                }
                else if (foundCollisions.Length == 0)
                {
                    if (!isDecending)
                    {
                        Debug.Log("Decending");
                        StartCoroutine(Decend());
                    }
                }
            }

            yield return new WaitForSeconds(1/DrillRate);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Decend()
    {
        isDecending = true;

        for (int i = 0; i < 4; i++)
        {
            transform.position = (Vector2)transform.position + Vector2.down * .25f;

            yield return new WaitForSeconds(1/DecentSpeed);
            yield return new WaitForFixedUpdate();
        }

        GameManager.Instance.CurrentDepth++;
        blockGenerator.GenerateNextLevel();

        isDecending = false;
    }

    IEnumerator ConsumeFuel()
    {
        while (true)
        {
            CurrentFuel -= Mathf.RoundToInt(FuelPerSecond / 4);

            yield return new WaitForSeconds(.25f);
        }
    }

    IEnumerator DrainPlayer()
    {
    
        DrillOffSound.Post(gameObject);

        Debug.Log("Out of fuel drain started");

        isDraining = true;

        while (fuel <= 0)
        {
            yield return new WaitForSeconds(2f);

            GameManager.Instance.CurrentHealth -= 1;

            if(GameManager.Instance.CurrentHealth <= 0)
            {
                break;
            }
           
        }
    }
}
