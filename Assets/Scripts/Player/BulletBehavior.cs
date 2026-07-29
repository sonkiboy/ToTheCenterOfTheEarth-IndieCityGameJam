using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : Bullet
{
    public AK.Wwise.Event HitSound;
    public AK.Wwise.Event EnemyHitsound;

    public bool IsEnemyBullet = false;

    
    [SerializeField]private Rigidbody2D rb;
    public float Speed = 1;
    

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

        Vector2 newPos = ( (Vector2)rb.transform.TransformPoint(Vector2.right) - rb.position).normalized * Speed * Time.deltaTime;
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
                if(GameManager.Instance.PowerManager != null) tb.Health -= Damage + ExtraDamage + (GameManager.Instance.PowerManager.DamageUp * GameManager.Instance.PowerManager.DamagePowerCount);
                else tb.Health -= Damage + ExtraDamage;

            }
            else if (enemy != null)
            {
                GameManager.Instance.SoundManager.PlaySoundOnObject("EnemyHit", enemy.gameObject);

                enemy.Health -= (Damage);
            }

            Destroy(gameObject);
        }
            

        

    }

    

}
