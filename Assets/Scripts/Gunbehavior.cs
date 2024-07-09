using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    public GameObject Bullet;

    public float FireSpeed = .5f;

    public int AdditionalDamage = 0;

    private bool canFire = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        if (canFire)
        {
            GameObject bullet = Instantiate(Bullet,this.transform.position,this.transform.rotation);
            bullet.GetComponent<BulletBehavior>().ExtraDamage = AdditionalDamage;

            StartCoroutine(FireCoolDown());
        }
    }

    IEnumerator FireCoolDown()
    {
        canFire = false;
        yield return new WaitForSeconds(FireSpeed);
        canFire = true;
    }


}
