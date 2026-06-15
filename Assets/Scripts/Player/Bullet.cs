using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    
    public int Damage = 5;
    public float DestroyTime = 5f;
    public int ExtraDamage = 0;
    protected GunBehavior Gun;

    // Bullet class does awake, derived classes use Start for intiation 
    private void Awake()
    {
        if(GameManager.Instance != null)
        {
            Gun = GameManager.Instance.Player.Gun;

        }
        else
        {
            Gun = GameObject.FindGameObjectWithTag("Player").transform.Find("Gun").GetComponent<GunBehavior>();
        }
    }

    
    protected IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(this.gameObject);
    }
}
