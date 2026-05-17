using UnityEngine;

[CreateAssetMenu]
public class GunSettings : ScriptableObject
{
    public enum BulletMode
    {
        Rapid,
        Singular,
        Burst
    }

    public BulletMode Mode = BulletMode.Rapid;

    public Sprite GunSprite;
    public GameObject Bullet;
    public float FireSpeed = .5f;
    public AK.Wwise.Event ShootEvent;

}
