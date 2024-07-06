using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    // Start is called before the first frame update

    public StatTracker StatTracker;


    private int level = 0;
    public int CurrentDepth
    {
        get { return level; }
        set
        {
            level = value;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
