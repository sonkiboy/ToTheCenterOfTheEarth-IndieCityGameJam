using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LBLineItemUI : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI nameForeTxt;
    [SerializeField] TextMeshProUGUI nameBackTxt;
    [SerializeField] TextMeshProUGUI scoreForeTxt;
    [SerializeField] TextMeshProUGUI scoreBackTxt;

    public string NameText
    {
        set
        {
            nameBackTxt.text = value;
            nameForeTxt.text = value;
        }
    }

    public string ScoreText
    {
        set
        {
            scoreForeTxt.text = value;
            scoreBackTxt.text = value;
        }
    }

    private void Awake()
    {
        //nameForeTxt = transform.Find("Name").transform.Find("ForeText").GetComponent<TextMeshProUGUI>();
        //nameBackTxt = transform.Find("Name").transform.Find("BackText").GetComponent<TextMeshProUGUI>();
        //scoreForeTxt = transform.Find("TreasureNumbers").transform.Find("ForeText").GetComponent<TextMeshProUGUI>();
        //scoreBackTxt = transform.Find("TreasureNumbers").transform.Find("BackText").GetComponent<TextMeshProUGUI>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
