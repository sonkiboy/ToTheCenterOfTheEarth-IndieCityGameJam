using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    public GameObject Bullet;

    public float FireSpeed = .5f;

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
            Instantiate(Bullet,this.transform.position,this.transform.rotation);

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
