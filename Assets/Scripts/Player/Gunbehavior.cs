using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GunBehavior : MonoBehaviour
{
    public GunSettings Settings;
    SpriteRenderer spriteRenderer;


    // additional damage that bullets fired from this gun deal
    public int AdditionalDamage = 0;
    public int AdditionalPierceCount = 0;
    public float ReloadModifier = 0f;

    // tracks whether or not the gun is currently firing 
    private bool isFiring = false;

    public float BulletSpawnOffset = 1f;



    // Start is called before the first frame update
    void Start()
    {
        Settings = GameManager.Instance.CurrentGun;
        // subscribe this guns StartFiring method do the Fire Input Performed event in the Input Manager
        UpdateInputs(this, GameManager.Instance.GameOptions);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Settings.GunSprite;
        GameManager.Instance.GameOptions.OnSettingsChanged += UpdateInputs;
        GameManager.Instance.InputManager.FireInput.performed += StartFiring;
        GameManager.Instance.OnGunChanged += OnGunChanged;
    }

    private void OnEnable()
    {
        // subscribe this guns StartFiring method do the Fire Input Performed event in the Input Manager
        try { UpdateInputs(this, GameManager.Instance.GameOptions); }
        catch { }

    }

    private void OnDisable()
    {
        // subscribe this guns StartFiring method do the Fire Input Performed event in the Input Manager
        GameManager.Instance.InputManager.FireInput.performed -= StartFiring;
        GameManager.Instance.InputManager.LookInput.performed -= StartFiring;


    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGunChanged -= OnGunChanged;


    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameOptions.IsFireOnAim)
        {
            if (isFiring == false && GameManager.Instance.Player.CurrentState != PlayerController.PlayerState.Dead && GameManager.Instance.Player.enabled == true)
            {
                if (GameManager.Instance.InputManager.LookInput.ReadValue<Vector2>() != Vector2.zero)
                {
                    StartFiring(new InputAction.CallbackContext());
                }
            }
        }
    }

    public void UpdateInputs(object sender, OptionsSettings settings)
    {

        //GameManager.Instance.InputManager.FireInput.performed += StartFiring;

        //if (GameManager.Instance.GameOptions.IsFireOnAim)
        //{
        //    GameManager.Instance.InputManager.LookInput.performed += StartFiring;

        //}
        //else
        //{
        //    GameManager.Instance.InputManager.LookInput.performed -= StartFiring;

        //}
    }

    // When the player presses the fire input and the player isn't already firing, start this guns fire routine
    void StartFiring(InputAction.CallbackContext context)
    {
        Debug.Log($"Input read as {context.action}");
        // while the Fire routines check isFiring at their start, this helps prevent the start of extra coroutines
        if (isFiring == false && GameManager.Instance.Player.CurrentState != PlayerController.PlayerState.Dead && GameManager.Instance.Player.enabled == true)
        {
            switch (Settings.Mode)
            {
                case (GunSettings.BulletMode.Rapid):

                    StartCoroutine(RapidFireRoutine());

                    break;

                case (GunSettings.BulletMode.Singular):

                    StartCoroutine(BeamFireRoutine());
                    break;

            }
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
            if (GameManager.Instance.GameOptions.IsFireOnAim)
            {
                while ((GameManager.Instance.InputManager.FireInput.IsPressed() || GameManager.Instance.InputManager.LookInput.IsInProgress()) && Settings.Mode == GunSettings.BulletMode.Rapid)
                {
                    Fire();
                    yield return new WaitForSeconds(Settings.FireSpeed - (Settings.FireSpeed * (GameManager.Instance.PowerManager.ReloadSpeedDown * GameManager.Instance.PowerManager.ReloadPowerCount)));
                    yield return new WaitForSeconds(Settings.FireSpeed * .05f);
                }
            }
            else
            {
                while (GameManager.Instance.InputManager.FireInput.IsPressed() && Settings.Mode == GunSettings.BulletMode.Rapid)
                {
                    Fire();
                    yield return new WaitForSeconds(Settings.FireSpeed - (Settings.FireSpeed * (GameManager.Instance.PowerManager.ReloadSpeedDown * GameManager.Instance.PowerManager.ReloadPowerCount)));
                    yield return new WaitForSeconds(Settings.FireSpeed * .05f);
                }
            }
            

            // once the button is released, set is firing to false
            isFiring = false;
            yield return null;
        }
    }

    // Spawns a bullet facing the same direction as the gun, apply any additonal damage this gun gives its bullets, and play the fire sound
    public void Fire()
    {
        GameObject bullet = Instantiate(Settings.Bullet, this.transform.position + (this.transform.right * BulletSpawnOffset), this.transform.rotation);
        GameManager.Instance.SoundManager.PlaySoundOnObject("PlayerShoot", this.gameObject);
        bullet.GetComponent<Bullet>().ExtraDamage = AdditionalDamage;
    }

    IEnumerator BeamFireRoutine()
    {

        if (isFiring == false)
        {
            isFiring = true;

            GameObject instantiated = Instantiate(Settings.Bullet, this.transform.position + (this.transform.right * BulletSpawnOffset), this.transform.rotation, this.transform);

            // while the fire button is pressed
            while (GameManager.Instance.InputManager.FireInput.IsPressed() && Settings.Mode == GunSettings.BulletMode.Singular)
            {
                yield return new WaitForSeconds(Settings.FireSpeed);
            }

            Destroy(instantiated);



            isFiring = false;
        }
    }
    public void OnGunChanged(object sender, GunSettings e)
    {
        Debug.Log("Gun Changed Called");
        if (Settings != e)
        {
            Settings = e;
            spriteRenderer.sprite = e.GunSprite;

            StopCoroutine(RapidFireRoutine());
            StopCoroutine(BeamFireRoutine());
        }

    }

}
