using UnityEngine;

public class GunSwapper : MonoBehaviour
{

    public GunSettings Gun;


    private GameObject Bubble;
    private GameObject EmptyBubble;

    private Animator BubbleAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Bubble = transform.Find("Bubble").gameObject;
        EmptyBubble = transform.Find("EmptyBubble").gameObject;

        BubbleAnimator = GetComponent<Animator>();
        BubbleAnimator.Play(0,-1, Random.Range(0f, 1f));

        CheckIfAlreadyEquiped();
        GameManager.Instance.OnGunChanged += CheckIfAlreadyEquiped;
    }

    private void OnDisable()
    {

    }

    private void OnEnable()
    {

    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGunChanged -= CheckIfAlreadyEquiped;

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.Instance.CurrentGun = Gun;
            CheckIfAlreadyEquiped() ;
        }
    }

    void CheckIfAlreadyEquiped()
    {
        if(GameManager.Instance.CurrentGun == Gun)
        {
            Bubble.SetActive(false);
            EmptyBubble.SetActive(true);

        }
        else
        {
            Bubble.SetActive(true);
            EmptyBubble.SetActive(false);

        }


    }
    void CheckIfAlreadyEquiped(object sender, GunSettings e)
    {
        if (e == Gun)
        {
            Bubble.SetActive(false);
            EmptyBubble.SetActive(true);

        }
        else
        {
            Bubble.SetActive(true);
            EmptyBubble.SetActive(false);

        }
    }
}
