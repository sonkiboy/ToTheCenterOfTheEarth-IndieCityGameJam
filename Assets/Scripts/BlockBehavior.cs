using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehavior : MonoBehaviour
{
    #region Object and Component Refrences

    [SerializeField] SpriteRenderer MainSprite;
    [SerializeField] SpriteRenderer CrackSprite;




    #endregion

    [SerializeField] Sprite[] TypeSprites;

    [SerializeField] Sprite[] DamageSprits;

    public enum RockType
    {
        Normal,
        Fuel,
        Treasure,
    }

    [SerializeField] RockType _type = RockType.Normal;

    public RockType Type
    {
        get { return _type; }

        set
        {
            _type = value;

            switch (_type)
            {
                default:
                    MainSprite.sprite = TypeSprites[0];
                    break;

                case (RockType.Fuel):
                    MainSprite.sprite = TypeSprites[1];
                    break;

                case (RockType.Treasure):
                    MainSprite.sprite = TypeSprites[2];
                    break;
            }

        }
    }

    [SerializeField] float IFrameTime = .5f;
    bool isInvincible = false;

    [SerializeField] int StartingHP = 10;
    int _hp  = 10;
    public int Health
    {
        get { return _hp; }
        set 
        {

            if (!isInvincible)
            {
                _hp = value;

                isInvincible = true;
                StartCoroutine(InvincibleTime());

                // if the block has no health, destroy it
                if (_hp <= 0)
                {
                    BlockBreak();
                }

                // if the block isnt destroyed yet, get the percentage of its health a nd set the crack sprite
                float hpPercent = (float)_hp / (float)StartingHP;

                //Debug.Log($"Health: {_hp} | Percent: {hpPercent}");

                switch (hpPercent)
                {
                    case <= .2f:
                        CrackSprite.sprite = DamageSprits[2];

                        //Debug.Log($"Setting sprite to 0 {DamageSprits[2]}");

                        break;

                    case <= .5f:
                        CrackSprite.sprite = DamageSprits[1];

                        //Debug.Log($"Setting sprite to 0 {DamageSprits[1]}");

                        break;

                    case <= .8f:
                        CrackSprite.sprite = DamageSprits[0];

                       //Debug.Log($"Setting sprite to 0 {DamageSprits[0]}");

                        break;

                    default:
                        CrackSprite.sprite = DamageSprits[DamageSprits.Length - 1];
                        break;
                }
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        Health = StartingHP;
        MainSprite = GetComponent<SpriteRenderer>();
        CrackSprite = transform.Find("Crack").GetComponent<SpriteRenderer>();


    }

    IEnumerator InvincibleTime()
    {
        yield return new WaitForSeconds(IFrameTime);
        isInvincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BlockBreak()
    {
        //Debug.Log("Block Broken");

        switch (this.Type)
        {
            case RockType.Normal:

                break;

            case RockType.Fuel:

                GameObject plat = GameObject.Find("Platform");

                //Debug.Log($"Platform found: {plat}");

                if (plat != null)
                {
                    // 5 is temp value
                    plat.GetComponent<PlatformBehavior>().CurrentFuel += 5;
                }



                break;

            case RockType.Treasure:

                break;
        }

        Destroy(this.gameObject);

    }

    private void OnDestroy()
    {
        
    }
}
