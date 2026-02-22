using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffectDamage : MonoBehaviour
{

    Collider2D colliderBody;

    public int Damage = 1;
    public float DamageCoolDown = 1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderBody = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DealDamage());
    }

    
    IEnumerator DealDamage()
    {
        while (true)
        {
            // look for any objects within the drill collider that match it's layer mask
            Collider2D[] foundCollisions = Physics2D.OverlapBoxAll(colliderBody.bounds.center, colliderBody.bounds.size, 0f, ~colliderBody.excludeLayers);

            if (foundCollisions.Length != 0)
            {
                foreach(Collider2D collision in foundCollisions)
                {
                    if(collision.tag == "Player")
                    {
                        collision.GetComponent<PlayerController>().TakeDamage(1);
                    }
                }
            }


            yield return new WaitForSeconds(DamageCoolDown);
        }
    }
}
