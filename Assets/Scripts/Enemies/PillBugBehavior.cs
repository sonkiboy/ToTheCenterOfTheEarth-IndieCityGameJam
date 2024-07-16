using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillBugBehavior : MonoBehaviour, IEnemy
{

    #region Obj and Components

    Rigidbody2D rb;
    [SerializeField] BoxCollider2D wallCheck;

    Renderer damageFlash;

    #endregion

    int hp = 20;
    public int Health
    {
        get { return hp; }
        set 
        { 
            hp = value;

            if(hp <= 0)
            {
                StartCoroutine(Die());
            }
        }
    }

    [Range(0f, 5f)]
    public float Speed = .15f;

    public int TreasureReward = 10;
    public int FuelReward = 15;

    Vector2 currentDirection = Vector2.left;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        damageFlash = transform.Find("Sprite").GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 newVel = new Vector2(currentDirection.x * Speed, rb.velocity.y);
        rb.velocity = newVel;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D foundWall = Physics2D.OverlapBox(wallCheck.bounds.center, wallCheck.size, 0f, wallCheck.includeLayers);

        if(foundWall != null)
        {

            currentDirection = -currentDirection;

            if (currentDirection.x > 0 )
            {
                gameObject.transform.rotation = Quaternion.Euler(Vector3.up * 180);
            }
            else if(currentDirection.x < 0)
            {
                gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            
            

        }
    }

    public void DealDamage(int Damage)
    {
        this.Health -= Damage;
        StartCoroutine(DamageFlash(.1f));
    }

    public IEnumerator Die()
    {

        GameManager.Instance.CurrentTreasure += TreasureReward;
        GameManager.Instance.Platform.CurrentFuel += FuelReward;

        yield return new WaitForFixedUpdate();
        Destroy(this.gameObject);
    }

    IEnumerator DamageFlash(float durration)
    {
        damageFlash.material.SetFloat("_Intensity", 1f);

        yield return new WaitForSeconds(durration);

        damageFlash.material.SetFloat("_Intensity", 0f);

    }
}
