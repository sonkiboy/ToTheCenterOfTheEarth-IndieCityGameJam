using System.Collections;
using UnityEngine;

public class FireFlyBehavior : Enemy
{
    FlyingEnemy flyingComp;
    Animator animator;

    public GameObject BulletPrefab;
    public float ChargeTime = .5f;
    public float CoolDownTime = 3f;

    public float FanAngle = 15f;

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

            Vector3 dir = (flyingComp.Target.transform.position - this.transform.position).normalized;
            Instantiate(BulletPrefab, this.transform.position, Quaternion.LookRotation(Vector3.forward, dir) * Quaternion.Euler(0, 0, 90));
            Instantiate(BulletPrefab, this.transform.position, Quaternion.LookRotation(Vector3.forward, dir) * Quaternion.Euler(0, 0, 90 - FanAngle));
            Instantiate(BulletPrefab, this.transform.position, Quaternion.LookRotation(Vector3.forward, dir) * Quaternion.Euler(0, 0, 90 + FanAngle));
            

            yield return new WaitForSeconds(ChargeTime / 2);
            flyingComp.enabled = true;

            //animator.SetTrigger("Idle");
        }
        yield return null;
    }
}
