using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{

    #region Inputs
    PlayerControllerInput inputManager;

    public InputAction MoveInput;
    public InputAction MouseLookInput;
    public InputAction JetInput;
    public InputAction LookInput;
    public InputAction FireInput;
    public InputAction MenuInput;
    public InputAction UIMoveInput;
    public InputAction UIHeldMoveInput;
    public InputAction UIEnter;
    public InputAction AnyInput;

    public InputAction DebugColorChange;

    public bool isController = false;
    public bool isKeyboard = false;

    #endregion

    private void Awake()
    {
        // create the instance of the input script
        inputManager = new PlayerControllerInput();

        // Assign Inputs to their events
        MoveInput = inputManager.Player.Move;
        JetInput = inputManager.Player.Jet;
        LookInput = inputManager.Player.Look;
        MouseLookInput = inputManager.Player.MouseLook;
        FireInput = inputManager.Player.Fire;
        MenuInput = inputManager.Player.Start;
        UIMoveInput = inputManager.UI.Move;
        UIHeldMoveInput = inputManager.UI.MoveHeld;
        UIEnter = inputManager.UI.Enter;
        AnyInput = inputManager.UI.AnyButton;

        DebugColorChange = inputManager.Debug.ChangePallete;
    }

    private void OnEnable()
    {
        inputManager.Enable();

        MoveInput.Enable();
        JetInput.Enable();
        LookInput.Enable();
        MouseLookInput.Enable();
        FireInput.Enable();
        MenuInput.Enable();
        UIMoveInput.Enable();
        UIHeldMoveInput.Enable();
        UIEnter.Enable();
        AnyInput.Enable();
        //MenuInput.performed += OnMenuDebugPress;

        DebugColorChange.Enable();
        //DebugColorChange.performed += OnColorChangeDebugPress;



    }

    private void OnDisable()
    {
        inputManager.Disable();

        MoveInput.Disable();
        JetInput.Disable();
        LookInput.Disable();
        MouseLookInput.Disable();
        FireInput.Disable();
        MenuInput.Disable();
        AnyInput.Disable();
        UIMoveInput.Disable();
        UIHeldMoveInput.Disable();
        UIEnter.Disable();

        DebugColorChange.Disable();
        DebugColorChange.performed -= OnMenuDebugPress;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DebugColorChange.performed += OnMenuDebugPress;

    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current != null)
        {
            if ((Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame) && !isKeyboard)
            {
                //Debug.Log($"Switching to Keyboard, found as {Keyboard.current}");
                isController = false;
                isKeyboard = true;
            }
        }
        if (Gamepad.current != null)
        {
            //Debug.Log("Found but nothing pressed");
            if ((Gamepad.current.aButton.wasPressedThisFrame || Gamepad.current.leftTrigger.wasPressedThisFrame || Gamepad.current.rightTrigger.wasPressedThisFrame) && !isController)
            {
                //Debug.Log("Switching to Controller");
                isController = true;
                isKeyboard = false;
            }

        }
    }

    bool testpause = false;
    void OnMenuDebugPress(InputAction.CallbackContext context)
    {
        //if(SceneManager.GetActiveScene().name == "RegularGame")
        //{
        //    AkUnitySoundEngine.StopAll();
        //    SceneManager.LoadScene("MainMenu");
        //}

        //if(testpause == false)
        //{
        //    Time.timeScale = 0;
        //    testpause = true;
        //}
        //else
        //{
        //    Time.timeScale = 1;
        //    testpause = false;
        //}
        GameManager.Instance.hatIndex++;
        if (GameManager.Instance.hatIndex >= GameManager.Instance.Hats.Length)
        {
            GameManager.Instance.hatIndex = 0;
        }

        GameManager.Instance.CurrentHat = GameManager.Instance.Hats[GameManager.Instance.hatIndex];

    }

    void OnColorChangeDebugPress(InputAction.CallbackContext context)
    {
        if(SceneManager.GetActiveScene().name == "MainMenu" && GameManager.Instance.PaletteManager != null)
        {
            GameManager.Instance.PaletteManager.IncrimentPallete(.25f);
        }
    }
}
