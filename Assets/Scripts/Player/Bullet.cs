using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    
    public int Damage = 5;
    public float DestroyTime = 5f;
    public int ExtraDamage = 0;

    
    protected IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(this.gameObject);
    }
}
