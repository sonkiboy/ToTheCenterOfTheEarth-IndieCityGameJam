using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{

    #region Inputs
    PlayerControllerInput inputManager;

    public InputAction MoveInput;
    //public InputAction MouseLookInput;
    public InputAction JetInput;
    public InputAction LookInput;
    public InputAction FireInput;
    public InputAction MenuInput;

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
        //MouseLookInput = inputManager.Player.MouseLook;
        FireInput = inputManager.Player.Fire;
        MenuInput = inputManager.Player.Start;

        DebugColorChange = inputManager.Debug.ChangePallete;
    }

    private void OnEnable()
    {
        inputManager.Enable();

        MoveInput.Enable();
        JetInput.Enable();
        LookInput.Enable();
        //MouseLookInput.Enable();
        FireInput.Enable();
        MenuInput.Enable();
        MenuInput.performed += OnMenuDebugPress;

        DebugColorChange.Enable();
        DebugColorChange.performed += OnColorChangeDebugPress;



    }

    private void OnDisable()
    {
        inputManager.Disable();

        MoveInput.Disable();
        JetInput.Disable();
        LookInput.Disable();
        //MouseLookInput.Disable();
        FireInput.Disable();
        MenuInput.Disable();
        MenuInput.performed -= OnMenuDebugPress;

        DebugColorChange.Enable();
        DebugColorChange.performed -= OnColorChangeDebugPress;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MenuInput.performed += OnMenuDebugPress;

    }

    // Update is called once per frame
    void Update()
    {
        //if (Keyboard.current != null)
        //{
        //    if (Keyboard.current.anyKey.wasPressedThisFrame && !isKeyboard)
        //    {
        //        //Debug.Log($"Switching to Keyboard, found as {Keyboard.current}");
        //        isController = false;
        //        isKeyboard = true;
        //    }
        //}
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

    void OnMenuDebugPress(InputAction.CallbackContext context)
    {
        if(SceneManager.GetActiveScene().name == "RegularGame")
        {
            AkUnitySoundEngine.StopAll();
            SceneManager.LoadScene("MainMenu");
        }
    }

    void OnColorChangeDebugPress(InputAction.CallbackContext context)
    {
        if(SceneManager.GetActiveScene().name == "MainMenu" && GameManager.Instance.PaletteManager != null)
        {
            GameManager.Instance.PaletteManager.IncrimentPallete(.25f);
        }
    }
}
