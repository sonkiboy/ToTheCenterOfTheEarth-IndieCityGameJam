using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpManager : MonoBehaviour
{
    public enum PowerUpTypes
    {
        Heal, Damage, DrillSpeed, Reload
    }

    [SerializeField] PowerDialUI dmgDial;
    [SerializeField] PowerDialUI reloadDial;
    [SerializeField] PowerDialUI drillDial;


    // POWER UP VARIABLES
    public int DamagePowerCount = 0;
    public int DamageUp = 20;
    public float DmgUpDurration = 10f;
    private float damageTimeCount = 0;

    public int ReloadPowerCount = 0;
    public float ReloadSpeedDown = 0.2f;
    public float ReloadSpeedDurration = 10f;
    private float reloadTimeCount = 0;


    public int DrillPowerCount = 0;
    public int DrillSpeed = 5;
    public float DrillDurration = 10f;
    private float drillTimeCount = 0;


    public int HealDamage = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ReloadRoutine());
        StartCoroutine(DamageRoutine());
        StartCoroutine(DrillSpeedRoutine());
        SceneManager.activeSceneChanged += OnSceneChanged;
        OnSceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ReloadRoutine()
    {
        while (isActiveAndEnabled)
        {
            float refreshIncriment = .1f;
            if (ReloadPowerCount > 0)
            {
                while (ReloadPowerCount > 0)
                {
                    // if the time count is 0 or less, then we need to start a fresh dial for this power up
                    if (reloadTimeCount <= 0)
                    {
                        reloadTimeCount = ReloadSpeedDurration;
                    }

                    reloadDial.Percentage = (reloadTimeCount / ReloadSpeedDurration) * 100f;

                    yield return new WaitForSeconds(refreshIncriment);
                    reloadTimeCount -= refreshIncriment;

                    // if this loop caused the time to run out, then subtract a power count
                    if (reloadTimeCount <= 0)
                    {
                        ReloadPowerCount--;
                    }
                    reloadDial.Count = ReloadPowerCount;
                }

            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator DamageRoutine()
    {
        while (isActiveAndEnabled)
        {
            float refreshIncriment = .1f;
            if (DamagePowerCount > 0)
            {
                while (DamagePowerCount > 0)
                {
                    // if the time count is 0 or less, then we need to start a fresh dial for this power up
                    if (damageTimeCount <= 0)
                    {
                        damageTimeCount = DmgUpDurration;
                    }

                    dmgDial.Percentage = (damageTimeCount / DmgUpDurration) * 100f;

                    yield return new WaitForSeconds(refreshIncriment);
                    damageTimeCount -= refreshIncriment;

                    // if this loop caused the time to run out, then subtract a power count
                    if (damageTimeCount <= 0)
                    {
                        DamagePowerCount--;
                    }
                    dmgDial.Count = DamagePowerCount;
                }

            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void OnSceneChanged(Scene Current, Scene Next)
    {
        dmgDial = GameObject.Find("DmgPower").GetComponent<PowerDialUI>();
        reloadDial = GameObject.Find("ReloadPower").GetComponent<PowerDialUI>(); ;
        drillDial = GameObject.Find("DrillPower").GetComponent<PowerDialUI>(); ;
    }

    IEnumerator DrillSpeedRoutine()
    {
        while (isActiveAndEnabled)
        {
            float refreshIncriment = .1f;
            if (DrillPowerCount > 0)
            {
                while (DrillPowerCount > 0)
                {
                    // if the time count is 0 or less, then we need to start a fresh dial for this power up
                    if (drillTimeCount <= 0)
                    {
                        drillTimeCount = DrillDurration;
                    }

                    drillDial.Percentage = (drillTimeCount / DrillDurration) * 100f;

                    yield return new WaitForSeconds(refreshIncriment);
                    drillTimeCount -= refreshIncriment;

                    // if this loop caused the time to run out, then subtract a power count
                    if (drillTimeCount <= 0)
                    {
                        DrillPowerCount--;
                    }
                    drillDial.Count = DrillPowerCount;
                }

            }
            yield return new WaitForFixedUpdate();
        }
    }
}
