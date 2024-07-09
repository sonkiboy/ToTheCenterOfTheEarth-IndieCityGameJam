using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public int ExtraDamage = 0;
    private Rigidbody2D rb;
    public float Speed = 1;
    public int Damage = 5;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        Vector2 newPos = (rb.position - (Vector2)rb.transform.TransformPoint(Vector2.right)).normalized * Speed * Time.deltaTime;
        rb.MovePosition(rb.position + newPos);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //Debug.Log($"Detected collision : {collision.gameObject.name}");


        TileBehavior tb = collision.gameObject.GetComponent<TileBehavior>();
        IEnemy enemy = collision.gameObject.GetComponent<IEnemy>();


        if (tb != null)
        {
            tb.Health -= Damage + ExtraDamage;
        }
        else if (enemy != null)
        {
            enemy.DealDamage(Damage);
        }



        Destroy(gameObject);
    }
}
