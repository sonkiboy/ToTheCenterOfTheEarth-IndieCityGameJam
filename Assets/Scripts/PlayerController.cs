using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Obj and Components

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;




    #endregion

    #region Inputs


    PlayerControllerInput inputManager;

    InputAction MoveInput;
    InputAction JetInput;
    InputAction LookInput;
    InputAction FireInput;

    #endregion


    public float Speed = 10;
    public float JetThrust = 40;

    private float ThrustPower = 0f;



    private Vector2 moveDirection;

    private void Awake()
    {
        inputManager = new PlayerControllerInput();

        MoveInput = inputManager.Player.Move;

        JetInput = inputManager.Player.Jet;

        LookInput = inputManager.Player.Look;

        FireInput = inputManager.Player.Fire;

    }


    private void OnEnable()
    {
        inputManager.Enable();
    }

    private void OnDisable()
    {
        inputManager.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = MoveInput.ReadValue<Vector2>();

        ThrustPower = JetInput.ReadValue<float>();

        

        //Debug.Log($"Move direction : {moveDirection}");
    }

    private void FixedUpdate()
    {
        Vector2 newPos = moveDirection * Speed;

        rb.AddForce(newPos);

        if (ThrustPower > .5f)
        {
            //Debug.Log("Thrusting");

            rb.AddForce(Vector2.up * JetThrust);
        }

    }
}
