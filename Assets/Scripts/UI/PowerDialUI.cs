using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerDialUI : MonoBehaviour
{
    Image Dial;
    Image Counter;
    Image plus;

    [SerializeField]
    Sprite[] NumberSprites;

    PowerUpManager powerUpManager;
    private float perc = 0f;
    public float Percentage
    {
        get
        {
            return perc;
        }
        set
        {
            perc = value;

            if (perc > 100) perc = 100;
            else if (perc < 0) perc = 0;

            Dial.fillAmount = perc/100f;
            
        }
    }
    private int cnt = 0;
    public int Count
    {
        get
        {
            return cnt;
        }
        set
        {
            cnt = value;
            if(cnt < 0)
            {
                cnt = 0;
                Counter.sprite = NumberSprites[0];
                if (plus.gameObject.activeSelf == true)
                {
                    plus.gameObject.SetActive(false);
                }
            }
            else if(cnt > 9)
            {
                Counter.sprite = NumberSprites[9];
                if (plus.gameObject.activeSelf == false)
                {
                    plus.gameObject.SetActive(true);
                }
            }
            else
            {
                Counter.sprite = NumberSprites[cnt];
                if (plus.gameObject.activeSelf == true)
                {
                    plus.gameObject.SetActive(false);
                }
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Dial = transform.Find("Dial").GetComponent<Image>();
        Counter = transform.Find("Count").GetComponent<Image>(); 
        plus = transform.Find("Plus").GetComponent<Image>();

        Percentage = 0;
        Count = 0;
    }

    
}
