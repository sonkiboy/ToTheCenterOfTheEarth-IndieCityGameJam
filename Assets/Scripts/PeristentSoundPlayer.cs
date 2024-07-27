using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PeristentSoundPlayer : MonoBehaviour
{
    
    
    public static PeristentSoundPlayer instance;

    public AK.Wwise.Event GameOverSound;

    bool isPlayingEnd = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (SceneManager.GetActiveScene().name == "IntroScene" && !isPlayingEnd)
            {
                PlayEnd();
            }
        }
        else
        {
            Debug.Log("Other persistent sound player found, deleting new one");
            Destroy(this.gameObject);
        }

        
    }

    private void Update()
    {
        transform.position = Camera.main.transform.position;
    }

    public void PlayEnd()
    {
        Debug.Log("Starting end music");
        isPlayingEnd = true;

        GameOverSound.Post(this.gameObject);

    }

    public void StopEnd()
    {
        Debug.Log("Stopping end music");
        isPlayingEnd = false;
        AkSoundEngine.StopAll(this.gameObject);

    }


}
