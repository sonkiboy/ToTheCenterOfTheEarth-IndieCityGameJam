using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy: MonoBehaviour
{
    [SerializeField] protected Renderer[] damageFlash;

    public int TreasureReward = 10;
    public int FuelReward = 15;
    public bool CollisionDamage = true;

    protected bool isDead = false;
    [SerializeField] int hp = 20;
    public int Health
    {
        get { return hp; }
        set
        {
            // if the hp is decreasing, call the method for any On Damage Taken behavior and flash for damage
            if(value < hp)
            {
                StartCoroutine(DamageFlash(.1f));
                OnTakenDamage();
            }

            // save the new value 
            hp = value;

            
            

            if (hp <= 0 && isDead == false)
            {
                GameManager.Instance.CurrentTreasure += TreasureReward;
                GameManager.Instance.Platform.CurrentFuel += FuelReward;

                isDead = true;

                

                Die();
            }
        }
    }

    public event EventHandler<Enemy> OnEnemyDeath;

    public void Awake()
    {
        GameManager.Instance.StateChanged += OnGameOver;
    }

    IEnumerator DamageFlash(float durration)
    {

        foreach(Renderer rend  in damageFlash)
        {
            rend.material.SetFloat("_Intensity", 1f);
        }

        yield return new WaitForSeconds(durration);

        foreach (Renderer rend in damageFlash)
        {
            rend.material.SetFloat("_Intensity", 0f);
        }
    }

    public virtual void Die()
    {
        if (OnEnemyDeath != null)
        {
            OnEnemyDeath.Invoke(this, this);
        }
        Destroy(this.gameObject);

    }

    public virtual void OnGameOver(object sender, GameManager.GameStates e)
    {
        if(e == GameManager.GameStates.GameOver)
        {
            Destroy(this.gameObject);

        }
    }

    public virtual void OnTakenDamage()
    {

    }
}
