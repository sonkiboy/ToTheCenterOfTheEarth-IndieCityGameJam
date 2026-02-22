using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public InputManager InputManager;
    public PeristentSoundPlayer SoundManager;
    public PaletteManager PaletteManager;
    public BossManager BossManager;
    public OverlayUI Overlay;
    public GameOver GameOver;
    

    

    

    public GameObject CenterScreen;

    

    // ------------ SCENE REFS ------------

    // NEED TO REFRENCE IN EVERY SCENE
    public PlayerController Player;

    // ONLY REFRENCED IN DEFAULT GAMES
    [HideInInspector] public PlatformBehavior Platform;
    [HideInInspector] public Transform ScoreAnchor;
    [HideInInspector] public StatTracker StatTracker;
    [HideInInspector] public HealthTracker HealthTracker;
    


    // ------------ DEPTH LEVEL ------------

    // tracks the level the player is on, USE "CurrentDepth" INSTEAD TO ACCESS
    [SerializeField]private int level = 0;
    public bool IsGameOver = false;

    // used to refrence the depth the player is on
    public int CurrentDepth
    {
        get { return level; }
        set
        {
            // save the passed in value to "level"
            level = value;

            // as the level increases, increase the fuel consumption rate on the platform
            // this will cause the dificulty to slowly increase as the player decends, to a point where the player cannot keep up with the demand
            Platform.FuelPerSecond += .1f;

            // set the UI Level counter to the current Level
            StatTracker.SetLevelScore(level);

            if(level%100 == 0 && level > 10)
            {
                BossManager.StartBossFight(BossManager.Bosses.InsectQueen);
            }

            
            

        }
    }

    public int ActuallDepth = 0;
    

    // ------------ TREASURE ------------

    // tracks the current treasure collected by the player 
    [SerializeField]private int treasure = 0;
    public int CurrentTreasure
    {
        get { return treasure; }
        set
        {
            // save the value to _treasure
            treasure = value;

            // set the Treasure UI counter to the current score
            if(StatTracker !=null) StatTracker.SetTreasureScore(treasure);

        }
    }

    // ------------ HEALTH ------------

    // current health of the player
    [SerializeField]private int health = 3;
    public int CurrentHealth
    {
        get { return health; }
        set
        {
            // if the game isn't already over, we can change the health of the player
            if (!IsGameOver)
            {
                // if the health is decreasing (player took damage) then give them invincibility
                if(health > value)
                {
                    StartCoroutine(Player.Invincibility(Player.InvincibilityDurration));
                }

                // save the value to _health
                health = value;

                // if health is 0 or below, call for a game over
                if (health <= 0)
                {
                    EndGame();
                }

                


                // set the health UI to the current health
                if (HealthTracker != null)
                {
                    HealthTracker.SetHealthUi(health);
                }
            }
        }
    }

    // ------------ GAME STATE ------------ 

    public enum GameStates
    {
        Menu,
        RegularGame,
        GameOver,
        Boss

    }

    [SerializeField]private GameStates _state;
    public GameStates CurrentState
    {
        get { return _state; }
        set
        {
            if (_state != value)
            {
                _state = value;

                switch (_state)
                {
                    case GameStates.Menu:
                        GameManager.Instance.SoundManager.PlayNonDiageticSound("MainThemeStop");
                        GameManager.Instance.SoundManager.PlayNonDiageticSound("BossThemeStop");


                        break;

                    case GameStates.RegularGame:
                        GameManager.Instance.SoundManager.PlayNonDiageticSound("BossThemeStop");
                        break;

                    case GameStates.GameOver:
                        EndGame();
                        break;

                    case GameStates.Boss:
                        GameManager.Instance.SoundManager.PlayNonDiageticSound("MainThemeStop");
                        GameManager.Instance.SoundManager.PlayNonDiageticSound("BossThemeStart");
                        break;
                }


                if (StateChanged != null)
                {
                    StateChanged.Invoke(this,_state);
                }
            }
        }
    }

    public event EventHandler<GameStates> StateChanged;

    private void Awake()
    {
        // if there isn't a game manager already saved as the Instance, save this as the Gamemanager of the session
        if (GameManager.Instance == null)
        {
            GameManager.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        // if there already is a game manager found, then this is a duplicate that needs to be destroyed
        else
        {
            Debug.Log("Previous GameManager found, deleting scene duplicate");
            Destroy(this.gameObject);

            return;
        }

        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    

    void Start()
    {
        // TO DO: make this change based on the starting Game State
        //GameManager.Instance.SoundManager.PlayNonDiageticSound("MainMusic");

        //OnSceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Starts the GameOver sequence 
    private void EndGame()
    {
        CurrentState = GameStates.GameOver;


        // set the Game State to Game Over
        IsGameOver = true;

        // stop the main game music
        GameManager.Instance.SoundManager.PlayNonDiageticSound("MainThemeStop");
        GameManager.Instance.SoundManager.PlayNonDiageticSound("BossThemeStop");


        GameOver.StartGameOver(CurrentTreasure);

        Player.CurrentState = PlayerController.PlayerState.Dead;

        


    }

    public void OnSceneChanged(Scene Current, Scene Next)
    {

        switch (Next.name)
        {
            case ("RegularGame"):
                CurrentState = GameStates.RegularGame;

                Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

                Platform = GameObject.FindGameObjectWithTag("Platform").GetComponent<PlatformBehavior>();
                StatTracker = GameObject.FindAnyObjectByType<StatTracker>();
                HealthTracker = GameObject.FindAnyObjectByType<HealthTracker>();

                ScoreAnchor = StatTracker.gameObject.transform.Find("ScoreAnchor");
                Overlay = GameObject.FindAnyObjectByType<OverlayUI>();
                
                BossManager = GameObject.Find("Cavern").GetComponent<BossManager>();

                StartNormalGame();

                break;

            case ("MainMenu"):
                CurrentState = GameStates.Menu;

                Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                ScoreAnchor = Player.transform;
                ScoreAnchor = Player.transform;

                GameManager.Instance.SoundManager.PlayNonDiageticSound("EndThemeStart");

                break;

            default:

                CurrentState = GameStates.RegularGame;
                Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();


                break;


        

            
        }
    }

    public void StartBossFight(GameObject boss)
    {
        CurrentState = GameStates.Boss;

        
    }

    public void StartNormalGame()
    {
        CurrentState = GameStates.RegularGame;

        
        StartCoroutine(StartGameSequence());

    }

    IEnumerator StartGameSequence()
    {
        yield return null;
        GameManager.Instance.Player.enabled = false;

        CurrentTreasure = 0;
        CurrentDepth = 0;
        ActuallDepth = 0;
        IsGameOver = false;
        CurrentHealth = 3;
        
        
        Overlay.StartCountdown();
        yield return new WaitForSeconds(3);
        GameManager.Instance.Player.enabled = true;

        Platform.StartDrilling();

        yield return new WaitForSeconds(7);
        Overlay.ShowLevelTitle(1);
        
    }
}
