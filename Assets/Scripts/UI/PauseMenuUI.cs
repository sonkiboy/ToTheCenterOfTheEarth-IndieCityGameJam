using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    EventSystem _system;
    InputSystemUIInputModule uiInputModule;

    OptionsSettings options;

    CanvasScaler screenScaler;

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
    RectTransform masterSoundBar;
    RectTransform soundBar;
    RectTransform musicBar;

    GameObject DeleteLBDataButton;
    GameObject LBDataConfirmPage;
    GameObject CancelLBDataButton;
    GameObject DeleteGameDataButton;
    GameObject GameDataConfirmPage;
    GameObject CancelGameDataButton;


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

        masterSoundBar = soundPage.transform.Find("MasterSoundBar").Find("Bar").GetComponent<RectTransform>();
        soundBar = soundPage.transform.Find("SoundBar").Find("Bar").GetComponent<RectTransform>();
        musicBar = soundPage.transform.Find("MusicBar").Find("Bar").GetComponent<RectTransform>();

        DeleteLBDataButton = savePage.transform.Find("ResetLBBox").gameObject;
        LBDataConfirmPage = savePage.transform.Find("LeaderBoardUSure").gameObject;
        CancelLBDataButton = LBDataConfirmPage.transform.Find("CancelButton").gameObject;

        DeleteGameDataButton = savePage.transform.Find("ResetGameData").gameObject;
        GameDataConfirmPage = savePage.transform.Find("SaveDataUSure").gameObject;
        CancelGameDataButton = GameDataConfirmPage.transform.Find("CancelButton").gameObject;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screenScaler = GameManager.Instance.LowResCamera.transform.Find("RendererCanvas").GetComponent<CanvasScaler>();

        PauseMenuObj.SetActive(false);
        GameManager.Instance.InputManager.MenuInput.performed += TogglePauseMenu;
        options = GameManager.Instance.GameOptions;

    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenuObj.activeSelf == true)
        {
            if (currentSelectedOption != _system.currentSelectedGameObject)
            {
                Debug.Log($"Setting Pause Current Selection from {currentSelectedOption} to {_system.currentSelectedGameObject}");
                currentSelectedOption = _system.currentSelectedGameObject;
                Selector.transform.position = currentSelectedOption.transform.position;
                Selector.sizeDelta = currentSelectedOption.GetComponent<RectTransform>().sizeDelta * (currentSelectedOption.transform.localScale.x / Selector.localScale.x) * 2;
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

            options.SaveSettingsToFile();

        }
        else if (PauseMenuObj.activeSelf == false && GameManager.Instance.CurrentState != GameManager.GameStates.GameOver)
        {
            Time.timeScale = 0;

            PauseMenuObj.SetActive(true);
            options.SaveSettingsToFile();

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

        if (readInput.x > 0)
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
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (GameManager.Instance.InputManager.isArcade == false)
            {
                Application.Quit();
            }

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
        options.SaveSettingsToFile();

        gamePlayPage.SetActive(true);
        displayPage.SetActive(false);
        soundPage.SetActive(false);
        savePage.SetActive(false);
        UpdateOptions();
        CloseMainPause();
    }

    public void CloseOptionsMenu()
    {
        options.SaveSettingsToFile();

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

        resolutionSettingTxt.text = options.SupportedResolutions[options.CurrentResolutionIndex];

        masterSoundBar.sizeDelta = new Vector2(options.MasterVolume, 10);
        soundBar.sizeDelta = new Vector2(options.SoundVolume, 10);
        musicBar.sizeDelta = new Vector2(options.MusicVolume, 10);

        GameManager.Instance.SoundManager.MasterVolumeParam.SetGlobalValue(options.MasterVolume);
        GameManager.Instance.SoundManager.SoundVolumeParam.SetGlobalValue(options.SoundVolume);
        GameManager.Instance.SoundManager.MusicVolumeParam.SetGlobalValue(options.MusicVolume);


    }

    public void OpenGamePlayPage()
    {
        UpdateOptions();
        options.SaveSettingsToFile();

        gamePlayPage.SetActive(true);
        displayPage.SetActive(false);
        soundPage.SetActive(false);
        savePage.SetActive(false);
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }
    public void OpenDisplayPage()
    {

        UpdateOptions();
        options.SaveSettingsToFile();

        gamePlayPage.SetActive(false);
        displayPage.SetActive(true);
        soundPage.SetActive(false);
        savePage.SetActive(false);

        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");


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
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }
    public void OpenSavePage()
    {
        UpdateOptions();
        gamePlayPage.SetActive(false);
        displayPage.SetActive(false);
        soundPage.SetActive(false);
        savePage.SetActive(true);
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    #endregion

    #region Gameplay Option Functions

    public void OnFireWithStickClick()
    {
        if (options.IsFireOnAim)
        {
            options.IsFireOnAim = false;
            options.UpdateSettings();
            fireAimBox.sprite = boxSprites[0];
            GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

        }
        else
        {
            options.IsFireOnAim = true;
            fireAimBox.sprite = boxSprites[1];

            options.UpdateSettings();
            GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

        }
    }

    public void OnJetWithStickClick()
    {
        if (options.IsJetOnMove)
        {
            options.IsJetOnMove = false;
            options.UpdateSettings();
            jetMoveBox.sprite = boxSprites[0];
            GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

        }
        else
        {
            options.IsJetOnMove = true;
            jetMoveBox.sprite = boxSprites[1];

            options.UpdateSettings();
            GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

        }
    }

    #endregion

    #region Graphics Option Functions

    public void OnFullscreenClick()
    {
        if (options.IsFullScreen)
        {
            options.IsFullScreen = false;
            fullscreenBox.sprite = boxSprites[0];

            Screen.fullScreen = false;

            GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

        }
        else
        {
            options.IsFullScreen = true;
            fullscreenBox.sprite = boxSprites[1];

            Screen.fullScreen = true;
            GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

        }
        if ((float)Screen.width / (float)Screen.height < 1.777f)
        {
            screenScaler.matchWidthOrHeight = 0;
        }
        else
        {
            screenScaler.matchWidthOrHeight = 1;

        }
    }

    // starts the change resolution mode
    public void OnResolutionClick()
    {
        GameManager.Instance.InputManager.UIEnter.performed += ExitResBox;
        GameManager.Instance.InputManager.UIMoveInput.performed += ChangeResBox;
        selectorArrows.SetActive(true);
        uiInputModule.enabled = false;
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }
    public void OnScreenShakeClick()
    {
        if (options.IsScreenShakeOn)
        {
            options.IsScreenShakeOn = false;
            screenShakeBox.sprite = boxSprites[0];

            GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");


        }
        else
        {
            options.IsScreenShakeOn = true;
            screenShakeBox.sprite = boxSprites[1];
            GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");


        }
    }

    public void OnVSynchClick()
    {
        if (options.IsVSynchOn)
        {
            options.IsVSynchOn = false;
            vsynchBox.sprite = boxSprites[0];

            QualitySettings.vSyncCount = 0;
            GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

        }
        else
        {
            options.IsVSynchOn = true;
            vsynchBox.sprite = boxSprites[1];

            QualitySettings.vSyncCount = 1;
            GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

        }
    }
    public void ExitResBox(InputAction.CallbackContext context)
    {
        GameManager.Instance.InputManager.UIEnter.performed -= ExitResBox;
        GameManager.Instance.InputManager.UIMoveInput.performed -= ChangeResBox;
        selectorArrows.SetActive(false);
        uiInputModule.enabled = true;
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void ChangeResBox(InputAction.CallbackContext context)
    {
        // incriment up
        if (context.ReadValue<Vector2>().x > 0f)
        {
            options.CurrentResolutionIndex++;
            if (options.CurrentResolutionIndex >= options.SupportedResolutions.Length)
            {
                options.CurrentResolutionIndex = 0;
            }


        }
        else if (context.ReadValue<Vector2>().x < 0f)
        {
            options.CurrentResolutionIndex--;
            if (options.CurrentResolutionIndex < 0)
            {
                options.CurrentResolutionIndex = options.SupportedResolutions.Length - 1;
            }
        }

        resolutionSettingTxt.text = options.SupportedResolutions[options.CurrentResolutionIndex];

        int width = int.Parse(resolutionSettingTxt.text.Split("x")[0]);
        int height = int.Parse(resolutionSettingTxt.text.Split("x")[1]);

        Screen.SetResolution(width, height, GameManager.Instance.GameOptions.IsFullScreen);
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

        if ((float)width / (float)height < 1.777f)
        {
            screenScaler.matchWidthOrHeight = 0;
        }
        else
        {
            screenScaler.matchWidthOrHeight = 1;

        }

    }

    public void StaticColorButton()
    {
        if (options.IsStaticColorsOn == true)
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
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void OnHeartyColorClick()
    {
        GameManager.Instance.InputManager.UIEnter.performed += ExitHeartyBox;
        GameManager.Instance.InputManager.UIMoveInput.performed += ChangeHeartyBox;
        selectorArrows.SetActive(true);
        uiInputModule.enabled = false;
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void ExitHeartyBox(InputAction.CallbackContext context)
    {
        GameManager.Instance.InputManager.UIEnter.performed -= ExitHeartyBox;
        GameManager.Instance.InputManager.UIMoveInput.performed -= ChangeHeartyBox;
        selectorArrows.SetActive(false);
        uiInputModule.enabled = true;
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

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
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void OnVibrantColorClick()
    {
        GameManager.Instance.InputManager.UIEnter.performed += ExitVibrantBox;
        GameManager.Instance.InputManager.UIMoveInput.performed += ChangeVibrantBox;
        selectorArrows.SetActive(true);
        uiInputModule.enabled = false;
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void ExitVibrantBox(InputAction.CallbackContext context)
    {
        GameManager.Instance.InputManager.UIEnter.performed -= ExitVibrantBox;
        GameManager.Instance.InputManager.UIMoveInput.performed -= ChangeVibrantBox;
        selectorArrows.SetActive(false);
        uiInputModule.enabled = true;
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

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
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

        GameManager.Instance.PaletteManager.SetToStaticColors();

    }

    #endregion

    #region Sound Option Functions

    public void OnMasterSoundClick()
    {
        GameManager.Instance.InputManager.UIEnter.performed += ExitMasterSoundBox;
        GameManager.Instance.InputManager.UIMoveInput.performed += ChangeMasterSoundBox;
        selectorArrows.SetActive(true);
        uiInputModule.enabled = false;
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void ExitMasterSoundBox(InputAction.CallbackContext context)
    {
        GameManager.Instance.InputManager.UIEnter.performed -= ExitMasterSoundBox;
        GameManager.Instance.InputManager.UIMoveInput.performed -= ChangeMasterSoundBox;
        selectorArrows.SetActive(false);
        uiInputModule.enabled = true;
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void ChangeMasterSoundBox(InputAction.CallbackContext context)
    {
        // incriment up
        if (context.ReadValue<Vector2>().x > 0f)
        {

            if (options.MasterVolume < 100)
            {
                options.MasterVolume += 5;
            }


        }
        else if (context.ReadValue<Vector2>().x < 0f)
        {
            if (options.MasterVolume > 0)
            {
                options.MasterVolume -= 5;
            }
        }
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

        masterSoundBar.sizeDelta = new Vector2(options.MasterVolume, 10);
        GameManager.Instance.SoundManager.MasterVolumeParam.SetGlobalValue(options.MasterVolume);
    }

    public void OnSoundClick()
    {
        GameManager.Instance.InputManager.UIEnter.performed += ExitSoundBox;
        GameManager.Instance.InputManager.UIMoveInput.performed += ChangeSoundBox;
        selectorArrows.SetActive(true);
        uiInputModule.enabled = false;
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void ExitSoundBox(InputAction.CallbackContext context)
    {
        GameManager.Instance.InputManager.UIEnter.performed -= ExitSoundBox;
        GameManager.Instance.InputManager.UIMoveInput.performed -= ChangeSoundBox;
        selectorArrows.SetActive(false);
        uiInputModule.enabled = true;
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void ChangeSoundBox(InputAction.CallbackContext context)
    {
        // incriment up
        if (context.ReadValue<Vector2>().x > 0f)
        {

            if (options.SoundVolume < 100)
            {
                options.SoundVolume += 5;
            }


        }
        else if (context.ReadValue<Vector2>().x < 0f)
        {
            if (options.SoundVolume > 0)
            {
                options.SoundVolume -= 5;
            }
        }

        soundBar.sizeDelta = new Vector2(options.SoundVolume, 10);
        GameManager.Instance.SoundManager.SoundVolumeParam.SetGlobalValue(options.SoundVolume);
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void OnMusicClick()
    {
        GameManager.Instance.InputManager.UIEnter.performed += ExitMusicBox;
        GameManager.Instance.InputManager.UIMoveInput.performed += ChangeMusicBox;
        selectorArrows.SetActive(true);
        uiInputModule.enabled = false;
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void ExitMusicBox(InputAction.CallbackContext context)
    {
        GameManager.Instance.InputManager.UIEnter.performed -= ExitMusicBox;
        GameManager.Instance.InputManager.UIMoveInput.performed -= ChangeMusicBox;
        selectorArrows.SetActive(false);
        uiInputModule.enabled = true;
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void ChangeMusicBox(InputAction.CallbackContext context)
    {
        // incriment up
        if (context.ReadValue<Vector2>().x > 0f)
        {

            if (options.MusicVolume < 100)
            {
                options.MusicVolume += 5;
            }


        }
        else if (context.ReadValue<Vector2>().x < 0f)
        {
            if (options.MusicVolume > 0)
            {
                options.MusicVolume -= 5;
            }
        }

        musicBar.sizeDelta = new Vector2(options.MusicVolume, 10);
        GameManager.Instance.SoundManager.MusicVolumeParam.SetGlobalValue(options.MusicVolume);
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");


    }

    #endregion

    #region Save Option Functions

    public void OnDeleteLeaderBoardClick()
    {
        LBDataConfirmPage.SetActive(true);
        _system.SetSelectedGameObject(CancelLBDataButton);
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void OnConfirmLBDelete()
    {
        LBDataConfirmPage.SetActive(false);
        _system.SetSelectedGameObject(DeleteLBDataButton);
        File.WriteAllText(Application.persistentDataPath + "/LeaderBoard.txt", "");

        options.UpdateSettings();
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void OnCancelLBDelete()
    {
        LBDataConfirmPage.SetActive(false);
        _system.SetSelectedGameObject(DeleteLBDataButton);
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void OnDeleteSaveDataClick()
    {
        GameDataConfirmPage.SetActive(true);
        _system.SetSelectedGameObject(CancelGameDataButton);
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void OnConfirmSaveDataDelete()
    {
        GameDataConfirmPage.SetActive(false);
        _system.SetSelectedGameObject(DeleteGameDataButton);
        options.UpdateSettings();
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    public void OnCancelSaveDataDelete()
    {
        GameDataConfirmPage.SetActive(false);
        _system.SetSelectedGameObject(DeleteGameDataButton);
        GameManager.Instance.SoundManager.PlayNonDiageticSound("Blip");

    }

    #endregion
}
