using UnityEngine;

public class RewardChestBehavior : Enemy
{

    public GameObject SmallGemPrefab;
    public GameObject BigGemPrefab;

    public int SmallGemsOnHit = 3;
    public float SmallGemFlingForce = 4f;
    public int BigGemsOnOpen = 5;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnTakenDamage()
    {
        GameObject spawnedGem;
        Rigidbody2D gemRb;
        Vector2 randDir;
        for (int i = 0; i < SmallGemsOnHit; i++)
        {
            spawnedGem = Instantiate(SmallGemPrefab, this.transform.position, Quaternion.identity);
            gemRb = spawnedGem.GetComponent<Rigidbody2D>();
            randDir = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            gemRb.AddForce(randDir * SmallGemFlingForce, ForceMode2D.Impulse);

            spawnedGem = null;
            gemRb = null;
            randDir = Vector2.zero;
        }
        
    }

    public override void Die()
    {
        for (int i = 0;i < BigGemsOnOpen;i++)
        {
            Instantiate(BigGemPrefab, this.transform.position + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f),0), Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}
