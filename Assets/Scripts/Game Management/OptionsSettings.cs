using System.IO;
using UnityEngine;



public class OptionsSettings : MonoBehaviour
{
    private string path;
    

    // GAMEPLAY SETTINGS
    private bool _fireAim = false;
    public bool IsFireOnAim
    {
        get { return _fireAim;}
        set 
        { 
            _fireAim = value;
        }
    }
    private bool _jetMove = false;
    public bool IsJetOnMove
    {
        get { return _jetMove; }
        set
        {
            _jetMove = value;
        }
    }

    // GRAPHICS SETTINGS
    public bool IsFullScreen;
    public string[] SupportedResolutions = new string[]
    {
        // 16:9
        "2560x1440",
        "1920x1080",
        "1366x768",
        "1280x720",
        // 16:10
        "1920x1200",
        "1680x1050",
        "1440x900",
        "1280x800",
        // 4:3
        "1024x768",
        "800x600",
        "640x480",
        // One to One
        "368x208",
    };


    public int CurrentResolutionIndex;
    public bool IsScreenShakeOn;
    public bool IsVSynchOn;
    
    //public enum SupportedStaticColors
    //{
        
    //    Aqua = 0x55ffff,
    //    Red = 0xff5555,
    //    Violet = 0xff55ff,
    //    Yellow = 0xffff55,
    //    Orange = 0xffff55, 
    //    Brown = 0xaa5500,
    //    Grey = 0xaaaaaa,
    //    DarkGrey = 0x555555,
    //    Blue = 0x5555ff,
    //    Green = 0x55ff55,
    //}

    public Color[] SupportedStaticColors = new Color[]
    {
        //Aqua
        new Color(85f/255f,255f/255f,255f/255f),
        //Red
       new Color(255f/255f,85f/255f,85f/255f),
        //Violet
        new Color(255f/255f,85f/255f,255f / 255f),
        //Yellow
        new Color(255f / 255f,255f / 255f,85f / 255f),
        //Orange
        new Color(255f / 255f,85f / 255f,0f / 255f),
        //Brown
        new Color(170f / 255f,85f / 255f,0f / 255f),
        //Grey
        new Color(170f / 255f,170f / 255f,170f / 255f),
        //DarkGrey
        new Color(85f / 255f,85f / 255f,85f / 255f),
        //Blue
        new Color(85f / 255f,85f / 255f,255f / 255f),
        //Green
        new Color(85f / 255f,255f / 255f,85f / 255f),


    };
    public bool IsStaticColorsOn;
    public int StaticVibrantColor;
    public int StaticHeartyColor;

    // SOUND SETTINGS
    public float MasterVolume = 50;
    public float SoundVolume = 50;
    public float MusicVolume = 50;

    // SAVE SETTINGS

    public string LastHighscoreName = "sink";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ColorUtility.TryParseHtmlString(SupportedStaticColors.Aqua.ToString(), out StaticVibrantColor);
        //ColorUtility.TryParseHtmlString(SupportedStaticColors.Red.ToString(), out StaticHeartyColor);

        path = Application.persistentDataPath + "/OptionsSettings.txt";
        LoadSettingsFromFile();


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadSettingsFromFile()
    {
        // if the file for the score board scores doesn't exist, then make a new one that has no text

        if (File.Exists(path)) 
        {
            string jsonstring = File.ReadAllText(path);
            OptionsSettings loadedSettings = new OptionsSettings();
            JsonUtility.FromJsonOverwrite(jsonstring, loadedSettings);

            // Load Gameplay settings
            IsFireOnAim = loadedSettings.IsFireOnAim;
            IsJetOnMove = loadedSettings.IsJetOnMove;

            // Load Graphics Settings
            IsFullScreen = loadedSettings.IsFullScreen;
            CurrentResolutionIndex = loadedSettings.CurrentResolutionIndex;
            IsScreenShakeOn = loadedSettings.IsScreenShakeOn;
            IsVSynchOn = loadedSettings.IsVSynchOn;
            IsStaticColorsOn = loadedSettings.IsStaticColorsOn;
            StaticVibrantColor = loadedSettings.StaticVibrantColor;
            StaticHeartyColor = loadedSettings.StaticHeartyColor;

            // Load Sound Settings
            MasterVolume = loadedSettings.MasterVolume;
            SoundVolume = loadedSettings.SoundVolume;
            MusicVolume = loadedSettings.MusicVolume;

            LastHighscoreName = loadedSettings.LastHighscoreName;

        }
        else
        {
            SaveSettingsToFile();
        }
        

        
    }

    private void SaveSettingsToFile()
    {
        

        string jsonstring = JsonUtility.ToJson((OptionsSettings)this);
        File.WriteAllText(path,jsonstring);

        
    }
}
