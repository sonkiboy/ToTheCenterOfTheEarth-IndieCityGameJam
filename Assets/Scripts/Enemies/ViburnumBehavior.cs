using System.Collections;
using UnityEngine;

public class ViburnumBehavior : Enemy
{
    private Transform head;
    private Animator animator;
    public GameObject PetalBulletPrefab;
    public int ShotsPerCycle = 3;
    public float FireRate = .75f;
    public float CoolDownTime = 3f;

    int startingHP;
    bool isEnranged = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.parent = null;
        head = transform.Find("Head");
        startingHP = Health;
        animator = head.GetComponent<Animator>();
        StartCoroutine(AttackRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if(isEnranged ==  false && Health < startingHP / 2)
        {
            isEnranged = true;
            animator.SetBool("IsAngry",true);
        }
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(FireRate);

        while (isActiveAndEnabled)
        {


            for (int i = 0; i < ShotsPerCycle; i++)
            {
                if (isEnranged) FireBurstPetal();
                else FireOnePetal();

                yield return new WaitForSeconds(FireRate);

            }

            yield return new WaitForSeconds(CoolDownTime);
            yield return null;
        }
        yield return null;
    }

    private void FireOnePetal()
    {
        Instantiate(PetalBulletPrefab,this.transform.position,head.rotation * Quaternion.Euler(0,0,90));
    }

    private void FireBurstPetal()
    {
        float angle = 30f;

        Instantiate(PetalBulletPrefab, this.transform.position, head.rotation * Quaternion.Euler(0, 0, 90));
        Instantiate(PetalBulletPrefab, this.transform.position, head.rotation * Quaternion.Euler(0, 0, 90 + angle));
        Instantiate(PetalBulletPrefab, this.transform.position, head.rotation * Quaternion.Euler(0, 0, 90 - angle));

    }
}
