using UnityEngine;

public class GunInfoDisplay : MonoBehaviour
{

    [SerializeField] GameObject pistolText;
    [SerializeField] GameObject sniperText;
    [SerializeField] GameObject laserText;
    [SerializeField] SpriteRenderer CurrentGunSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnGunChanged += OnGunChanges;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnGunChanged -= OnGunChanges;

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGunChanges(object sender, GunSettings settings)
    {

        CurrentGunSprite.sprite = settings.GunSprite;
        switch (settings.name)
        {

            
            case "Pistol":

                laserText.SetActive(false);
                sniperText.SetActive(false);
                pistolText.SetActive(true);

                break;
            case "Sniper":

                laserText.SetActive(false);
                sniperText.SetActive(true);
                pistolText.SetActive(false);

                break;
            case "Beam":

                laserText.SetActive(true);
                sniperText.SetActive(false);
                pistolText.SetActive(false);

                break;
        }
    }
}
