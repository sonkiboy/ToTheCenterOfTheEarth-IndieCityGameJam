using UnityEngine;

public class StartGameCollider : MonoBehaviour
{

    public StartSlideShow SlideShow;

    bool gameStarted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameStarted == false)
        {
            gameStarted = true;
            SlideShow.StartGame();

        }
    }
    
}
