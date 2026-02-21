using System.Collections;
using UnityEngine;

public class FireBulletTile : MonoBehaviour
{


    public GameObject BulletPrefab;
    public Animator TileAnimator;
    public float FireTime = 4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(RunFire());
    }

    IEnumerator RunFire()
    {
        float chargeAnimTime = 1f;

        while (true)
        {
            yield return new WaitForSeconds(FireTime - chargeAnimTime);

            TileAnimator.SetTrigger("StartCharge");

            yield return new WaitForSeconds(chargeAnimTime);

            

            yield return null;
        }
    }
}
