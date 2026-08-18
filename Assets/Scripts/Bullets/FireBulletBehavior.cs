using UnityEngine;

public class FireBulletBehavior : Bullet
{

    Rigidbody2D rb;
    public float Speed = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Lifetime());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {

        Vector2 newPos = ((Vector2)rb.transform.TransformPoint(Vector2.right) - rb.position).normalized * Speed * Time.deltaTime;
        rb.MovePosition(rb.position + newPos);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            GameManager.Instance.Player.TakeDamage(1);
            Destroy(gameObject);
        }
        else
        {


            //Debug.Log($"Detected collision : {collision.gameObject.name}");

            GameManager.Instance.SoundManager.PlaySoundOnObject("BulletHit", this.gameObject);

            TileBehavior tb = collision.gameObject.GetComponent<TileBehavior>();
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();


            if (tb != null)
            {

                RootedTileMod root = tb.GetComponent<RootedTileMod>();
                if (root != null)
                {
                    root.IgniteRoot();
                }

            }


        }



    }
}
