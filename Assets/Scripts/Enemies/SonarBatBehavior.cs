using System.Collections;
using UnityEngine;

public class SonarBatBehavior : Enemy
{
    FlyingEnemy flyingComp;
    Animator animator;

    public GameObject BulletPrefab;
    public float ChargeTime = .5f;
    public float CoolDownTime = 3f; 
    public float FireRate = 1f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flyingComp = GetComponent<FlyingEnemy>();
        animator = GetComponent<Animator>();
        StartCoroutine(FireRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FireRoutine()
    {
        while (isActiveAndEnabled)
        {
            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(CoolDownTime);
            animator.SetTrigger("Charge");
            flyingComp.enabled = false;
            yield return new WaitForSeconds(ChargeTime);
            animator.SetTrigger("Fire");

            Instantiate(BulletPrefab, this.transform.position, Quaternion.LookRotation(Vector3.forward, (flyingComp.Target.transform.position - this.transform.position).normalized) * Quaternion.Euler(0,0,90));
            yield return new WaitForSeconds(FireRate);
            Instantiate(BulletPrefab, this.transform.position, Quaternion.LookRotation(Vector3.forward, (flyingComp.Target.transform.position - this.transform.position).normalized) * Quaternion.Euler(0,0,90));
            yield return new WaitForSeconds(FireRate);
            Instantiate(BulletPrefab, this.transform.position, Quaternion.LookRotation(Vector3.forward, (flyingComp.Target.transform.position - this.transform.position).normalized) * Quaternion.Euler(0,0,90));
            yield return new WaitForSeconds(FireRate);

            yield return new WaitForSeconds(ChargeTime/2);
            flyingComp.enabled = true;

            //animator.SetTrigger("Idle");
        }
        yield return null;
    }
}
