using UnityEngine;

public class GunSwapper : MonoBehaviour
{

    public GunSettings Gun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            this.gameObject.SetActive(false);
        }
        else
        {
            this .gameObject.SetActive(true);
        }

        
    }
    void CheckIfAlreadyEquiped(object sender, GunSettings e)
    {
        if (e == Gun)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }
}
