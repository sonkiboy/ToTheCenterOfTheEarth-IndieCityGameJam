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
    Camera camera;

    Renderer damageFlash;

    GameObject gunObj;
    GunBehavior gunBehavior;
    SpriteRenderer gunSprite;

    #endregion



    #region Sounds

    public AK.Wwise.Event JetPackOn;
    public AK.Wwise.Event JetPackOff;


    #endregion

    #region Inputs
    PlayerControllerInput inputManager;

    InputAction MoveInput;
    InputAction MouseLookInput;
    InputAction JetInput;
    InputAction LookInput;
    InputAction FireInput;

    bool isController = false;
    bool isKeyboard = false;

    #endregion


    public float RunSpeed = 10;
    [SerializeField] float MaxRunSpeed = 10f;
    public float JetThrust = 40;

    public float InvincibilityDurration = 2f;

    private float ThrustPower = 0f;



    private Vector2 moveDirection = Vector2.zero;
    private Vector2 aimDirection = Vector2.zero;

    private bool isInvincible = false;

    private void Awake()
    {
        inputManager = new PlayerControllerInput();

        MoveInput = inputManager.Player.Move;

        JetInput = inputManager.Player.Jet;

        LookInput = inputManager.Player.Look;

        MouseLookInput = inputManager.Player.MouseLook;

        FireInput = inputManager.Player.Fire;

        gunObj = transform.Find("Gun").gameObject;

        gunBehavior = gunObj.GetComponent<GunBehavior>();

        gunSprite = gunObj.GetComponent<SpriteRenderer>();

        damageFlash = transform.Find("Sprite").GetComponent<Renderer>();

    }

    bool IsJeckpack = false;



    private void OnEnable()
    {
        inputManager.Enable();
    }

    private void OnDisable()
    {
        inputManager.Disable();

        damageFlash.material.SetFloat("_Intensity", 1f);

        
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();

        camera = Camera.main;

        //damageFlash.material.SetFloat("_Intensity", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = MoveInput.ReadValue<Vector2>();

        

        if (Keyboard.current != null)
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame && !isKeyboard)
            {
                Debug.Log($"Switching to Keyboard, found as {Keyboard.current}");
                isController = false;
                isKeyboard = true;
            }
        }
        if (Gamepad.current != null)
        {
            Debug.Log("Found but nothing pressed");
            if ((Gamepad.current.aButton.wasPressedThisFrame || Gamepad.current.leftTrigger.wasPressedThisFrame || Gamepad.current.rightTrigger.wasPressedThisFrame) && !isController)
            {
                Debug.Log("Switching to Controller");
                isController = true;
                isKeyboard = false;
            }
            
        }









        if (isController)
        {
            aimDirection = LookInput.ReadValue<Vector2>();
        }
        else if (isKeyboard)
        {
            aimDirection = MouseLookInput.ReadValue<Vector2>();
            aimDirection = ((Vector2)this.transform.position - (Vector2)camera.ScreenToWorldPoint(aimDirection)).normalized;
            aimDirection = -aimDirection;
        }







        if (aimDirection.x < 0 && spriteRenderer.flipX == true)
        {
            spriteRenderer.flipX = false;
            gunSprite.flipY = false;
        }
        else if (aimDirection.x > 0 && spriteRenderer.flipX == false)
        {
            spriteRenderer.flipX = true;
            gunSprite.flipY = true;
        }


        ThrustPower = JetInput.ReadValue<float>();

        if (FireInput.inProgress)
        {
            gunBehavior.Fire();
        }


        //Debug.Log($"Move direction : {moveDirection}");
    }

    private void FixedUpdate()
    {
        float dotDirection = Vector2.Dot(new Vector2(moveDirection.x, 0f), new Vector2(rb.velocity.x, 0f));

        if (dotDirection > 0)
        {
            if (Mathf.Abs(rb.velocity.x) < MaxRunSpeed)
            {
                Vector2 newPos = new Vector2(moveDirection.x * RunSpeed, 0f);

                rb.AddForce(newPos);
            }

        }
        else
        {
            Vector2 newPos = new Vector2(moveDirection.x * RunSpeed, 0f);

            rb.AddForce(newPos);
        }

        // sound stuff
        if (ThrustPower > .5f)
        {
            //Debug.Log("Thrusting");

            rb.AddForce(Vector2.up * JetThrust);

            if (!IsJeckpack)
            {
                JetPackOn.Post(gameObject);

                IsJeckpack = true;
            }

        }
        else if (IsJeckpack)
        {
            IsJeckpack = false;
            JetPackOff.Post(gameObject);
        }

        Quaternion newRotation = new Quaternion(Quaternion.identity.x, Quaternion.identity.y, Quaternion.LookRotation(aimDirection, Vector3.forward).z, Quaternion.LookRotation(aimDirection, Vector3.forward).w);

        gunObj.transform.rotation = newRotation * Quaternion.Euler(0f, 0f, 90f);
        //Debug.Log($"Aim Direction : {aimDirection} | Rotation : {gunObj.transform.rotation}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (!isInvincible)
            {
                GameManager.Instance.CurrentHealth--;

                StartCoroutine(Invincibility(InvincibilityDurration));
            }
        }
    }

    public IEnumerator Invincibility(float Durration)
    {



        isInvincible = true;

        float count = Durration;

        float flashRate = .2f;

        bool flashOn = false;

        while (count > 0)
        {
            if (GameManager.Instance.CurrentHealth <= 0)
            {
                damageFlash.material.SetFloat("_Intensity", 1f);
                break;
            }

            if (count - flashRate > 0)
            {
                count -= flashRate;

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


                yield return new WaitForSeconds(flashRate);
            }
            else
            {
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

                yield return new WaitForSeconds(count);
                break;
            }
        }

        damageFlash.material.SetFloat("_Intensity", 0f);

        yield return new WaitForFixedUpdate();

        if (GameManager.Instance.CurrentHealth > 0)
        {
            isInvincible = false;
        }


    }

    public IEnumerator Die()
    {
        AkSoundEngine.StopAll(gameObject);

        yield return null;
    }
}
