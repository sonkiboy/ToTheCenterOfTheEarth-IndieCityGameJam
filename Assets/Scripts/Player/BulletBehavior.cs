using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public AK.Wwise.Event HitSound;
    public AK.Wwise.Event EnemyHitsound;

    public bool IsEnemyBullet = false;

    public int ExtraDamage = 0;
    private Rigidbody2D rb;
    public float Speed = 1;
    public int Damage = 5;
    public float DestroyTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Lifetime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {

        Vector2 newPos = (rb.position - (Vector2)rb.transform.TransformPoint(Vector2.right)).normalized * Speed * Time.deltaTime;
        rb.MovePosition(rb.position + newPos);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!IsEnemyBullet)
        {
            //Debug.Log($"Detected collision : {collision.gameObject.name}");

            GameManager.Instance.SoundManager.PlaySoundOnObject("BulletHit", this.gameObject);

            TileBehavior tb = collision.gameObject.GetComponent<TileBehavior>();
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();


            if (tb != null)
            {
                tb.Health -= Damage + ExtraDamage;
            }
            else if (enemy != null)
            {
                GameManager.Instance.SoundManager.PlaySoundOnObject("EnemyHit", enemy.gameObject);

                enemy.Health -= (Damage);
            }

            Destroy(gameObject);
        }
            

        

    }

    IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(this.gameObject);
    }

}
