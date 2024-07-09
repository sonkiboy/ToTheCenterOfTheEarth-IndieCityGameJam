using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{

    #region Obj and Compo

    Animator animator;
    SpriteRenderer spriteRenderer;

    #endregion 

    public float Durration = 10f;
    public int DamageUp = 0;
    public float GunSpeedDown = 0;
    public int DrillSpeed = 0;
    public int HealDamage = 0;

    bool isCollected = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isCollected)
        {
            StartCoroutine(UsePower());
        }
    }

    IEnumerator UsePower()
    {
        isCollected = true;

        animator.enabled = false;
        spriteRenderer.enabled = false;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        GunBehavior gun = playerObj.transform.GetComponentInChildren<GunBehavior>(true);

        gun.AdditionalDamage += DamageUp;
        gun.FireSpeed -= GunSpeedDown;
        
        GameManager.Instance.CurrentHealth += HealDamage;

        if(GameManager.Instance.Platform != null)
        {
            GameManager.Instance.Platform.DecentSpeed += DrillSpeed;

        }



        yield return new WaitForSeconds(Durration);

        gun.AdditionalDamage -= DamageUp;
        gun.FireSpeed += GunSpeedDown;
        GameManager.Instance.CurrentHealth -= HealDamage;
        GameManager.Instance.Platform.DecentSpeed += DrillSpeed;

    }
}
