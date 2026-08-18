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
    public int[] DamageLevels = new int[6];
    public float FireSpeed = .5f;
    public float[] ReloadSpeedLevels = new float[6];
    public int PierceCount = 1;
    public AK.Wwise.Event ShootEvent;

}
