using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfEffectDamage : MonoBehaviour
{

    Collider2D colliderBody;

    public int Damage = 1;
    public float DamageCoolDown = 1f;

    private List<GameObject> objectsBeingDamaged = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderBody = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (objectsBeingDamaged.Contains(collision.gameObject) == false)
        {
            if (collision.tag == "Player")
            {
                Debug.Log("Player entered lazer beam");
                StartCoroutine(DamagePlayer(collision.gameObject));
            }
        }
        
    }

    IEnumerator DamagePlayer(GameObject player)
    {
        objectsBeingDamaged.Add(player);

        while(colliderBody.bounds.Contains(player.transform.position))
        {
            GameManager.Instance.CurrentHealth--;

            yield return new WaitForSeconds(DamageCoolDown);
        }

        Debug.Log("Player left lazer beam");
        objectsBeingDamaged.Remove(player);

    }
}
