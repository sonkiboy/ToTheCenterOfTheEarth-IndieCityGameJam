using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunBehavior : MonoBehaviour
{
    // Bullet Prefab that is spawned in the Fire() method
    public GameObject Bullet;

    // The Assigned Shoot sound event that plays on each Fire() exectution
    public AK.Wwise.Event ShootEvent;

    // the rate of fire for this gun
    public float FireSpeed = .5f;

    // additional damage that bullets fired from this gun deal
    public int AdditionalDamage = 0;

    // tracks whether or not the gun is currently firing 
    private bool isFiring = false;

    // Start is called before the first frame update
    void Start()
    {
        // subscribe this guns StartFiring method do the Fire Input Performed event in the Input Manager
        GameManager.Instance.InputManager.FireInput.performed += StartFiring;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // When the player presses the fire input and the player isn't already firing, start this guns fire routine
    void StartFiring(InputAction.CallbackContext context)
    {
        // while the Fire routines check isFiring at their start, this helps prevent the start of extra coroutines
        if (isFiring == false && GameManager.Instance.Player.CurrentState != PlayerController.PlayerState.Dead)
        {
            StartCoroutine(RapidFireRoutine());
        }
    }

    // Rapid fire gun routine will fire bullets at a consistent rate based on the Fire Speed
    IEnumerator RapidFireRoutine()
    {
        // check if the player isn't already firing, this should be checked before the routine is started to prevent excess coroutines being called, this check is a fail safe
        if (isFiring == false)
        {
            // set isFiring to true
            isFiring = true;

            // while the fire button is pressed, fire a bullet ever number of seconds equal to Fire Speed
            while (GameManager.Instance.InputManager.FireInput.IsPressed())
            {
                Fire();
                yield return new WaitForSeconds(FireSpeed);
            }

            // once the button is released, set is firing to false
            isFiring = false;
            yield return null;
        }
    }

    // Spawns a bullet facing the same direction as the gun, apply any additonal damage this gun gives its bullets, and play the fire sound
    public void Fire()
    {
        GameObject bullet = Instantiate(Bullet, this.transform.position, this.transform.rotation);
        GameManager.Instance.SoundManager.PlaySoundOnObject("PlayerFire",this.gameObject);
        bullet.GetComponent<BulletBehavior>().ExtraDamage = AdditionalDamage;
    }




}
