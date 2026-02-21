using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthTracker : MonoBehaviour
{
    public Sprite[] Icons;
    private Image[] Hearts;


    // Start is called before the first frame update
    void Start()
    {
        Hearts = new Image[transform.childCount];
        for (int i = 0; i < Hearts.Length; i++)
        {
            Hearts[i] = transform.GetChild(i).GetComponent<Image>();
        }

        
    }

    public void SetHealthUi(int health)
    {
        for (int i = 0; i < Hearts.Length; i++)
        {
            if(i < health)
            {
                Hearts[i].sprite = Icons[1];

            }
            else
            {
                Hearts[i].sprite = Icons[0];
            }
            
        }


    }

    
}
