using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    EventSystem _system;
    InputSystemUIInputModule uiInputModule;

    OptionsSettings options;


    [SerializeField]
    Sprite[] boxSprites; 

    #region Obj and Component

    GameObject PauseMenuObj;

    [SerializeField] RectTransform Selector;
    GameObject selectorArrows;
    [SerializeField] GameObject MainPauseMenu;
    GameObject ResumeButton;


    [SerializeField] GameObject OptionsMenu;
    GameObject gamePlayButton;

   
    GameObject gamePlayPage;
    GameObject displayPage;
    GameObject soundPage;
    GameObject savePage;

    // GAMPLAY
    Image fireAimBox;
    Image jetMoveBox;

    // GRAPHICS
    GameObject resolutionBox;
    TextMeshProUGUI resolutionSettingTxt;
    Image fullscreenBox;
    Image screenShakeBox;
    Image vsynchBox;
    Image staticColorBox;
    Image vibrantColorBox;
    Image heartyColorBox;

    // SOUND
    GameObject masterSoundBar;
    GameObject soundBar;
    GameObject musicBar;

    #endregion

    GameObject currentSelectedOption;


    private void Awake()
    {
        _system = GameObject.FindAnyObjectByType<EventSystem>();
        uiInputModule = _system.GetComponent<InputSystemUIInputModule>();

        

        PauseMenuObj = transform.Find("Menu").gameObject;
        MainPauseMenu = PauseMenuObj.transform.Find("MainPauseMenu").gameObject;
        ResumeButton = MainPauseMenu.transform.Find("ResumeIcon").gameObject;

        Selector = PauseMenuObj.transform.Find("Selector").GetComponent<RectTransform>();
        selectorArrows = Selector.Find("Arrows").gameObject;

        OptionsMenu = PauseMenuObj.transform.Find("OptionsMenu").gameObject;
        gamePlayButton = OptionsMenu.transform.Find("SideIcons").Find("GamePlayIcon").gameObject;
        gamePlayPage = OptionsMenu.transform.Find("GamePlayPage").gameObject;
        displayPage = OptionsMenu.transform.Find("GraphicsPage").gameObject;
        soundPage = OptionsMenu.transform.Find("SoundPage").gameObject;
        savePage = OptionsMenu.transform.Find("SavePage").gameObject;

        fireAimBox = gamePlayPage.transform.Find("FireOnAimCheckBox").GetComponent<Image>();
        jetMoveBox = gamePlayPage.transform.Find("JetWithMoveCheckBox").GetComponent<Image>();

        resolutionSettingTxt = displayPage.transform.Find("ResolutionBox").GetComponentInChildren<TextMeshProUGUI>();
        fullscreenBox = displayPage.transform.Find("FullScreenCheckBox").GetComponent<Image>();
        screenShakeBox = displayPage.transform.Find("ScreenShakeCheckBox").GetComponent<Image>();
        vsynchBox = displayPage.transform.Find("VSynchCheckBox").GetComponent<Image>();
        staticColorBox = displayPage.transform.Find("StaticColorsCheckBox").GetComponent<Image>();
        vibrantColorBox = displayPage.transform.Find("VibrantColorBox").GetComponent<Image>();
        heartyColorBox = displayPage.transform.Find("HeartyColorBox").GetComponent<Image>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        PauseMenuObj.SetActive(false);
        GameManager.Instance.InputManager.MenuInput.performed += TogglePauseMenu;
        options = GameManager.Instance.GameOptions;

    }

    // Update is called once per frame
    void Update()
    {
        if(PauseMenuObj.activeSelf == true)
        {
            if (currentSelectedOption != _system.currentSelectedGameObject)
            {
                Debug.Log($"Setting Pause Current Selection from {currentSelectedOption} to {_system.currentSelectedGameObject}");
                currentSelectedOption = _system.currentSelectedGameObject;
                Selector.transform.position = currentSelectedOption.transform.position;
                Selector.sizeDelta = currentSelectedOption.GetComponent<RectTransform>().sizeDelta * (currentSelectedOption.transform.localScale.x/ Selector.localScale.x) * 2;
            }
           

            
        }
        
    }

    private void OnDisable()
    {
        GameManager.Instance.InputManager.MenuInput.performed -= TogglePauseMenu;

    }
    public void PointerOverObject(GameObject uiObject)
    {
        Debug.Log("hit");
        if (currentSelectedOption != uiObject)
        {
            Debug.Log($"Setting Pause Current Selection from {currentSelectedOption} to {uiObject}");
            currentSelectedOption = uiObject;
            Selector.transform.position = currentSelectedOption.transform.position;
        }
    }

    public void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (PauseMenuObj.activeSelf == true)
        {
            // close the menus
            Time.timeScale = 1;

            CloseMainPause();
            CloseOptionsMenu();
            PauseMenuObj.SetActive(false);

            GameManager.Instance.InputManager.UIEnter.performed -= ExitVibrantBox;
            GameManager.Instance.InputManager.UIMoveInput.performed -= ChangeVibrantBox;
            GameManager.Instance.InputManager.UIEnter.performed -= ExitHeartyBox;
            GameManager.Instance.InputManager.UIMoveInput.performed -= ChangeHeartyBox;
            GameManager.Instance.InputManager.UIEnter.performed -= ExitResBox;
            GameManager.Instance.InputManager.UIMoveInput.performed -= ChangeResBox;
            selectorArrows.SetActive(false);
            uiInputModule.enabled = true;

        }
        else if(PauseMenuObj.activeSelf == false && GameManager.Instance.CurrentState != GameManager.GameStates.GameOver) 
        {
            Time.timeScale = 0;

            PauseMenuObj.SetActive(true);
            
            OpenMainPause();
        }
    }

    public void OnOptionMove(InputAction.CallbackContext context)
    {
        Vector2 readInput = context.ReadValue<Vector2>();


        if (Mathf.Abs(readInput.x) > .25f)
        {
            return;
        }

        bool isLeft;

        if(readInput.x > 0)
        {
            isLeft = false;
        }
        else 
        {
            isLeft = true;
        }

        
    }


    #region Main Pause Menu Functions

    public void OpenMainPause()
    {
        MainPauseMenu.SetActive(true);
        _system.SetSelectedGameObject(ResumeButton);
        CloseOptionsMenu();
    }

    public void CloseMainPause()
    {
        MainPauseMenu.SetActive(false);
    }

    public void ResumeGame()
    {
        TogglePauseMenu(new InputAction.CallbackContext());
    }

    public void QuitGame()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            Application.Quit();
        }
        else
        {
            TogglePauseMenu(new InputAction.CallbackContext());
            SceneManager.LoadScene("MainMenu");
        }
    }



    #endregion

    #region Option Menu Functions
    public void OpenOptionsMenu()
    {
        Debug.Log("Hit open options");
        OptionsMenu.SetActive(true);
        _system.SetSelectedGameObject(gamePlayButton);

        gamePlayPage.SetActive(true);
        displayPage.SetActive(false);
        soundPage.SetActive(false);
        savePage.SetActive(false);
        UpdateOptions();
        CloseMainPause();
    }

    public void CloseOptionsMenu()
    {
        OptionsMenu.SetActive(false);
    }

    void UpdateOptions()
    {
        if (options.IsFireOnAim) fireAimBox.sprite = boxSprites[1];
        else fireAimBox.sprite = boxSprites[0];

        if (options.IsJetOnMove) jetMoveBox.sprite = boxSprites[1];
        else jetMoveBox.sprite = boxSprites[0];

        if (options.IsFullScreen) fullscreenBox.sprite = boxSprites[1];
        else fullscreenBox.sprite = boxSprites[0];

        if (options.IsScreenShakeOn) screenShakeBox.sprite = boxSprites[1];
        else screenShakeBox.sprite = boxSprites[0];

        if (options.IsVSynchOn) vsynchBox.sprite = boxSprites[1];
        else vsynchBox.sprite = boxSprites[0];

        if (options.IsStaticColorsOn) staticColorBox.sprite = boxSprites[1];
        else staticColorBox.sprite = boxSprites[0];

    }

    public void OpenGamePlayPage()
    {
        UpdateOptions();

        gamePlayPage.SetActive(true);
        displayPage.SetActive(false);
        soundPage.SetActive(false);
        savePage.SetActive(false);
    }
    public void OpenDisplayPage()
    {

        UpdateOptions();

        gamePlayPage.SetActive(false);
        displayPage.SetActive(true);
        soundPage.SetActive(false);
        savePage.SetActive(false);

        if (options.IsStaticColorsOn)
        {
            
            vibrantColorBox.gameObject.SetActive(true);
            heartyColorBox.gameObject.SetActive(true);
        }
        else
        {
            vibrantColorBox.gameObject.SetActive(false);
            heartyColorBox.gameObject.SetActive(false);
        }
    }
    public void OpenSoundPage()
    {

        UpdateOptions();
        gamePlayPage.SetActive(false);
        displayPage.SetActive(false);
        soundPage.SetActive(true);
        savePage.SetActive(false);
    }
    public void OpenSavePage()
    {
        UpdateOptions();
        gamePlayPage.SetActive(false);
        displayPage.SetActive(false);
        soundPage.SetActive(false);
        savePage.SetActive(true);
    }

    #endregion

    #region Gameplay Option Functions

    #endregion

    #region Graphics Option Functions

    public void OnFullscreenClick()
    {
        if (options.IsFullScreen)
        {
            options.IsFullScreen = false;
            fullscreenBox.sprite = boxSprites[0];

            Screen.fullScreen = false;

        }
        else
        {
            options.IsFullScreen = true;
            fullscreenBox.sprite = boxSprites[1];

            Screen.fullScreen = true;
        }
    }

    // starts the change resolution mode
    public void OnResolutionClick()
    {
        GameManager.Instance.InputManager.UIEnter.performed += ExitResBox;
        GameManager.Instance.InputManager.UIMoveInput.performed += ChangeResBox;
        selectorArrows.SetActive(true);
        uiInputModule.enabled = false;

    }
    public void OnScreenShakeClick()
    {
        if (options.IsScreenShakeOn)
        {
            options.IsScreenShakeOn = false;
            screenShakeBox.sprite = boxSprites[0];

            

        }
        else
        {
            options.IsScreenShakeOn = true;
            screenShakeBox.sprite = boxSprites[1];

            
        }
    }

    public void OnVSynchClick()
    {
        if (options.IsVSynchOn)
        {
            options.IsVSynchOn = false;
            vsynchBox.sprite = boxSprites[0];

            QualitySettings.vSyncCount = 0;

        }
        else
        {
            options.IsVSynchOn = true;
            vsynchBox.sprite = boxSprites[1];

            QualitySettings.vSyncCount = 1;

        }
    }
    public void ExitResBox(InputAction.CallbackContext context)
    {
        GameManager.Instance.InputManager.UIEnter.performed -= ExitResBox;
        GameManager.Instance.InputManager.UIMoveInput.performed -= ChangeResBox;
        selectorArrows.SetActive(false);
        uiInputModule.enabled = true;

    }

    public void ChangeResBox(InputAction.CallbackContext context)
    {
        // incriment up
        if(context.ReadValue<Vector2>().x > 0f)
        {
            options.CurrentResolutionIndex++;
            if(options.CurrentResolutionIndex >= options.SupportedResolutions.Length)
            {
                options.CurrentResolutionIndex = 0;
            }
            
            
        }
        else if(context.ReadValue<Vector2>().x < 0f)
        {
            options.CurrentResolutionIndex--;
            if (options.CurrentResolutionIndex < 0)
            {
                options.CurrentResolutionIndex = options.SupportedResolutions.Length-1;
            }
        }

        resolutionSettingTxt.text = options.SupportedResolutions[options.CurrentResolutionIndex];

        int width = int.Parse(resolutionSettingTxt.text.Split("x")[0]);
        int height = int.Parse(resolutionSettingTxt.text.Split("x")[1]);

        Screen.SetResolution(width,height,GameManager.Instance.GameOptions.IsFullScreen);
    }

    public void StaticColorButton()
    {
        if(options.IsStaticColorsOn == true)
        {
            options.IsStaticColorsOn = false;
            staticColorBox.sprite = boxSprites[0];
            heartyColorBox.gameObject.SetActive(false);
            vibrantColorBox.gameObject.SetActive(false);
            GameManager.Instance.PaletteManager.RefreshCurrentColors();

        }
        else
        {
            options.IsStaticColorsOn = true;
            staticColorBox.sprite = boxSprites[1];

            heartyColorBox.gameObject.SetActive(true);
            vibrantColorBox.gameObject.SetActive(true);
            GameManager.Instance.PaletteManager.SetToStaticColors();

        }
    }

    public void OnHeartyColorClick()
    {
        GameManager.Instance.InputManager.UIEnter.performed += ExitHeartyBox;
        GameManager.Instance.InputManager.UIMoveInput.performed += ChangeHeartyBox;
        selectorArrows.SetActive(true);
        uiInputModule.enabled = false;

    }

    public void ExitHeartyBox(InputAction.CallbackContext context)
    {
        GameManager.Instance.InputManager.UIEnter.performed -= ExitHeartyBox;
        GameManager.Instance.InputManager.UIMoveInput.performed -= ChangeHeartyBox;
        selectorArrows.SetActive(false);
        uiInputModule.enabled = true;

    }

    public void ChangeHeartyBox(InputAction.CallbackContext context)
    {
        // incriment up
        if (context.ReadValue<Vector2>().x > 0f)
        {
            options.StaticHeartyColor++;
            if (options.StaticHeartyColor >= options.SupportedStaticColors.Length)
            {
                options.StaticHeartyColor = 0;
            }


        }
        else if (context.ReadValue<Vector2>().x < 0f)
        {
            options.StaticHeartyColor--;
            if (options.StaticHeartyColor < 0)
            {
                options.StaticHeartyColor = options.SupportedStaticColors.Length - 1;
            }
        }

        GameManager.Instance.PaletteManager.SetToStaticColors();

    }

    public void OnVibrantColorClick()
    {
        GameManager.Instance.InputManager.UIEnter.performed += ExitVibrantBox;
        GameManager.Instance.InputManager.UIMoveInput.performed += ChangeVibrantBox;
        selectorArrows.SetActive(true);
        uiInputModule.enabled = false;

    }

    public void ExitVibrantBox(InputAction.CallbackContext context)
    {
        GameManager.Instance.InputManager.UIEnter.performed -= ExitVibrantBox;
        GameManager.Instance.InputManager.UIMoveInput.performed -= ChangeVibrantBox;
        selectorArrows.SetActive(false);
        uiInputModule.enabled = true;

    }

    public void ChangeVibrantBox(InputAction.CallbackContext context)
    {
        // incriment up
        if (context.ReadValue<Vector2>().x > 0f)
        {
            options.StaticVibrantColor++;
            if (options.StaticVibrantColor >= options.SupportedStaticColors.Length)
            {
                options.StaticVibrantColor = 0;
            }


        }
        else if (context.ReadValue<Vector2>().x < 0f)
        {
            options.StaticVibrantColor--;
            if (options.StaticVibrantColor < 0)
            {
                options.StaticVibrantColor = options.SupportedStaticColors.Length - 1;
            }
        }

        GameManager.Instance.PaletteManager.SetToStaticColors();

    }

    #endregion
}
