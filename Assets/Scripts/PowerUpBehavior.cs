using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{

    #region Obj and Compo

    Animator animator;
    SpriteRenderer spriteRenderer;

    #endregion 

    

    public PowerUpManager.PowerUpTypes Type;
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

        switch (Type)
        {
            case PowerUpManager.PowerUpTypes.Reload:

                GameManager.Instance.PowerManager.ReloadPowerCount++;

                break;

            case PowerUpManager.PowerUpTypes.DrillSpeed:

                GameManager.Instance.PowerManager.DrillPowerCount++;


                break;

            case PowerUpManager.PowerUpTypes.Damage:

                GameManager.Instance.PowerManager.DamagePowerCount++;
                break;


        }

        GameManager.Instance.SoundManager.PlayNonDiageticSound("TreasureCollected");

        GameManager.Instance.PopUpManager.DisplayPowerPopUp(Type, this.transform.position);

        animator.enabled = false;
        spriteRenderer.enabled = false;
        yield return null;
        Destroy(this.gameObject);
        //GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        //GunBehavior gun = playerObj.transform.GetComponentInChildren<GunBehavior>(true);

        //gun.AdditionalDamage += DamageUp;
        //gun.ReloadModifier += GunSpeedDown;
        
        //GameManager.Instance.CurrentHealth += HealDamage;

        //if(GameManager.Instance.Platform != null)
        //{
        //    GameManager.Instance.Platform.DecentSpeed += DrillSpeed;

        //}



        //yield return new WaitForSeconds(Durration);

        //gun.AdditionalDamage -= DamageUp;
        //gun.ReloadModifier -= GunSpeedDown;
        //GameManager.Instance.CurrentHealth -= HealDamage;
        //if( GameManager.Instance.Platform != null)
        //{
        //    GameManager.Instance.Platform.DecentSpeed += DrillSpeed;

        //}

    }
}
