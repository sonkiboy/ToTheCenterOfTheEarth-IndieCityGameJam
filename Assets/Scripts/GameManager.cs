using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    // Start is called before the first frame update

    public StatTracker StatTracker;
    public HealthTracker HealthTracker;

    public PlatformBehavior Platform;

    public GameObject CenterScreen;

    public AK.Wwise.Event PlayerDamagedSound;
    public AK.Wwise.Event MainMusicOn;
    public AK.Wwise.Event MainMusicOff;


    private int level = 0;
    private bool isGameOver = false;
    public int CurrentDepth
    {
        get { return level; }
        set
        {
            level = value;

            Platform.FuelPerSecond += .1f;

            StatTracker.SetLevelScore(level);

        }
    }

    private int treasure = 0;
    public int CurrentTreasure
    {
        get { return treasure; }
        set
        {
            treasure = value;
            StatTracker.SetTreasureScore(treasure);
        }
    }

    private int health = 3;
    public int CurrentHealth
    {
        get { return health; }
        set
        {
            if (!isGameOver)
            {

                health = value;

                if (health <= 0)
                {
                    GameOver();
                }


                PlayerDamagedSound.Post(CenterScreen);

                HealthTracker.SetHealthUi(health);
            }
        }
    }



    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            GameManager.Instance = this;
        }
        else
        {
            Debug.Log("Previous GameManager found, deleting scene duplicate");

            Instantiate(GameManager.Instance.gameObject);
            Destroy(this.gameObject);
        }
    }


    void Start()
    {
        MainMusicOn.Post(CenterScreen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GameOver()
    {

        isGameOver = true;  
        MainMusicOff.Post(CenterScreen);

        GetComponent<GameOver>().StartGameOver(CurrentTreasure);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        player.transform.Find("Sprite").GetComponent<Renderer>().material.SetFloat("_Intensity", 1f);
        Platform.enabled = (false);


    }
}
