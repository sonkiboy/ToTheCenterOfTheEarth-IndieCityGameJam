using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    #region Obj and Components

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;

    Renderer damageFlash;

    GameObject gunObj;
    SpriteRenderer gunSprite;


    #endregion



    #region Sounds

    public AK.Wwise.Event JetPackOn;
    public AK.Wwise.Event JetPackOff;


    #endregion

    // ------------- MOVEMENT -------------

    // Acceleration rate of side to side movement 
    public float Acceleration = 10;

    // Max Velocity the player can reach
    [SerializeField] float MaxVelocity = 10f;


    // ------------- JETPACK -------------

    // Thrust force for the Jetpack
    public float JetThrust = 40;

    // tracks if the jetpack is currently engaged
    bool IsJeckpack = false;


    // ------------- INVINCIBILITY -------------
    // (DAMAGE FROM PLATFORM BY PASSES INVINCIBILITY)

    // Number of seconds the player is invincible after getting hit
    public float InvincibilityDurration = 2f;

    // tracks if the player is invincible and can take damage this frame
    private bool isInvincible = false;


    // ------------- INPUTS -------------

    // Thurst Power, Move Direction, and Aim Direction are all used to keep track of the read input values for their respective action
    private float ThrustPower = 0f; // while we could read this as a bool button press, reading as a value give us the option to have a range when reading pressure sensitivity
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 aimDirection = Vector2.zero;

    // ------------- PLAYER STATE -------------

    public enum PlayerState
    {
        Default,
        Dead,
        NoJet,
        NoMove
    }

    private PlayerState _state;
    public PlayerState CurrentState
    {
        get { return _state; }
        set 
        { 
            _state = value;

            switch (_state)
            {
                case PlayerState.Dead:

                    // make sure the sprite is the flashing damage color
                    damageFlash.material.SetFloat("_Intensity", 1f);

                    // set the RB to no constraints so the player can roll around on death
                    rb.constraints = RigidbodyConstraints2D.None;

                    // set the animator to be dead
                    animator.SetBool("IsDead", true);

                    break;
                case PlayerState.NoJet:

                    // make sure the sprite isn't flashing damage (Because of Dead Player State)
                    damageFlash.material.SetFloat("_Intensity", 0f);

                    // freeze the rotation constraints of the RB and make the player rotation back to 0
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    transform.rotation = Quaternion.identity;

                    // make sure the jetpack animation turns off 
                    animator.SetBool("IsJetpacking", false);
                    // set the animator to not be dead
                    animator.SetBool("IsDead", false);

                    break;

                case PlayerState.NoMove:

                    // make sure the sprite isn't flashing damage (Because of Dead Player State)
                    damageFlash.material.SetFloat("_Intensity", 0f);

                    // freeze the rotation constraints of the RB and make the player rotation back to 0
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    transform.rotation = Quaternion.identity;

                    // make sure the jetpack animation turns off 
                    animator.SetBool("IsJetpacking", false);
                    // set the animator to not be dead
                    animator.SetBool("IsDead", false);

                    break;

                default:

                    // make sure the sprite isn't flashing damage (Because of Dead Player State)
                    damageFlash.material.SetFloat("_Intensity", 0f);
                    // set the animator to not be dead
                    animator.SetBool("IsDead", false);

                    break;
            }
        }
    }

    private void Awake()
    {
        // Find the Gun and save it, and the sprite (Used in aiming)
        gunObj = transform.Find("Gun").gameObject;
        gunSprite = gunObj.GetComponent<SpriteRenderer>();

        // Get the player material renderer and sprite renderer
        damageFlash = transform.Find("Sprite").GetComponent<Renderer>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        animator = spriteRenderer.gameObject.GetComponent<Animator>();

        // Get the Rigidbody
        rb = GetComponent<Rigidbody2D>();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // read the current move direction this frame
        moveDirection = GameManager.Instance.InputManager.MoveInput.ReadValue<Vector2>();

        // if the player is using a controller or other sort of joystick, we can use the thumb stick to get a direction input directly
        if (GameManager.Instance.InputManager.isController)
        {
            aimDirection = GameManager.Instance.InputManager.LookInput.ReadValue<Vector2>();
        }

        // if the player is on keyboard and mouse, we must do further calculation to get the aim direciton
        else if (GameManager.Instance.InputManager.isKeyboard)
        {
            // first get the current position of the mouse on the screen as a 2d vector
            aimDirection = GameManager.Instance.InputManager.MouseLookInput.ReadValue<Vector2>();

            // then figure out where that screen position would be in the game, and then calculate the direction in comparison from the player, and normalize
            aimDirection = ((Vector2)this.transform.position - (Vector2)Camera.main.ScreenToWorldPoint(aimDirection)).normalized;

            // Invert the aim direciton
            aimDirection = -aimDirection;
        }

        // if the player is aiming to the left of the character, flip the gun and player sprite if they aren't already facing left 
        if (aimDirection.x < 0 && spriteRenderer.flipX == true)
        {
            spriteRenderer.flipX = false;
            gunSprite.flipY = false;
        }

        // if the player is aiming to the right, flip the gun and player sprite if they aren't already facing right
        else if (aimDirection.x > 0 && spriteRenderer.flipX == false)
        {
            spriteRenderer.flipX = true;
            gunSprite.flipY = true;
        }

        // read the Jetpack Thrust input this frame
        ThrustPower = GameManager.Instance.InputManager.JetInput.ReadValue<float>();

    }

    private void FixedUpdate()
    {
        // if the absolute value of the players horizontal velocity is less than the Max Velocity...
        if (Mathf.Abs(rb.linearVelocity.x) < MaxVelocity)
        {
            // if the player is trying to move horizontally and they are in a Player Mode that can move...
            if (moveDirection.x != 0 && CurrentState != PlayerState.Dead && CurrentState != PlayerState.NoMove)
            {
                // calculate the force added to the players horizontal motion based on the X Axis of the Move Input
                Vector2 newForce = new Vector2(moveDirection.x * Acceleration, 0f);

                // add that new force to the Player Rigidbody
                rb.AddForce(newForce);

                // set the animator speed value to the horizontal velocity of the player
                animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocityX));
            }

            // We dont need to do much if the player isn't trying to input a move command, but we do need to set the animator speed to 0 to trigger idle animation
            else
            {
                animator.SetFloat("Speed", 0);
            }

        }

        // if the player input is pressed half way or more and they are in a player stat that allows jetting...
        if (ThrustPower > .5f && CurrentState != PlayerState.NoJet && CurrentState != PlayerState.Dead)
        {
            // add an upwards force to the Player Rigidbody based on the Jet Thrust 
            rb.AddForce(Vector2.up * JetThrust);

            // set to Is Jetpacking if not already set
            if (!IsJeckpack)
            {
                // start the Start Jetpack Sound event when starting to jetpack
                GameManager.Instance.SoundManager.PlaySoundOnObject("JetOn", this.gameObject);

                // set is jetpacking to true
                IsJeckpack = true;

                // tell the animator to play the jet animation
                animator.SetBool("IsJetting", true);
            }

        }
        // if the player input isn't trying to trigger and we are still set to Is Jetpacking Set Is Jetpacking to false and turn off the Jetpack sound
        else if (IsJeckpack)
        {
            IsJeckpack = false;

            GameManager.Instance.SoundManager.PlaySoundOnObject("JetOn",this.gameObject);

            // tell the animator to play the jet animation
            animator.SetBool("IsJetting", false);
        }

        // If the player is inputing an Aim Direction, rotate the gun to point in that direction
        if (aimDirection != Vector2.zero)
        {
            Quaternion newRotation = new Quaternion(Quaternion.identity.x, Quaternion.identity.y, Quaternion.LookRotation(aimDirection, Vector3.forward).z, Quaternion.LookRotation(aimDirection, Vector3.forward).w);
            gunObj.transform.rotation = newRotation * Quaternion.Euler(0f, 0f, 90f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if the player has collided with an Enemy...
        if (collision.gameObject.tag == "Enemy")
        {
            // check if the player isn't invincible, if they are, ignore the collision
            if (!isInvincible)
            {

                Enemy enemyComp = collision.gameObject.GetComponent<Enemy>();

                if (enemyComp != null)
                {
                    if (!enemyComp.CollisionDamage)
                    {
                        // THIS ENEMY DOES NOT DO COLLISION DAMAGE, RETURN AND IGNORE COLLISION
                        return;
                    }
                }

                // if there is no enemy component, or there is one and it allows for collision damage, then we will proceed and deal collision damage to the player

                // subtract player health from the Game Manager
                GameManager.Instance.CurrentHealth--;

                // if the player died on this collision, set the animator back to idel by setting its speed to 0 and jetpack state to false
                if (GameManager.Instance.CurrentHealth <= 0)
                {
                    animator.SetFloat("Speed", 0);
                    animator.SetBool("IsJetpacking", false);
                }

            }
        }
        else if (collision.gameObject.tag == "EnemyBullet")
        {
            // check if the player isn't invincible, if they are, ignore the collision
            if (!isInvincible)
            {
                // subtract player health from the Game Manager
                GameManager.Instance.CurrentHealth--;

                Destroy(collision.gameObject);

            }
        }
    }

    // This Routine will make the player invulnerable from attacks during its durration, and flash the players damage color on and off
    // (DAMAGE FROM PLATFORM BY PASSES INVINCIBILITY)
    public IEnumerator Invincibility(float Durration)
    {
        // set invincibility to true
        isInvincible = true;

        // create the count variable and set its starting value to the passed in Duration
        float count = Durration;

        // the flash rate in seconds
        float flashRate = .2f;

        // keeps track if the flash is On/Full Value or not
        bool flashOn = false;

        // while the count has not run out
        while (count > 0)
        {
            // while the player is invincible, the player might have died from the platfrom between counts. If so, set the flash to on and break out of the loop
            if (GameManager.Instance.CurrentHealth <= 0)
            {
                damageFlash.material.SetFloat("_Intensity", 1f);
                break;
            }

            // set the flash state to the opposite status
            if (flashOn)
            {
                flashOn = false;
                damageFlash.material.SetFloat("_Intensity", 0f);
            }
            else
            {
                flashOn = true;
                damageFlash.material.SetFloat("_Intensity", 1f);
            }

            // subtract the flash rate time from the count and wait for the rate time to pass
            count -= flashRate;
            yield return new WaitForSeconds(flashRate);
        }

        // wait an update
        yield return new WaitForFixedUpdate();

        // if the player is still alive, turn the damage flash off and make them vulnerable again
        if (GameManager.Instance.CurrentHealth > 0)
        {
            isInvincible = false;
            damageFlash.material.SetFloat("_Intensity", 0f);
        }
    }

    
}
