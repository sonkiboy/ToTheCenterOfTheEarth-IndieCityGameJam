using System.Collections;
using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    #region Object and Components

    PlayerController playerController;

    public SpriteRenderer fuelBar;
    SpriteRenderer spriteRenderer;
    BoxCollider2D drillCollider;

    BlockGenerator blockGenerator;

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
            // save the new value 
            fuel = value;

            // if the new fuel level is less than 0, set it to 0, we dont want a negative value
            if (fuel < 0)
            {
                fuel = 0;
            }

            // START DRAINING HEALTH?
            // if the platform is out of fuel and isn't already draining the health of the player, start draining their health
            if (fuel <= 0 && !isDrainingHealth)
            {
                // when the draining starts, turn on the alarm and turn off the drilling sound

                if(GameManager.Instance.CurrentState == GameManager.GameStates.RegularGame)
                {
                    GameManager.Instance.SoundManager.PlayNonDiageticSound("AlarmOn");
                    GameManager.Instance.SoundManager.PlayNonDiageticSound("PlatformStop");

                    StartCoroutine(DrainPlayer());
                }
                
            }

            // STOP DRAINING HEALTH?
            // else if the fuel level is above 0 and the platform is currently trying to drain the health of the player, stop draining
            else if (fuel > 0 && isDrainingHealth)
            {
                // when the draining stops, turn off the alarm sound and turn back on the drilling
                GameManager.Instance.SoundManager.PlayNonDiageticSound("AlarmOff");
                GameManager.Instance.SoundManager.PlayNonDiageticSound("PlatformStart");


                isDrainingHealth = false;
                StopCoroutine(DrainPlayer());
            }


            // if the fuel exceeds the max value it can be, set it to the max instead
            if (fuel > MaxFuel)
            {
                fuel = MaxFuel;
            }

            // calculate the size of the Fuel Bar graphic on the Platform
            float width = ((float)fuel / (float)MaxFuel) * 2.5f;
            if(fuelBar != null)
            {
                fuelBar.size = new Vector2(width, fuelBar.size.y);

            }

        }
    }

    // max fuel the platform can carry
    public int MaxFuel = 200;
    public float FuelPerSecond = 15f;

    // tracks if the platfrom is currently in the process of decending to the next row
    bool isDecending = false;

    // time it takes for the platform to drain one health from the player when out of fuel
    public float HealthDrainRate = 2f;

    // tracks if the platform is currently out of fuel and draining health from the player
    bool isDrainingHealth = false;

    // Start is called before the first frame update
    void Start()
    {
        // get the Drill Collider which will define the area to deal damage too
        drillCollider = transform.Find("DrillCollider").GetComponent<BoxCollider2D>();

        // save a refrence to the Level Generation component
        blockGenerator = GameObject.Find("Cavern").GetComponent<BlockGenerator>();

        // save a refrence of the Player Controller
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // Set the Current Fuel to its Maximum
        CurrentFuel = MaxFuel;

        

        // subscribe the OnGameOver method to the Game State Changed event to see when Game Over happens
        GameManager.Instance.StateChanged += OnGameOver;

        
    }

    public void StartDrilling()
    {
        // Start the Drilling Sound
        GameManager.Instance.SoundManager.PlayNonDiageticSound("PlatformStart");

        // Start the routines for Drilling and Consuming Fuel
        StartCoroutine(Drill());
        StartCoroutine(ConsumeFuel());
    }

    private void OnDisable()
    {
        // stop all sounds tied to this game object
        GameManager.Instance.SoundManager.PlayNonDiageticSound("AlarmOff");
    }

    private void OnDestroy()
    {
        // stop all sounds tied to this game object
        GameManager.Instance.SoundManager.PlayNonDiageticSound("AlarmOff");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if the player or an enemy lands on the platform, make this platform obj its parent so it will move with the platform
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            collision.transform.parent = this.gameObject.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // if the player or an emeny leaves the platform, make sure its parent object is set to nothing
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            if (gameObject.activeSelf)
            {
                collision.transform.parent = null;
            }

        }
    }

    // Deals damage to Blocks and Enemies within the Drill Collider Bounds at a consistent rate
    // Called once at Start() and loops indefinitley 
    IEnumerator Drill()
    {
        // just padding to help with errors
        yield return new WaitForSeconds(1f);

        while (GameManager.Instance.CurrentState != GameManager.GameStates.GameOver)
        {
            // as long as there is fuel in the tank, we can drill and deal damage
            if (CurrentFuel > 0)
            {
                // look for any objects within the drill collider that match it's layer mask
                Collider2D[] foundCollisions = Physics2D.OverlapBoxAll(drillCollider.bounds.center, drillCollider.size, 0f, ~drillCollider.excludeLayers);

                // if there were any colliders found in the drill bounds...
                if (foundCollisions.Length > 0)
                {
                    // for each collider found within the drill collider...
                    foreach (Collider2D collision in foundCollisions)
                    {
                        // if the collider was a Tile, deal damage to its Tile Behavior component
                        if (collision.gameObject.tag == "Tile")
                        {
                            TileBehavior tileBehavior = collision.GetComponent<TileBehavior>();
                            tileBehavior.Health -= DrillDamage;
                        }

                        // if the collider was instead an Enemy, deal damage to its Enemy component
                        else if (collision.gameObject.tag == "Enemy")
                        {
                            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                            enemy.Health -= (DrillDamage);
                        }
                    }
                }

                // if there were no colliders found in the drill bounds, then the platform is clear to start decending
                else if (foundCollisions.Length == 0 && !isDecending)
                {
                    StartCoroutine(Decend());
                }
            }

            // buffer time
            yield return new WaitForSeconds(1 / DrillRate);
            yield return new WaitForFixedUpdate();
        }
    }

    // Routine will lower the platform down one layer in incriments by 1/4 unit every 1/Decent Speed Seconds
    IEnumerator Decend()
    {
        // check to make sure this routine wasn't accidentally started while the platform is already decending
        if (isDecending == false)
        {
            // set is Decending to true
            isDecending = true;

            // decend the platform down 1/4 of a unit four times, with 1/decent speed seconds in between each incriment
            for (int i = 0; i < 4; i++)
            {
                transform.position = (Vector2)transform.position + Vector2.down * .25f;

                yield return new WaitForSeconds(1 / DecentSpeed);
                yield return new WaitForFixedUpdate();
            }



            // TO DO: REPLACE THIS WITH AN EVENT INSTEAD
            if (GameManager.Instance.CurrentState == GameManager.GameStates.RegularGame)
            {
                GameManager.Instance.CurrentDepth++;
            }

            GameManager.Instance.ActuallDepth++;
            blockGenerator.GenerateNextLevel();

            // at the end of the routine, set is Decending back to false
            isDecending = false;
        }
    }


    // will drain fuel from the platform based on Fuel Per Second (Drains 1/4 the value every 1/4 seconds)
    IEnumerator ConsumeFuel()
    {
        while (true)
        {
            CurrentFuel -= Mathf.RoundToInt(FuelPerSecond / 4);
            yield return new WaitForSeconds(.25f);
        }
    }

    // drains health from the player while the platform has no fuel until either the platform refuels, or the player dies
    IEnumerator DrainPlayer()
    {
        // 
        GameManager.Instance.SoundManager.PlayNonDiageticSound("AlarmOn");

        // turn off the drill sound
        GameManager.Instance.SoundManager.PlayNonDiageticSound("PlatformStop");

        // set is Draining Health to true
        isDrainingHealth = true;

        // wait HealthDrainRate seconds to give the player time to react 
        yield return new WaitForSeconds(HealthDrainRate);

        // while there is no fuel in the tank
        while (CurrentFuel <= 0)
        {
            // subtract one unit of health from the player
            GameManager.Instance.CurrentHealth -= 1;

            // give the player invincibility?
            // TODO: do this better, id rather this get called in the Current Health get set
            

            // wait HealthDrainRate seconds, then loop
            yield return new WaitForSeconds(HealthDrainRate);
        }
    }

    public void OnGameOver(object sender, GameManager.GameStates e)
    {
        if (e == GameManager.GameStates.GameOver)
        {
            
            StopAllCoroutines();
            StopCoroutine(Decend());
            
        }
    }
}
